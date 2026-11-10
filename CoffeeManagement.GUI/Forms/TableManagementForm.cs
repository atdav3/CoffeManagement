using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class TableManagementForm : Form
    {
        private readonly CatalogService service = new CatalogService();

        public TableManagementForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnAdd.Click += (s, e) => Edit(null);
            btnEdit.Click += (s, e) => Edit(Selected());
            btnReserve.Click += ToggleReservation;
            btnDeactivate.Click += DeactivateSelected;
            grid.CellDoubleClick += (s, e) => Edit(Selected());

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "TableStatus" && e.Value != null)
                {
                    string st = Convert.ToString(e.Value);
                    if (st == "Available")
                    {
                        e.Value = "🟢 Trống (Sẵn sàng đón khách)";
                        e.CellStyle.ForeColor = UITheme.Success;
                    }
                    else if (st == "Occupied")
                    {
                        e.Value = "🔴 Đang có khách phục vụ";
                        e.CellStyle.ForeColor = UITheme.Danger;
                    }
                    else if (st == "Reserved")
                    {
                        e.Value = "🟡 Đã đặt trước";
                        e.CellStyle.ForeColor = UITheme.WarmAmber;
                    }
                    e.FormattingApplied = true;
                }
            };

            Shown += (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
                LoadData();
            };
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplySuccessButton(btnAdd);
            UITheme.ApplyPrimaryButton(btnEdit);
            UITheme.ApplyWarningButton(btnReserve);
            UITheme.ApplyDangerButton(btnDeactivate);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private CafeTableDto Selected()
        {
            if (grid.CurrentRow == null) return null;
            DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;

            return new CafeTableDto
            {
                TableId = Convert.ToInt32(row["TableId"]),
                TableName = Convert.ToString(row["TableName"]),
                AreaName = Convert.ToString(row["AreaName"]),
                TableStatus = Convert.ToString(row["TableStatus"])
            };
        }

        private void LoadData()
        {
            try
            {
                DataTable tables = service.GetTables();
                grid.DataSource = tables;

                if (grid.Columns.Contains("TableId")) grid.Columns["TableId"].Visible = false;
                if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"].Visible = false;

                if (grid.Columns.Contains("TableName"))
                {
                    grid.Columns["TableName"].HeaderText = "Tên bàn";
                    grid.Columns["TableName"].FillWeight = 100;
                }
                if (grid.Columns.Contains("AreaName"))
                {
                    grid.Columns["AreaName"].HeaderText = "Khu vực / Tầng";
                    grid.Columns["AreaName"].FillWeight = 100;
                }
                if (grid.Columns.Contains("TableStatus"))
                {
                    grid.Columns["TableStatus"].HeaderText = "Trạng thái bàn";
                    grid.Columns["TableStatus"].FillWeight = 140;
                }

                // Render KPI mini cards
                flowCards.Controls.Clear();
                int total = tables.Rows.Count;
                int avail = tables.AsEnumerable().Count(r => Convert.ToString(r["TableStatus"]) == "Available");
                int occupied = tables.AsEnumerable().Count(r => Convert.ToString(r["TableStatus"]) == "Occupied");
                int reserved = tables.AsEnumerable().Count(r => Convert.ToString(r["TableStatus"]) == "Reserved");

                flowCards.Controls.Add(UITheme.CreateCard("TỔNG SỐ BÀN", total.ToString(), "Toàn bộ các khu vực", UITheme.Espresso, "🪑"));
                flowCards.Controls.Add(UITheme.CreateCard("BÀN ĐANG TRỐNG", avail.ToString(), "Sẵn sàng đón khách", UITheme.Success, "🟢"));
                flowCards.Controls.Add(UITheme.CreateCard("ĐANG CÓ KHÁCH", occupied.ToString(), "Đang phục vụ đồ uống", UITheme.Danger, "🔴"));
                flowCards.Controls.Add(UITheme.CreateCard("ĐẶT TRƯỚC", reserved.ToString(), "Khách đã giữ chỗ", UITheme.WarmAmber, "🟡"));
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void Edit(CafeTableDto current)
        {
            using (Form dialog = new Form())
            {
                string formTitle = current == null ? "Thêm bàn mới" : "Chỉnh sửa bàn: " + current.TableName;
                UITheme.StyleDialog(dialog, formTitle, 460, 360);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    current == null ? "Thêm bàn mới" : "Chỉnh sửa bàn",
                    "Thiết lập tên bàn và khu vực phục vụ (Tầng 1, Ban công, Sân vườn...)."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label lblName = new Label { Text = "Tên bàn (vd: Bàn 01, VIP 01) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 75) };
                TextBox table = new TextBox { Text = current == null ? "" : current.TableName, Width = 390, Font = UITheme.FontMedium, Location = new Point(28, 98) };

                Label lblArea = new Label { Text = "Khu vực / Tầng *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 140) };
                ComboBox area = new ComboBox { Width = 390, Font = UITheme.FontMedium, Location = new Point(28, 163) };
                area.Items.AddRange(new object[] { "Tầng 1", "Tầng 2", "Ban công", "Sân vườn", "Phòng Lạnh (VIP)" });
                area.Text = current == null ? "Tầng 1" : current.AreaName;

                Label statusNotice = new Label
                {
                    Text = current == null
                        ? "ℹ️ Bàn mới tạo sẽ tự động có trạng thái 'Trống' (Available)."
                        : "ℹ️ Trạng thái hiện tại: " + (current.TableStatus == "Available" ? "Trống" : current.TableStatus == "Occupied" ? "Đang có khách" : "Đã đặt trước"),
                    AutoSize = true,
                    Font = UITheme.FontSmall,
                    ForeColor = UITheme.TextSecondary,
                    Location = new Point(28, 205)
                };

                body.Controls.AddRange(new Control[] { lblName, table, lblArea, area, statusNotice });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu bàn", Width = 120, Height = 38, Location = new Point(210, 12) };
                UITheme.ApplyPrimaryButton(save);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 38, Location = new Point(340, 12) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => dialog.Close();

                footer.Controls.Add(save);
                footer.Controls.Add(cancel);

                dialog.Controls.Add(body);
                dialog.Controls.Add(footer);

                save.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(table.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập tên bàn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        table.Focus();
                        return;
                    }

                    try
                    {
                        service.SaveTable(new CafeTableDto
                        {
                            TableId = current == null ? 0 : current.TableId,
                            TableName = table.Text.Trim(),
                            AreaName = string.IsNullOrWhiteSpace(area.Text) ? "Tầng 1" : area.Text.Trim(),
                            TableStatus = current == null ? "Available" : current.TableStatus
                        });
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, dialog);
                    }
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                    LoadData();
            }
        }

        private void DeactivateSelected(object sender, EventArgs e)
        {
            CafeTableDto current = Selected();
            if (current == null)
            {
                MessageBox.Show(this, "Vui lòng chọn bàn cần thao tác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn ngừng sử dụng “" + current.TableName + "”?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                service.DeleteTable(current.TableId);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void ToggleReservation(object sender, EventArgs e)
        {
            CafeTableDto current = Selected();
            if (current == null)
            {
                MessageBox.Show(this, "Vui lòng chọn bàn cần đổi trạng thái đặt bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (current.TableStatus != "Available" && current.TableStatus != "Reserved")
            {
                MessageBox.Show(this, "Chỉ có thể đặt trước hoặc hủy đặt đối với bàn đang Trống hoặc Đã đặt trước. Bàn đang có khách không thể thay đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool reserve = current.TableStatus == "Available";
            string action = reserve ? "đặt trước" : "hủy đặt trước cho";

            if (MessageBox.Show(this, "Bạn có muốn " + action + " “" + current.TableName + "” không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                service.SetTableReservation(current.TableId, reserve);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }
    }
}
