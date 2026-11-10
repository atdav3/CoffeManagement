@echo off
setlocal
set "SQLSERVER=%~1"
if "%SQLSERVER%"=="" set "SQLSERVER=."
for %%S in (CreateDatabase.sql Tables.sql Constraints.sql Indexes.sql Views.sql StoredProcedures.sql SeedData.sql AuditTriggers.sql) do (
    echo Applying %%S to %SQLSERVER%...
    sqlcmd -S "%SQLSERVER%" -E -b -f i:65001 -i "%~dp0%%S"
    if errorlevel 1 (
        echo Failed while applying %%S.
        exit /b 1
    )
)
echo Database scripts applied successfully.
exit /b 0
