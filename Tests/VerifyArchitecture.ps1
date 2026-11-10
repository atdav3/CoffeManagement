$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$projects = @{
    GUI = Join-Path $repoRoot 'CoffeeManagement.GUI\CoffeeManagement.GUI.csproj'
    BUS = Join-Path $repoRoot 'CoffeeManagement.BUS\CoffeeManagement.BUS.csproj'
    DAL = Join-Path $repoRoot 'CoffeeManagement.DAL\CoffeeManagement.DAL.csproj'
    DTO = Join-Path $repoRoot 'CoffeeManagement.DTO\CoffeeManagement.DTO.csproj'
}
$expectedReferences = @{
    GUI = @('CoffeeManagement.BUS', 'CoffeeManagement.DTO')
    BUS = @('CoffeeManagement.DAL', 'CoffeeManagement.DTO')
    DAL = @('CoffeeManagement.DTO')
    DTO = @()
}

foreach ($name in $projects.Keys) {
    $path = $projects[$name]
    $content = Get-Content -LiteralPath $path -Raw
    if ($content -notmatch '<TargetFrameworkVersion>v4\.8</TargetFrameworkVersion>') { throw "$name does not target .NET Framework 4.8." }
    $actual = @([regex]::Matches($content, '<ProjectReference\s+Include="([^"]+)"') | ForEach-Object {
        [IO.Path]::GetFileNameWithoutExtension($_.Groups[1].Value)
    } | Sort-Object -Unique)
    $expected = @($expectedReferences[$name] | Sort-Object -Unique)
    if (Compare-Object -ReferenceObject $expected -DifferenceObject $actual) {
        throw "$name references are incorrect. Expected [$($expected -join ', ')], found [$($actual -join ', ')]."
    }
}

$guiSources = Get-ChildItem -LiteralPath (Join-Path $repoRoot 'CoffeeManagement.GUI') -Filter '*.cs' -Recurse
foreach ($file in $guiSources) {
    if (Select-String -LiteralPath $file.FullName -Pattern 'System\.Data\.SqlClient|\bSqlConnection\b|\bSqlCommand\b' -Quiet) {
        throw "GUI source contains direct SQL access: $($file.FullName)"
    }
}
$busSources = Get-ChildItem -LiteralPath (Join-Path $repoRoot 'CoffeeManagement.BUS') -Filter '*.cs' -Recurse
foreach ($file in $busSources) {
    if (Select-String -LiteralPath $file.FullName -Pattern 'System\.Data\.SqlClient|\bSqlConnection\b|\bSqlCommand\b' -Quiet) {
        throw "BUS source contains direct SQL access: $($file.FullName)"
    }
}

$dalSources = Get-ChildItem -LiteralPath (Join-Path $repoRoot 'CoffeeManagement.DAL\Repositories') -Filter '*.cs' -Recurse
$commandCount = 0
foreach ($file in $dalSources) {
    $source = Get-Content -LiteralPath $file.FullName -Raw
    foreach ($match in [regex]::Matches($source, 'new\s+SqlCommand\s*\(\s*"([^"]+)"')) {
        $commandCount++
        if ($match.Groups[1].Value -notmatch '^dbo\.sp_[A-Za-z0-9_]+$') {
            throw "DAL contains a non-procedure SQL command literal in $($file.Name): $($match.Groups[1].Value)"
        }
    }
}
if ($commandCount -lt 1) { throw 'No stored-procedure SqlCommand calls were found in DAL repositories.' }
Write-Output "Architecture checks passed: .NET Framework 4.8 references are layered correctly; GUI/BUS contain no direct SQL; $commandCount static DAL commands name stored procedures."
