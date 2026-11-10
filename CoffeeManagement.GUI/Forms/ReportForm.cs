using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Enums;
using CoffeeManagement.GUI.Helpers;

using System.ComponentModel;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class ReportForm : Form
    {
        private readonly ReportService reports = new ReportService();
        private bool loading;
        private readonly PrintDocument printDocument = new PrintDocument();
        private int printRow;

        public ReportForm()
        {
            InitializeComponent();
            UITheme.ApplyGridStyle(grid);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };

            kind.Items.Add(new ReportChoice(ReportKind.SalesByDay, "📊 Doanh thu theo ngày"));
            kind.Items.Add(new ReportChoice(ReportKind.SalesByMonth, "📅 Doanh thu theo tháng"));
            kind.Items.Add(new ReportChoice(ReportKind.SalesByYear, "📆 Doanh thu theo năm"));
            kind.Items.Add(new ReportChoice(ReportKind.BestSellingProducts, "☕ Món bán chạy nhất"));
            kind.Items.Add(new ReportChoice(ReportKind.SalesByEmployee, "👤 Doanh thu theo nhân viên"));
            kind.Items.Add(new ReportChoice(ReportKind.Invoices, "🧾 Danh sách hóa đơn"));
            if (kind.Items.Count > 0) kind.SelectedIndex = 0;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && !DesignMode)
            {
                DateTime businessToday = new BusinessCalendarService().Today;
                start.Value = businessToday.AddDays(-30);
                end.Value = businessToday;

                printDocument.PrintPage += PrintPage;
                runButton.Click += (s, e) => LoadReport();
                export.Click += (s, e) => ExportCsv();
                pdf.Click += (s, e) => ExportCrystalPdf();
                print.Click += (s, e) => PrintReport();
                kind.SelectedIndexChanged += (s, e) => LoadReport();
                Shown += (s, e) => LoadReport();
            }
        }

        private async void LoadReport()
        {
            if (loading) return;
            loading = true;
            runButton.Enabled = false;
            Cursor = Cursors.WaitCursor;
            status.Text = "Đang tổng hợp dữ liệu báo cáo từ SQL Server…";

            try
            {
                ReportChoice selected = kind.SelectedItem as ReportChoice;
                if (selected == null) { status.Text = "Vui lòng chọn loại báo cáo."; return; }

                ReportKind reportKind = selected.Kind;
                DateTime reportStart = start.Value, reportEnd = end.Value;
                DataTable result = await Task.Run(() => reports.Get(reportKind, reportStart, reportEnd));
                if (IsDisposed || Disposing) return;

                grid.DataSource = result;

                // Localize header names dynamically
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    if (column.ValueType == typeof(decimal))
                        column.DefaultCellStyle.Format = "N0";

                    if (column.Name.Equals("SalesDate", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Ngày bán";
                    else if (column.Name.Equals("SalesMonth", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Tháng";
                    else if (column.Name.Equals("SalesYear", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Năm";
                    else if (column.Name.Equals("Revenue", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Doanh thu (₫)";
                    else if (column.Name.Equals("InvoiceCount", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Số hóa đơn";
                    else if (column.Name.Equals("ItemsSold", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Món đã bán";
                    else if (column.Name.Equals("ProductName", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Tên món / Đồ uống";
                    else if (column.Name.Equals("CategoryName", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Danh mục";
                    else if (column.Name.Equals("QuantitySold", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Số lượng bán";
                    else if (column.Name.Equals("EmployeeName", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Nhân viên";
                    else if (column.Name.Equals("OrderCode", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Mã hóa đơn";
                    else if (column.Name.Equals("TotalAmount", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Tổng tiền (₫)";
                    else if (column.Name.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase)) column.HeaderText = "Thời gian tạo";
                }

                status.Text = "✓ Đã tải " + grid.Rows.Count.ToString("N0") + " dòng dữ liệu (" + reportStart.ToString("dd/MM/yyyy") + " đến " + reportEnd.ToString("dd/MM/yyyy") + ")";
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                {
                    status.Text = "Không thể tải báo cáo.";
                    UiError.Show(ex, this);
                }
            }
            finally
            {
                loading = false;
                if (!IsDisposed && !Disposing)
                {
                    runButton.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
        }

        private void ExportCsv()
        {
            if (grid.DataSource == null)
            {
                MessageBox.Show(this, "Vui lòng xem báo cáo trước khi xuất dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|CSV for Excel (*.csv)|*.csv",
                FileName = "BaoCaoDoanhThu-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    DataTable data = grid.DataSource as DataTable ?? ((DataView)grid.DataSource).Table;
                    if (dialog.FilterIndex == 1)
                    {
                        ExcelExporter.Write(data, dialog.FileName);
                    }
                    else
                    {
                        StringBuilder csv = new StringBuilder();
                        for (int c = 0; c < data.Columns.Count; c++)
                        {
                            if (c > 0) csv.Append(',');
                            csv.Append(Escape(data.Columns[c].ColumnName));
                        }
                        csv.AppendLine();
                        foreach (DataRow row in data.Rows)
                        {
                            for (int c = 0; c < data.Columns.Count; c++)
                            {
                                if (c > 0) csv.Append(',');
                                csv.Append(Escape(row[c]));
                            }
                            csv.AppendLine();
                        }
                        File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                    }
                    MessageBox.Show(this, "Xuất báo cáo thành công!\nTệp đã được lưu tại:\n" + dialog.FileName, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    UiError.Show(ex, this);
                }
            }
        }

        private static string Escape(object value)
        {
            string text = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? "";
            Type type = value == null || value == DBNull.Value ? typeof(string) : value.GetType();
            bool numeric = type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal);
            if (!numeric && text.Length > 0 && "=+-@\t\r".IndexOf(text[0]) >= 0) text = "'" + text;
            return "\"" + text.Replace("\"", "\"\"").Replace("\r", " ").Replace("\n", " ") + "\"";
        }

        private void PrintReport()
        {
            if (grid.DataSource == null)
            {
                MessageBox.Show(this, "Vui lòng xem báo cáo trước khi in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (PrintDialog dialog = new PrintDialog { Document = printDocument, UseEXDialog = true })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                printRow = 0;
                try { printDocument.Print(); }
                catch (Exception ex) { UiError.Show(ex, this); }
            }
        }

        private DataTable CurrentData()
        {
            if (grid.DataSource == null) return null;
            return grid.DataSource as DataTable ?? ((DataView)grid.DataSource).Table;
        }

        private string CurrentReportName()
        {
            ReportChoice choice = kind.SelectedItem as ReportChoice;
            return choice == null ? "Report" : choice.Kind.ToString();
        }

        private void ExportCrystalPdf()
        {
            DataTable data = CurrentData();
            if (data == null)
            {
                MessageBox.Show(this, "Vui lòng xem báo cáo trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf",
                FileName = "BaoCao-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    CrystalReportsBridge.ExportPdf(data, CurrentReportName(), dialog.FileName);
                    MessageBox.Show(this, "Đã xuất báo cáo PDF thành công!\n" + dialog.FileName, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    UiError.Show(ex, this);
                }
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            DataTable data = grid.DataSource as DataTable ?? ((DataView)grid.DataSource).Table;
            float x = e.MarginBounds.Left, y = e.MarginBounds.Top;
            using (Font heading = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (Font body = new Font("Segoe UI", 8.5F))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString("COFFEE MANAGEMENT · " + ((ReportChoice)kind.SelectedItem).Name, heading, brush, x, y);
                y += 30;
                e.Graphics.DrawString("Thời gian: " + start.Value.ToString("dd/MM/yyyy") + " - " + end.Value.ToString("dd/MM/yyyy"), body, brush, x, y);
                y += 24;

                while (printRow < data.Rows.Count && y < e.MarginBounds.Bottom - 24)
                {
                    string line = "";
                    foreach (DataColumn column in data.Columns)
                        line += (line.Length == 0 ? "" : "   |   ") + column.ColumnName + ": " + Convert.ToString(data.Rows[printRow][column]);
                    e.Graphics.DrawString(line, body, brush, new RectangleF(x, y, e.MarginBounds.Width, 50));
                    y += 50;
                    printRow++;
                }
            }
            e.HasMorePages = printRow < data.Rows.Count;
        }

        private sealed class ReportChoice
        {
            public ReportKind Kind { get; private set; }
            public string Name { get; private set; }
            public ReportChoice(ReportKind kind, string name) { Kind = kind; Name = name; }
            public override string ToString() { return Name; }
        }
    }
}
