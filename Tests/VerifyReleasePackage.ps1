$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$archivePath = Join-Path $repoRoot 'artifacts\CoffeeManagement-Release.zip'
if (-not (Test-Path -LiteralPath $archivePath)) { throw "Release archive does not exist: $archivePath" }

Add-Type -AssemblyName System.IO.Compression.FileSystem
$archive = [IO.Compression.ZipFile]::OpenRead($archivePath)
try {
    $entries = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    foreach ($entry in $archive.Entries) {
        [void]$entries.Add($entry.FullName.Replace('/', '\'))
    }
    $required = @(
        'CoffeeManagement.GUI.exe', 'CoffeeManagement.GUI.exe.config',
        'CoffeeManagement.BUS.dll', 'CoffeeManagement.DAL.dll', 'CoffeeManagement.DTO.dll',
        'README.md', 'Database\InstallDatabase.cmd', 'Database\CreateDatabase.sql',
        'Database\Tables.sql', 'Database\StoredProcedures.sql', 'Database\SeedData.sql',
        'Database\BackupDatabase.sql', 'Database\RestoreDatabase.sql',
        'Documents\Architecture.md', 'Documents\DatabaseDesign.md',
        'Documents\InstallationGuide.md', 'Documents\UserGuide.md',
        'Reports\README.md', 'Tests\TestCases.md',
        'Tests\VerifyArchitecture.ps1', 'Tests\VerifyCore.ps1', 'Tests\VerifyReleasePackage.ps1'
    )
    $missing = @($required | Where-Object { -not $entries.Contains($_) })
    if ($missing.Count -gt 0) { throw "Release archive is missing required entries: $($missing -join ', ')" }
    if ($entries.Contains('UserGuide.md')) { throw 'Release archive contains a stale root-level UserGuide.md.' }
    Write-Output "Release package check passed: $($required.Count) required entries found in $archivePath."
} finally { $archive.Dispose() }
