# CoffeeManagement

Coffee shop management desktop application for .NET Framework 4.8, WinForms, ADO.NET and SQL Server 2022.

## Solution

- `CoffeeManagement.GUI` — WinForms login/setup, dashboard, POS, catalog, employees, accounts, tables, inventory, orders and reports.
- `CoffeeManagement.BUS` — validation, authentication, password hashing and business services.
- `CoffeeManagement.DAL` — SQL Server stored-procedure repositories.
- `CoffeeManagement.DTO` — shared transfer objects.
- `Database` — fresh database creation scripts and seed data.
- `Documents` — architecture, database and user/install guidance.

Open `CoffeeManagement.sln` in Visual Studio 2026. The solution targets .NET Framework 4.8; no NuGet dependencies are required for the current implementation.

## Quick setup

1. Install SQL Server 2022 and enable Windows authentication for the Windows user that will run the application.
2. Run `Database\InstallDatabase.cmd [SQLServerInstance]` from a command prompt with `sqlcmd` available. The default server argument is `.`.
3. Update the `CoffeeManagementDB` connection string in `CoffeeManagement.GUI\App.config` if SQL Server is not the local default instance.
4. Build and launch `CoffeeManagement.GUI`. Use **First-time setup** to create the first administrator password; no default password is seeded.
5. Create employee records and their login accounts, then use POS.

See [Installation Guide](Documents/InstallationGuide.md), [User Guide](Documents/UserGuide.md), [Database Design](Documents/DatabaseDesign.md), [Architecture](Documents/Architecture.md) and [Test Cases](Tests/TestCases.md).

## Data export and reporting

The Reports screen reads SQL data and exports `.xlsx` or CSV. Crystal preview and PDF export are wired to the SAP runtime and use templates from `Reports`; the SAP runtime and `.rpt` templates are external prerequisites and are not bundled. See [Installation Guide](Documents/InstallationGuide.md) and [report template notes](Reports/README.md).

## Build package

Run `scripts\Build-Release.ps1` in PowerShell from the repository root to build Release and create `artifacts\CoffeeManagement-Release.zip`.

## Current verification status

The Release solution builds with Visual Studio 2026 MSBuild. `scripts/Build-Release.ps1` also runs `Tests/VerifyArchitecture.ps1` to check the .NET Framework 4.8 targets, project-reference direction and SQL isolation. Offline checks parse SQL Server 2022 batches, compare all 49 DAL procedure references with SQL definitions, exercise XLSX/PBKDF2/business-calendar behavior and confirm unauthorized BUS role calls are rejected. No SQL Server instance is reachable in the current environment (both `.\SQLEXPRESS` and `(localdb)\MSSQLLocalDB` fail connection), so database installation, database authorization, transactions and UI database workflows still require execution against SQL Server. Crystal preview/PDF also require an external SAP runtime and six `.rpt` templates, which are not bundled. See `Tests/TestCases.md` for the database verification plan.
