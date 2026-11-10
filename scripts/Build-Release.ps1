$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$vswhere = Join-Path ([Environment]::GetFolderPath('ProgramFilesX86')) 'Microsoft Visual Studio\Installer\vswhere.exe'
if (-not (Test-Path -LiteralPath $vswhere)) { throw 'Visual Studio Installer vswhere.exe was not found.' }
$vsPath = & $vswhere -version '[18.0,19.0)' -products '*' -requires Microsoft.Component.MSBuild -property installationPath
if (-not $vsPath) { throw 'Visual Studio 2026 MSBuild installation was not found.' }
$msbuild = Join-Path $vsPath 'MSBuild\Current\Bin\MSBuild.exe'
$solution = Join-Path $repoRoot 'CoffeeManagement.sln'
& $msbuild $solution /t:Rebuild /p:Configuration=Release /nr:false /m:1 /v:minimal -noAutoResponse
if ($LASTEXITCODE -ne 0) { throw "Release build failed with exit code $LASTEXITCODE." }
powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $repoRoot 'Tests\VerifyArchitecture.ps1')
if ($LASTEXITCODE -ne 0) { throw "Architecture verification failed with exit code $LASTEXITCODE." }

$artifactRoot = Join-Path $repoRoot 'artifacts'
$stage = Join-Path $artifactRoot 'CoffeeManagement-Release'
$guiOutput = Join-Path $repoRoot 'CoffeeManagement.GUI\bin\Release'
$artifactRootFull = [System.IO.Path]::GetFullPath($artifactRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
$stageFull = [System.IO.Path]::GetFullPath($stage)
if (-not $stageFull.StartsWith($artifactRootFull, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "Refusing to clean package staging path outside artifacts: $stageFull"
}
if (Test-Path -LiteralPath $stageFull) { Remove-Item -LiteralPath $stageFull -Recurse -Force }
New-Item -ItemType Directory -Force -Path $stageFull | Out-Null
Copy-Item -Path (Join-Path $guiOutput '*') -Destination $stage -Recurse -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'README.md') -Destination $stage -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'Documents') -Destination $stage -Recurse -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'Tests') -Destination $stage -Recurse -Force
Copy-Item -LiteralPath (Join-Path $repoRoot 'Database') -Destination $stage -Recurse -Force
$reportTemplates = Join-Path $repoRoot 'Reports'
if (Test-Path -LiteralPath $reportTemplates) { Copy-Item -LiteralPath $reportTemplates -Destination (Join-Path $stage 'Reports') -Recurse -Force }
$zip = Join-Path $artifactRoot 'CoffeeManagement-Release.zip'
Compress-Archive -Path (Join-Path $stage '*') -DestinationPath $zip -Force
powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $repoRoot 'Tests\VerifyReleasePackage.ps1')
if ($LASTEXITCODE -ne 0) { throw "Release package verification failed with exit code $LASTEXITCODE." }
Write-Output "Release package created: $zip"
