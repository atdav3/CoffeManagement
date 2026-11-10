using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class AuditLogForm : Form
    {
        private readonly AuditService service = new AuditService();
        private bool loading;

        public AuditLogForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            DateTime businessToday = new BusinessCalendarService().Today;
            start.Value = businessToday.AddDays(-30);
            end.Value = businessToday;

            refresh.Click += (s, e) => LoadRows();

            Shown += (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
                LoadRows();
            };
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplyPrimaryButton(refresh);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private async void LoadRows()
        {
            if (loading) return;
            loading = true;
            refresh.Enabled = false;
            start.Enabled = end.Enabled = grid.Enabled = false;
            refresh.Text = "Đang tải dữ liệu…";
            status.Text = "Đang truy vấn nhật ký hoạt động từ SQL Server…";
            Cursor = Cursors.WaitCursor;

            try
            {
                DateTime from = start.Value, to = end.Value;
                DataTable rows = await Task.Run(() => service.Get(from, to));
                if (IsDisposed || Disposing) return;

                grid.DataSource = rows;
                if (grid.Columns.Contains("AuditLogId")) grid.Columns["AuditLogId"].Visible = false;
                if (grid.Columns.Contains("Details")) grid.Columns["Details"].Visible = false;

                if (grid.Columns.Contains("CreatedAtUtc"))
                {
                    grid.Columns["CreatedAtUtc"].HeaderText = "Thời gian ghi nhận (UTC)";
                    grid.Columns["CreatedAtUtc"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    grid.Columns["CreatedAtUtc"].FillWeight = 110;
                }
                if (grid.Columns.Contains("ActionName"))
                {
                    grid.Columns["ActionName"].HeaderText = "Hành động / Thao tác";
                    grid.Columns["ActionName"].FillWeight = 90;
                }
                if (grid.Columns.Contains("EntityName"))
                {
                    grid.Columns["EntityName"].HeaderText = "Phân hệ / Bảng";
                    grid.Columns["EntityName"].FillWeight = 90;
                }
                if (grid.Columns.Contains("EntityId"))
                {
                    grid.Columns["EntityId"].HeaderText = "Mã bản ghi";
                    grid.Columns["EntityId"].FillWeight = 70;
                }
                if (grid.Columns.Contains("ActorOrDetails"))
                {
                    grid.Columns["ActorOrDetails"].HeaderText = "Người thực hiện / Chi tiết";
                    grid.Columns["ActorOrDetails"].FillWeight = 200;
                }

                status.Text = "✓ Đã tải " + rows.Rows.Count.ToString("N0") + " bản ghi nhật ký hoạt động.";
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                {
                    status.Text = "Không thể tải nhật ký hoạt động.";
                    UiError.Show(ex, this);
                }
            }
            finally
            {
                loading = false;
                if (!IsDisposed && !Disposing)
                {
                    refresh.Enabled = true;
                    start.Enabled = end.Enabled = grid.Enabled = true;
                    refresh.Text = "🔄 Tải nhật ký hệ thống";
                    Cursor = Cursors.Default;
                }
            }
        }
    }
}
