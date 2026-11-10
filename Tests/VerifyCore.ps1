$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$guiAssemblyPath = Join-Path $repoRoot 'CoffeeManagement.GUI\bin\Release\CoffeeManagement.GUI.exe'
$busAssemblyPath = Join-Path $repoRoot 'CoffeeManagement.BUS\bin\Release\CoffeeManagement.BUS.dll'
if (-not (Test-Path -LiteralPath $guiAssemblyPath) -or -not (Test-Path -LiteralPath $busAssemblyPath)) {
    throw 'Build Release first by running scripts\Build-Release.ps1.'
}

$tempPath = Join-Path ([IO.Path]::GetTempPath()) ('CoffeeManagement-' + [Guid]::NewGuid().ToString('N') + '.xlsx')
try {
    Add-Type -AssemblyName System.IO.Compression
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $table = New-Object System.Data.DataTable 'Report'
    [void]$table.Columns.Add('Product', [string])
    [void]$table.Columns.Add('Revenue', [decimal])
    [void]$table.Columns.Add('UntrustedText', [string])
    [void]$table.Rows.Add('Espresso', [decimal]1234.50, '=1+1')

    $guiAssembly = [Reflection.Assembly]::LoadFrom($guiAssemblyPath)
    $exporter = $guiAssembly.GetType('CoffeeManagement.GUI.Helpers.ExcelExporter', $true)
    $write = $exporter.GetMethod('Write', [Reflection.BindingFlags]'Static, Public, NonPublic')
    $arguments = New-Object object[] 2
    $arguments[0] = $table.PSObject.BaseObject
    $arguments[1] = $tempPath.PSObject.BaseObject
    [void]$write.Invoke($null, $arguments)

    $archive = [IO.Compression.ZipFile]::OpenRead($tempPath)
    try {
        $required = @('[Content_Types].xml', '_rels/.rels', 'xl/workbook.xml', 'xl/_rels/workbook.xml.rels', 'xl/worksheets/sheet1.xml')
        foreach ($entryName in $required) {
            if (-not $archive.GetEntry($entryName)) { throw "XLSX package is missing $entryName" }
        }
        $sheetEntry = $archive.GetEntry('xl/worksheets/sheet1.xml')
        $sheetReader = New-Object IO.StreamReader($sheetEntry.Open())
        try { [xml]$sheet = $sheetReader.ReadToEnd() } finally { $sheetReader.Dispose() }
        $namespace = New-Object Xml.XmlNamespaceManager($sheet.NameTable)
        $namespace.AddNamespace('x', 'http://schemas.openxmlformats.org/spreadsheetml/2006/main')
        $texts = @($sheet.SelectNodes('//x:t', $namespace) | ForEach-Object { $_.InnerText })
        if ($texts -notcontains 'Product' -or $texts -notcontains 'Espresso' -or $texts -notcontains '=1+1') { throw 'XLSX cell text did not round-trip.' }
        if ($sheet.SelectNodes('//x:f', $namespace).Count -ne 0) { throw 'Untrusted text was emitted as a spreadsheet formula.' }
    } finally { $archive.Dispose() }

    $busAssembly = [Reflection.Assembly]::LoadFrom($busAssemblyPath)
    $hasherType = $busAssembly.GetType('CoffeeManagement.BUS.Services.PasswordHasher', $true)
    $hasher = [Activator]::CreateInstance($hasherType)
    $encoded = $hasher.Hash('Coffee-Shop-Passphrase-42')
    if (-not $hasher.Verify('Coffee-Shop-Passphrase-42', $encoded)) { throw 'Correct password did not verify.' }
    if ($hasher.Verify('another-passphrase', $encoded)) { throw 'Incorrect password unexpectedly verified.' }
    if ($hasher.Verify('anything', 'PBKDF2-SHA256$999999999999999999999999$AA==$AA==')) { throw 'Malformed iteration count unexpectedly verified.' }
    if (-not $encoded.StartsWith('PBKDF2-SHA256$')) { throw 'Password hash has an unexpected format.' }

    $dtoAssembly = [Reflection.Assembly]::LoadFrom((Join-Path $repoRoot 'CoffeeManagement.DTO\bin\Release\CoffeeManagement.DTO.dll'))
    $sessionType = $dtoAssembly.GetType('CoffeeManagement.DTO.Entities.UserSessionDto', $true)
    $authType = $busAssembly.GetType('CoffeeManagement.BUS.Services.AuthenticationService', $true)
    $orderType = $busAssembly.GetType('CoffeeManagement.BUS.Services.OrderService', $true)
    $calendarType = $busAssembly.GetType('CoffeeManagement.BUS.Services.BusinessCalendarService', $true)
    $calendar = [Activator]::CreateInstance($calendarType)
    $businessZone = [TimeZoneInfo]::FindSystemTimeZoneById('SE Asia Standard Time')
    $expectedBusinessDate = [TimeZoneInfo]::ConvertTimeFromUtc([DateTime]::UtcNow, $businessZone).Date
    if ($calendar.Today -ne $expectedBusinessDate) { throw 'Business calendar did not return the configured default business date.' }
    $businessExceptionName = 'CoffeeManagement.BUS.Exceptions.BusinessException'
    foreach ($roleName in @('Cashier', 'Staff')) {
        $session = [Activator]::CreateInstance($sessionType)
        $session.UserId = 17
        $session.RoleName = $roleName
        $auth = [Activator]::CreateInstance($authType)
        foreach ($case in @('create', 'disable')) {
            $rejected = $false
            try {
                if ($case -eq 'create') {
                    $invokeArgs = [object[]]@($session, 'blocked-user', 'Valid-Passphrase-42', 'Staff', 8)
                    [void]$authType.GetMethod('CreateAccount').Invoke($auth, $invokeArgs)
                } else {
                    $invokeArgs = [object[]]@($session, 8, $false, 'Staff')
                    [void]$authType.GetMethod('SetAccountActive').Invoke($auth, $invokeArgs)
                }
            } catch {
                $cause = $_.Exception
                while ($cause.GetType().FullName -ne $businessExceptionName -and $cause.InnerException) { $cause = $cause.InnerException }
                if ($cause.GetType().FullName -ne $businessExceptionName) { throw $cause }
                $rejected = $true
            }
            if (-not $rejected) { throw "$roleName unexpectedly passed the account $case authorization check." }
        }
        $order = [Activator]::CreateInstance($orderType)
        $rejected = $false
        try {
            [void]$orderType.GetMethod('Cancel').Invoke($order, [object[]]@([long]1, $session))
        } catch {
            $cause = $_.Exception
            while ($cause.GetType().FullName -ne $businessExceptionName -and $cause.InnerException) { $cause = $cause.InnerException }
            if ($cause.GetType().FullName -ne $businessExceptionName) { throw $cause }
            $rejected = $true
        }
        if (-not $rejected) { throw "$roleName unexpectedly passed the order cancellation authorization check." }
    }

    Write-Output 'Core checks passed: XLSX package, PBKDF2, business calendar, and unauthorized BUS role calls.'
} finally {
    if (Test-Path -LiteralPath $tempPath) { Remove-Item -LiteralPath $tempPath -Force }
}
