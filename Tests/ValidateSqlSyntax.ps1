$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$programFilesX86 = [Environment]::GetFolderPath('ProgramFilesX86')
$programFiles = [Environment]::GetFolderPath('ProgramFiles')
$candidates = @(
    (Join-Path $programFilesX86 'Microsoft SQL Server Management Studio 20\Common7\IDE\Extensions\Application\Microsoft.SqlServer.TransactSql.ScriptDom.dll'),
    (Join-Path $programFiles 'Microsoft Visual Studio\18\Community\Common7\IDE\Extensions\Microsoft\SQLCommon\Microsoft.SqlServer.TransactSql.ScriptDom.dll')
)
$parserPath = $candidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $parserPath) { throw 'Install SSMS 20 or VS SQL tools to use the offline SQL syntax parser.' }
$assembly = [Reflection.Assembly]::LoadFrom($parserPath)
$parserType = $assembly.GetType('Microsoft.SqlServer.TransactSql.ScriptDom.TSql160Parser', $true)
$parser = [Activator]::CreateInstance($parserType, [object[]]@($true))
$failed = $false
$sqlFiles = @(Get-ChildItem -LiteralPath (Join-Path $repoRoot 'Database') -Filter '*.sql')
foreach ($file in $sqlFiles) {
    $script = Get-Content -LiteralPath $file.FullName -Raw
    $batches = $script -split '(?im)^\s*GO\s*(?:--.*)?$'
    $batchNumber = 0
    foreach ($batch in $batches) {
        if ([string]::IsNullOrWhiteSpace($batch)) { continue }
        $batchNumber++
        $reader = New-Object IO.StringReader($batch)
        $parseErrors = $null
        [void]$parser.Parse($reader, [ref]$parseErrors)
        $reader.Dispose()
        foreach ($error in $parseErrors) {
            Write-Error ("{0}, batch {1}, line {2}, column {3}: {4}" -f $file.Name, $batchNumber, $error.Line, $error.Column, $error.Message) -ErrorAction Continue
            $failed = $true
        }
    }
}
if ($failed) { throw 'One or more SQL script batches failed the T-SQL 160 syntax parser.' }
Write-Output 'Database setup batches parsed successfully as SQL Server 2022 T-SQL.'
