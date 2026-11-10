# User Guide

## First administrator

After the SQL scripts have been installed and the connection string configured, open CoffeeManagement and select **First-time setup**. Choose a username and a password of at least 10 characters. Sign in with that account.

## Catalog and team setup

Admins and Managers can add product categories and products, choose a product image file, manage employees, tables, units and ingredients. On the Tables screen, **Reserve / release** toggles Available and Reserved; open orders cannot be released as available. Use **Accounts** to link an employee to a login and assign Admin, Manager, Cashier or Staff. A Manager cannot create or disable Admin accounts. Disabling an employee also disables the linked login.

Admins and Managers can also configure discount promotions with start/end times. Active promotions appear in POS; the database validates their window and computes the invoice discount.

## Point of sale

Open **Point of sale**, choose an available table or Take-away, search products and add items. **Save and pay** stores the order in SQL Server. If the payment dialog is cancelled, the open order remains in **Orders**. Enter the payment amount separately from cash received; partial payments leave a balance that can be paid later in Orders. Cash payments record tender and change; transfers require a reference. Orders must have a positive total.

## Orders

Use date, employee and status filters to search invoices. **Details** shows saved line snapshots and totals. Open orders can receive payment, move to an available table, merge into another open order, split quantities from multiple product lines into a new order or be cancelled. For a split, enter the quantity to move on each desired product row and leave other rows at zero. The source order must retain at least one item, and paid or promoted orders cannot be split.

## Inventory

Add or edit ingredients with a unit and reorder level. **Manage units** lets managers maintain measurement units; a unit assigned to an ingredient cannot be deleted. Record receipts and issues from the Inventory screen. The database rejects issue quantities that exceed current stock. Rows at or below reorder level are highlighted.
On final payment, recipe quantities are issued from inventory in the same database transaction. If recipe stock is insufficient, the payment and invoice status roll back together. Use **History** to inspect inventory movements.

## Reports

Select a report type and date range, then run it. Reports query paid sales and invoice rows from SQL Server. Export `.xlsx` or CSV directly, or use Crystal preview/PDF when the matching SAP runtime and report templates are installed. The regular print action remains available through an installed printer.

## Sign out and errors

**Sign out** closes the current application session. User-facing errors omit SQL details; diagnostic details are stored in the local application log.

Admins and Managers can open **Activity log** to inspect recent events by date range. Catalog, employee, table, promotion, ingredient and unit changes are recorded in SQL `AuditLogs` with the SQL Server login that executed the change. Sign-in, payment, order and inventory movements record their business event details.

Database backup and restore are DBA operations. Use the provided SQL scripts from SSMS; the regular application role does not have server-level restore rights.
