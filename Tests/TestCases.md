# Manual test cases

Run these against a disposable SQL Server 2022 database after applying all scripts. They are a verification plan, not evidence that the database workflow has already passed.

Run `powershell.exe -NoProfile -ExecutionPolicy Bypass -File Tests\VerifyArchitecture.ps1` for layer/reference checks, and `Tests\ValidateSqlSyntax.ps1` for offline SQL Server 2022 syntax parsing (requires SSMS 20 or VS SQL tools). After a Release build, run `Tests\VerifyStoredProcedureReferences.ps1` and `Tests\VerifyCore.ps1` for DAL procedure coverage, XLSX/password/calendar/authorization checks. Database workflows require a reachable SQL Server; use the app's **Test database connection** action first, then follow the manual scenarios below.

| Area | Steps | Expected result |
|---|---|---|
| Setup/auth | Create initial Admin; sign in; sign out; sign in with an incorrect password | Hash is stored, correct password succeeds, incorrect password is rejected, sign-in time and audit record are stored |
| Roles | Create Manager/Cashier/Staff accounts; try to create Admin as Manager; try direct stored-procedure account changes as Cashier or inactive actor; deactivate self/Admin as Manager | BUS and SQL reject unauthorized direct calls; role limits are enforced; inactive user cannot sign in |
| Order cancellation authorization | Attempt cancellation through BUS and `sp_Order_Cancel` as Cashier, Staff, inactive user and Manager/Admin | BUS and SQL reject unauthorized callers; active Manager/Admin can cancel only unpaid open orders |
| Audit actors | Create/disable a user and cancel an unpaid order | Activity log identifies the signed-in actor for account changes and order cancellation |
| Catalog | Create/edit product; search/filter; deactivate; delete category with an active product | CRUD/search works; soft-deleted product disappears; category deletion is rejected while active products exist |
| Employee filters | Search by name/code and filter by job title; edit an employee title and filter again | Search and title filter combine on SQL Server results; title choices refresh after edits |
| POS | Create dine-in and take-away order; apply an active/expired promotion; save; reopen in Orders | Order and detail rows are persisted; only an active promotion is applied; table state changes only for dine-in |
| Invoice filters | Filter invoices by date, employee and status, including a deactivated employee | Each filter narrows the SQL result without hiding historical invoices |
| Payment | Cash exact/change; transfer with/without reference; partial then final payment; duplicate/over payment; save a zero-total order | Tender/change match; blank transfer reference and zero-total order are rejected; balance and order status update once; overpayment is rejected |
| Transaction rollback | Submit order with one unavailable product; pay an order whose recipe needs more stock than on hand; attempt issue above stock | Procedure rolls back the full operation and leaves payment/order totals/stock unchanged |
| Table operations | Move open order; merge two unpaid orders; split quantities from multiple lines to another available table; try fractional quantities whose line rounding changes the total; try split of promoted/paid order | Table availability and source-plus-target totals remain consistent; split that creates a rounding difference is rolled back with a business message |
| Table reservation | Reserve an available table; release reservation; attempt to release a table with an open order; attempt to set an unsupported status; race a reservation against order creation | Available/Reserved changes are persisted; tables with open orders cannot be freed; manual status changes cannot set Occupied or Disabled; row locks prevent a table from becoming reserved while an order is created |
| Product images | Add/edit a product with an existing image file; save a missing path | Existing image path is saved and missing files are rejected by the UI |
| Inventory | Create/edit/delete units; try deleting a unit assigned to an ingredient; create/edit ingredient; receive; issue available amount; issue too much | Unit CRUD works subject to references; ingredient updates persist; on-hand view reflects movements; over-issue is rejected under transaction lock |
| Reports | Run daily/monthly/yearly, best seller, employee and invoice reports; export XLSX/CSV; preview and export PDF through Crystal with templates installed | Date filters and totals match paid invoice data; exports match returned report rows |
| Error handling | Disconnect SQL Server and retry sign-in/query | UI shows a generic safe error; details appear in local log |
| Activity log | Perform catalog, employee, table, inventory, order and payment actions; query the date range in Activity log | SQL AuditLogs contains actions and actor context; GUI lists newest entries first |
| Release | Extract release ZIP on a Windows machine without Visual Studio and launch | Application starts when .NET Framework 4.8 is present and configured SQL access is available |
| Backup/restore | Take a backup; run RESTORE VERIFYONLY; restore to a disposable database and validate seed/catalog/orders | Backup is readable; restore completes and database returns to MULTI_USER |

Record SQL Server version, script execution, operator, date and observed results when these are run. Do not mark a database scenario passed until it is exercised against SQL Server.
