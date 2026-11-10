# Architecture

CoffeeManagement is a Windows Forms desktop application targeting .NET Framework 4.8. It uses a traditional three-layer dependency direction:

```text
CoffeeManagement.GUI  →  CoffeeManagement.BUS  →  CoffeeManagement.DAL
            └──────────────────→ CoffeeManagement.DTO ←────────────┘
```

- **GUI** contains WinForms, presentation validation, navigation and local exception display. Forms call BUS services; they do not issue SQL.
- **BUS** validates inputs, enforces account-management policy, hashes passwords with PBKDF2-SHA256 and delegates persistence to repositories.
- **DAL** owns configuration-based `SqlConnection` creation, parameterized stored-procedure calls, `SqlDataReader`, `SqlDataAdapter`, `DataTable`/`DataSet` and SQL transaction boundaries implemented in stored procedures.
- **DTO** contains the entities shared between layers.

`Tests/VerifyArchitecture.ps1` checks project references, .NET Framework 4.8 targets, GUI/BUS SQL isolation and static DAL command literals. `scripts/Build-Release.ps1` runs this check after the Release build.

The payment procedure locks the open invoice, prevents overpayment, inserts the payment, records cash tender/change and marks the order paid when the outstanding balance reaches zero, all within one SQL transaction. Inventory issues lock the ingredient row and reject quantities above on-hand stock. Order save/split/merge/transfer procedures maintain table state and record audit events. Database triggers record category, product, employee, table, promotion, ingredient and unit changes, including the SQL Server login that made the change.

Account creation/status procedures independently verify that the acting user is active and has the Admin or Manager role; they also prevent Managers from changing Admin accounts. Order cancellation is checked in both BUS and SQL so hiding its button in the UI is not the only authorization control.

No ORM, web framework or external UI library is used. The WinForms chart control is part of the .NET Framework desktop assemblies. SQL connection details are in `CoffeeManagement.GUI/App.config`; the checked-in default uses Windows Integrated Security and contains no password.

Persisted timestamps use UTC (`SYSUTCDATETIME`). Date filters and daily summaries use the configured business zone (`BusinessTimeZoneId`, default `SE Asia Standard Time`) so a Vietnamese business day remains consistent at midnight. Change that application setting and the SQL expressions together if deploying to a different business timezone.
