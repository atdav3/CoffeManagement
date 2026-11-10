# Crystal Reports templates

The application loads a `.rpt` file named after the selected report kind:

- `SalesByDay.rpt`
- `SalesByMonth.rpt`
- `SalesByYear.rpt`
- `BestSellingProducts.rpt`
- `SalesByEmployee.rpt`
- `Invoices.rpt`

Each template must use a single ADO.NET `DataTable` as its data source, with the field names and types returned by the corresponding report stored procedure. The application supplies that live `DataTable` at preview/export time. Add finished `.rpt` files to this directory; the Release build copies them beside the executable in `Reports`.

No template binaries are included yet. Author and validate them in a SAP-supported Crystal Reports designer before distributing them. The application's Crystal preview and PDF actions report a clear missing-template/runtime error rather than substituting mock data.
