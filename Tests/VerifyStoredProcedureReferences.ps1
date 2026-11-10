$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$dalRoot = Join-Path $repoRoot 'CoffeeManagement.DAL'
$procedureScript = Join-Path $repoRoot 'Database\StoredProcedures.sql'
$referenced = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
foreach ($file in Get-ChildItem -LiteralPath $dalRoot -Filter '*.cs' -Recurse) {
    $source = Get-Content -LiteralPath $file.FullName -Raw
    foreach ($match in [regex]::Matches($source, 'dbo\.sp_[A-Za-z0-9_]+')) { [void]$referenced.Add($match.Value) }
}
$defined = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$sql = Get-Content -LiteralPath $procedureScript -Raw
foreach ($match in [regex]::Matches($sql, 'CREATE\s+OR\s+ALTER\s+PROCEDURE\s+(dbo\.sp_[A-Za-z0-9_]+)', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)) { [void]$defined.Add($match.Groups[1].Value) }
$missing = @($referenced | Where-Object { -not $defined.Contains($_) })
if ($missing.Count -gt 0) { throw "DAL procedure references missing from StoredProcedures.sql: $($missing -join ', ')" }
Write-Output "Stored procedure coverage passed: $($referenced.Count) DAL procedure names are defined."
