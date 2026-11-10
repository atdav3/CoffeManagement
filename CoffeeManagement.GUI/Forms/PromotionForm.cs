using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class PromotionForm : Form
    {
        private readonly CatalogService service = new CatalogService();

        public PromotionForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnAdd.Click += (s, e) => Edit(null);
            btnEdit.Click += (s, e) => Edit(Selected());
            btnDisable.Click += DisableSelected;
            grid.CellDoubleClick += (s, e) => Edit(Selected());

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "DiscountPercent" && e.Value != null)
                {
                    if (decimal.TryParse(e.Value.ToString(), out decimal val))
                    {
                        e.Value = val.ToString("0.##") + " %";
                        e.FormattingApplied = true;
                    }
                }
                else if (grid.Columns[e.ColumnIndex].Name == "StartsAt" && e.Value != null)
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime dt))
                    {
                        e.Value = dt.ToString("dd/MM/yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                }
                else if (grid.Columns[e.ColumnIndex].Name == "EndsAt" && e.Value != null)
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime dt))
                    {
                        e.Value = dt.ToString("dd/MM/yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
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
            UITheme.ApplyDangerButton(btnDisable);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private PromotionDto Selected()
        {
            if (grid.CurrentRow == null) return null;
            DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;

            return new PromotionDto
            {
                PromotionId = Convert.ToInt32(row["PromotionId"]),
                PromotionName = Convert.ToString(row["PromotionName"]),
                DiscountPercent = Convert.ToDecimal(row["DiscountPercent"]),
                StartsAt = Convert.ToDateTime(row["StartsAt"]),
                EndsAt = Convert.ToDateTime(row["EndsAt"])
            };
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = service.GetPromotions(false);
                grid.DataSource = dt;

                if (grid.Columns.Contains("PromotionId")) grid.Columns["PromotionId"].Visible = false;

                if (grid.Columns.Contains("PromotionName"))
                {
                    grid.Columns["PromotionName"].HeaderText = "Tên chương trình";
                    grid.Columns["PromotionName"].FillWeight = 160;
                }
                if (grid.Columns.Contains("DiscountPercent"))
                {
                    grid.Columns["DiscountPercent"].HeaderText = "Mức giảm (%)";
                    grid.Columns["DiscountPercent"].FillWeight = 80;
                }
                if (grid.Columns.Contains("StartsAt"))
                {
                    grid.Columns["StartsAt"].HeaderText = "Thời gian bắt đầu";
                    grid.Columns["StartsAt"].FillWeight = 110;
                }
                if (grid.Columns.Contains("EndsAt"))
                {
                    grid.Columns["EndsAt"].HeaderText = "Thời gian kết thúc";
                    grid.Columns["EndsAt"].FillWeight = 110;
                }
                if (grid.Columns.Contains("IsActive"))
                {
                    grid.Columns["IsActive"].HeaderText = "Hoạt động";
                    grid.Columns["IsActive"].FillWeight = 70;
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void Edit(PromotionDto current)
        {
            using (Form dialog = new Form())
            {
                string formTitle = current == null ? "Tạo chương trình khuyến mãi mới" : "Chỉnh sửa khuyến mãi: " + current.PromotionName;
                UITheme.StyleDialog(dialog, formTitle, 480, 440);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    current == null ? "Thêm khuyến mãi" : "Chỉnh sửa khuyến mãi",
                    "Thiết lập tên ưu đãi, tỷ lệ chiết khấu (%) và thời gian áp dụng."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = "Tên chương trình khuyến mãi *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                TextBox name = new TextBox { Text = current == null ? "" : current.PromotionName, Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 92) };

                Label l2 = new Label { Text = "Mức giảm giá (% từ 1 đến 100) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                NumericUpDown percent = new NumericUpDown
                {
                    Width = 180,
                    Font = UITheme.FontMedium,
                    DecimalPlaces = 1,
                    Minimum = 0.1m,
                    Maximum = 100m,
                    Increment = 1m,
                    Value = current == null ? 10m : current.DiscountPercent,
                    Location = new Point(28, 152)
                };

                Label l3 = new Label { Text = "Thời gian bắt đầu *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 190) };
                DateTimePicker starts = new DateTimePicker
                {
                    Width = 410,
                    Font = UITheme.FontMedium,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm",
                    Value = current == null ? DateTime.Now : current.StartsAt,
                    Location = new Point(28, 212)
                };

                Label l4 = new Label { Text = "Thời gian kết thúc *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 250) };
                DateTimePicker ends = new DateTimePicker
                {
                    Width = 410,
                    Font = UITheme.FontMedium,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm",
                    Value = current == null ? DateTime.Now.AddDays(7) : current.EndsAt,
                    Location = new Point(28, 272)
                };

                body.Controls.AddRange(new Control[] { l1, name, l2, percent, l3, starts, l4, ends });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu khuyến mãi", Width = 150, Height = 38, Location = new Point(200, 12) };
                UITheme.ApplyPrimaryButton(save);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 38, Location = new Point(360, 12) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => dialog.Close();

                footer.Controls.Add(save);
                footer.Controls.Add(cancel);

                dialog.Controls.Add(body);
                dialog.Controls.Add(footer);

                save.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(name.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập tên chương trình khuyến mãi.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        name.Focus();
                        return;
                    }
                    if (ends.Value <= starts.Value)
                    {
                        MessageBox.Show(dialog, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        service.SavePromotion(new PromotionDto
                        {
                            PromotionId = current == null ? 0 : current.PromotionId,
                            PromotionName = name.Text.Trim(),
                            DiscountPercent = percent.Value,
                            StartsAt = starts.Value,
                            EndsAt = ends.Value
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

        private void DisableSelected(object sender, EventArgs e)
        {
            PromotionDto selected = Selected();
            if (selected == null)
            {
                MessageBox.Show(this, "Vui lòng chọn khuyến mãi cần vô hiệu hóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn ngừng áp dụng khuyến mãi “" + selected.PromotionName + "”?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                service.DisablePromotion(selected.PromotionId);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }
    }
}
