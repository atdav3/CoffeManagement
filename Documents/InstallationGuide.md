# Installation Guide

## Requirements

- Windows 10/11 or Windows Server supported by .NET Framework 4.8.
- .NET Framework 4.8 runtime on client computers.
- SQL Server 2022 instance reachable from each client.
- SQL Server command-line utilities (`sqlcmd`) on the database setup machine.
- Windows account with permission to create the database and execute the scripts.

## Create the database

Open Command Prompt at the repository root and run:

```bat
Database\InstallDatabase.cmd .
```

Pass an instance name such as `SERVER\SQLEXPRESS` when needed. Scripts are for a fresh database and are applied in this order: database, tables/types, constraints marker, indexes, views, stored procedures, seed data. They are not a migration framework and should not be run over a customized production schema.

Edit `Database\Permissions.sql` to replace its example Windows group, then run it as a SQL Server administrator. Add the Windows group that will run the desktop application; the script grants stored-procedure execution and table-valued parameter type access without granting direct table writes.

For administrative backup/restore, use `Database\BackupDatabase.sql` and `Database\RestoreDatabase.sql` from SSMS under a DBA account. Edit the server-side backup path first; the SQL Server service account must be able to access it. Restore replaces the current `CoffeeManagementDB` contents and forces other sessions out. These scripts are deliberately separate from the desktop application role.

## Configure the application

Edit the `CoffeeManagementDB` entry in `CoffeeManagement.GUI\App.config` before deployment. It uses integrated Windows authentication, so grant the desktop users only the SQL permissions required to execute the application procedures and read the required views. Do not put SQL credentials in source control.

At the login screen, select **Test database connection** to verify the configured server and database. This performs a read-only account-count procedure call; it does not create or alter records. The supplied configuration targets `MSI\SQLEXPRESS`; change `Data Source` in `CoffeeManagement.GUI.exe.config` if the SQL Server instance has a different reachable name. The app reports a safe connection error and writes diagnostic details to the local application log.

Build a Release package by running `scripts\Build-Release.ps1`. The build runs the architecture verifier before creating the ZIP. Run `Tests\ValidateSqlSyntax.ps1`, `Tests\VerifyStoredProcedureReferences.ps1` and `Tests\VerifyCore.ps1` for the available offline checks. Copy/extract the ZIP to a 64-bit Windows machine with .NET Framework 4.8 and the matching 64-bit SAP Crystal Reports for Visual Studio runtime installed. No Visual Studio installation is required on the client. The SAP runtime installer is not bundled; obtain it from the [SAP Crystal Reports for Visual Studio resources](https://pages.community.sap.com/topics/crystal-reports/visual-studio). Crystal preview and PDF export also require the `.rpt` templates documented in `Reports/README.md`.

SAP's current compatibility listing names Visual Studio through 2022 for the Crystal designer, while this project is built with Visual Studio 2026. The application loads the .NET runtime dynamically, but report template authoring must use a SAP-supported designer environment; this repository does not include Crystal template binaries. The Release executable is AnyCPU with 32-bit preference disabled, so it runs 64-bit on 64-bit Windows to match SAP's current 64-bit runtime distribution.

## First launch

The login screen checks whether the database contains any users. If none exist, **First-time setup** creates an administrator with a PBKDF2-SHA256 password hash and a linked employee record. Subsequent staff and roles are managed through **Accounts**. The seed script creates catalog, tables, units and initial inventory but does not create a reusable account/password.

Application logs are written under `%LOCALAPPDATA%\CoffeeManagement\Logs`.
