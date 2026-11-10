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
    internal partial class CategoryForm : Form
    {
        private readonly CatalogService service = new CatalogService();

        public CategoryForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnAdd.Click += (s, e) => Edit(null);
            btnEdit.Click += (s, e) => Edit(Selected());
            btnDeactivate.Click += DeactivateSelected;
            grid.CellDoubleClick += (s, e) => Edit(Selected());

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
            UITheme.ApplyDangerButton(btnDeactivate);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private CategoryDto Selected()
        {
            if (grid.CurrentRow == null) return null;
            DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;

            return new CategoryDto
            {
                CategoryId = Convert.ToInt32(row["CategoryId"]),
                CategoryName = Convert.ToString(row["CategoryName"]),
                Description = row["Description"] == DBNull.Value ? null : Convert.ToString(row["Description"])
            };
        }

        private void LoadData()
        {
            try
            {
                grid.DataSource = service.GetCategories();
                if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"].Visible = false;

                if (grid.Columns.Contains("CategoryId"))
                {
                    grid.Columns["CategoryId"].HeaderText = "Mã danh mục";
                    grid.Columns["CategoryId"].FillWeight = 40;
                }
                if (grid.Columns.Contains("CategoryName"))
                {
                    grid.Columns["CategoryName"].HeaderText = "Tên danh mục";
                    grid.Columns["CategoryName"].FillWeight = 120;
                }
                if (grid.Columns.Contains("Description"))
                {
                    grid.Columns["Description"].HeaderText = "Mô tả phân loại";
                    grid.Columns["Description"].FillWeight = 200;
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void Edit(CategoryDto category)
        {
            using (Form dialog = new Form())
            {
                string formTitle = category == null ? "Thêm danh mục món mới" : "Chỉnh sửa danh mục";
                UITheme.StyleDialog(dialog, formTitle, 460, 320);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    category == null ? "Thêm danh mục" : "Chỉnh sửa danh mục",
                    "Nhập tên nhóm thực đơn và mô tả ngắn gọn."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label lblName = new Label { Text = "Tên danh mục *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 75) };
                TextBox name = new TextBox { Text = category == null ? "" : category.CategoryName, Width = 390, Font = UITheme.FontMedium, Location = new Point(28, 98) };

                Label lblDesc = new Label { Text = "Mô tả danh mục", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 140) };
                TextBox description = new TextBox { Text = category == null ? "" : category.Description, Width = 390, Font = UITheme.FontRegular, Location = new Point(28, 163) };

                body.Controls.AddRange(new Control[] { lblName, name, lblDesc, description });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu danh mục", Width = 140, Height = 38, Location = new Point(190, 12) };
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
                    if (string.IsNullOrWhiteSpace(name.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập tên danh mục.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        name.Focus();
                        return;
                    }

                    try
                    {
                        service.SaveCategory(new CategoryDto
                        {
                            CategoryId = category == null ? 0 : category.CategoryId,
                            CategoryName = name.Text.Trim(),
                            Description = description.Text.Trim()
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
            CategoryDto category = Selected();
            if (category == null)
            {
                MessageBox.Show(this, "Vui lòng chọn danh mục cần xóa / khóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn ngừng kích hoạt danh mục “" + category.CategoryName + "”?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                service.DeleteCategory(category.CategoryId);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }
    }
}
