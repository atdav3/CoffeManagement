using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class CatalogForm : Form
    {
        private readonly CatalogService service = new CatalogService();

        public CatalogForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnFilter.Click += (s, e) => LoadProducts();
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadProducts(); };
            cboCategory.SelectedIndexChanged += (s, e) => LoadProducts();

            btnAdd.Click += (s, e) => EditProduct(null);
            btnEdit.Click += (s, e) => EditProduct(SelectedProduct());
            btnDelete.Click += DeleteSelected;
            grid.CellDoubleClick += (s, e) => EditProduct(SelectedProduct());

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "AvailabilityStatus" && e.Value is string status)
                {
                    e.CellStyle.ForeColor = status.StartsWith("🟢", StringComparison.Ordinal)
                        ? UITheme.Success
                        : UITheme.Danger;
                }
            };

            Shown += (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
                LoadCategories();
                LoadProducts();
            };
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplySecondaryButton(btnFilter);
            UITheme.ApplySuccessButton(btnAdd);
            UITheme.ApplyPrimaryButton(btnEdit);
            UITheme.ApplyDangerButton(btnDelete);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private void LoadCategories()
        {
            DataTable data = service.GetCategories();
            cboCategory.Items.Clear();
            cboCategory.Items.Add(new CategoryChoice(0, "Tất cả danh mục"));
            foreach (DataRow row in data.Rows)
                cboCategory.Items.Add(new CategoryChoice(Convert.ToInt32(row["CategoryId"]), Convert.ToString(row["CategoryName"])));
            cboCategory.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            try
            {
                CategoryChoice option = cboCategory.SelectedItem as CategoryChoice;
                DataTable dt = service.GetProducts(txtSearch.Text.Trim(), option == null || option.Id == 0 ? (int?)null : option.Id);
                AddBooleanTextColumn(dt, "IsAvailable", "AvailabilityStatus", "🟢 Đang bán", "🔴 Ngừng bán");
                grid.DataSource = dt;

                foreach (string col in new[] { "CategoryId", "IsActive", "IsAvailable", "ImagePath", "Description" })
                    if (grid.Columns.Contains(col)) grid.Columns[col].Visible = false;

                if (grid.Columns.Contains("ProductId"))
                {
                    grid.Columns["ProductId"].HeaderText = "Mã món";
                    grid.Columns["ProductId"].FillWeight = 50;
                }
                if (grid.Columns.Contains("ProductName"))
                {
                    grid.Columns["ProductName"].HeaderText = "Tên món / Sản phẩm";
                    grid.Columns["ProductName"].FillWeight = 150;
                }
                if (grid.Columns.Contains("CategoryName"))
                {
                    grid.Columns["CategoryName"].HeaderText = "Danh mục";
                    grid.Columns["CategoryName"].FillWeight = 100;
                }
                if (grid.Columns.Contains("UnitPrice"))
                {
                    grid.Columns["UnitPrice"].HeaderText = "Đơn giá (₫)";
                    grid.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                    grid.Columns["UnitPrice"].FillWeight = 80;
                }
                if (grid.Columns.Contains("AvailabilityStatus"))
                {
                    grid.Columns["AvailabilityStatus"].HeaderText = "Trạng thái";
                    grid.Columns["AvailabilityStatus"].FillWeight = 80;
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private static void AddBooleanTextColumn(DataTable data, string sourceColumn, string displayColumn, string trueText, string falseText)
        {
            if (!data.Columns.Contains(sourceColumn)) return;
            if (!data.Columns.Contains(displayColumn)) data.Columns.Add(displayColumn, typeof(string));

            foreach (DataRow row in data.Rows)
            {
                row[displayColumn] = row.IsNull(sourceColumn)
                    ? (object)DBNull.Value
                    : Convert.ToBoolean(row[sourceColumn]) ? trueText : falseText;
            }
        }

        private ProductDto SelectedProduct()
        {
            if (grid.CurrentRow == null) return null;
            DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;

            return new ProductDto
            {
                ProductId = Convert.ToInt32(row["ProductId"]),
                CategoryId = Convert.ToInt32(row["CategoryId"]),
                ProductName = Convert.ToString(row["ProductName"]),
                Description = Convert.ToString(row["Description"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                ImagePath = row["ImagePath"] == DBNull.Value ? null : Convert.ToString(row["ImagePath"]),
                IsAvailable = Convert.ToBoolean(row["IsAvailable"])
            };
        }

        private void EditProduct(ProductDto product)
        {
            DataTable categories = service.GetCategories();
            using (ProductDialog dialog = new ProductDialog(categories, product))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        service.SaveProduct(dialog.Product);
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, this);
                    }
                }
            }
        }

        private void DeleteSelected(object sender, EventArgs e)
        {
            ProductDto product = SelectedProduct();
            if (product == null)
            {
                MessageBox.Show(this, "Vui lòng chọn món cần thao tác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string actionName = product.IsAvailable ? "ngừng bán" : "mở bán lại";
            if (MessageBox.Show(this, "Bạn có chắc chắn muốn " + actionName + " món “" + product.ProductName + "”?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                service.DeleteProduct(product.ProductId);
                LoadProducts();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private sealed class CategoryChoice
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public CategoryChoice(int id, string name) { Id = id; Name = name; }
            public override string ToString() { return Name; }
        }

        private sealed class ProductDialog : Form
        {
            private readonly TextBox name = new TextBox();
            private readonly TextBox price = new TextBox();
            private readonly TextBox description = new TextBox();
            private readonly TextBox imagePath = new TextBox();
            private readonly ComboBox category = new ComboBox();
            private readonly CheckBox available = new CheckBox();
            private readonly int existingId;

            public ProductDto Product { get; private set; }

            public ProductDialog(DataTable categories, ProductDto current)
            {
                existingId = current == null ? 0 : current.ProductId;
                string formTitle = current == null ? "Thêm món mới vào thực đơn" : "Chỉnh sửa thông tin món: " + current.ProductName;
                UITheme.StyleDialog(this, formTitle, 520, 530);

                Controls.Add(UITheme.CreateDialogHeader(
                    current == null ? "Thêm món mới" : "Chỉnh sửa món",
                    "Thiết lập tên món, phân loại danh mục và đơn giá niêm yết."));

                Panel body = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = UITheme.Background,
                    Padding = new Padding(32, 16, 32, 16)
                };

                // Name
                Label lblName = new Label { Text = "Tên món / Đồ uống *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(32, 70) };
                name.SetBounds(32, 92, 440, 32);
                name.Font = UITheme.FontMedium;

                // Category
                Label lblCat = new Label { Text = "Danh mục sản phẩm *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(32, 132) };
                category.SetBounds(32, 154, 440, 32);
                category.DropDownStyle = ComboBoxStyle.DropDownList;
                category.Font = UITheme.FontMedium;
                foreach (DataRow row in categories.Rows)
                    category.Items.Add(new CategoryChoice(Convert.ToInt32(row["CategoryId"]), Convert.ToString(row["CategoryName"])));

                // Price
                Label lblPrice = new Label { Text = "Đơn giá niêm yết (₫) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(32, 194) };
                price.SetBounds(32, 216, 440, 32);
                price.Font = UITheme.FontMedium;

                // Description
                Label lblDesc = new Label { Text = "Mô tả ngắn về món", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(32, 256) };
                description.SetBounds(32, 278, 440, 52);
                description.Multiline = true;
                description.Font = UITheme.FontRegular;

                // Image Path
                Label lblImg = new Label { Text = "Hình ảnh sản phẩm (tùy chọn)", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(32, 338) };
                imagePath.SetBounds(32, 360, 345, 32);
                imagePath.Font = UITheme.FontRegular;
                Button browse = new Button { Text = "Chọn ảnh...", Left = 385, Top = 359, Width = 87, Height = 32 };
                UITheme.ApplySecondaryButton(browse);
                browse.Click += (s, e) => BrowseImage();

                // Available checkbox
                available.Text = "Đang kinh doanh (Hiển thị trên màn hình bán hàng POS)";
                available.Font = UITheme.FontSemibold;
                available.ForeColor = UITheme.Espresso;
                available.AutoSize = true;
                available.Location = new Point(32, 404);

                body.Controls.AddRange(new Control[]
                {
                    lblName, name,
                    lblCat, category,
                    lblPrice, price,
                    lblDesc, description,
                    lblImg, imagePath, browse,
                    available
                });

                // Footer
                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu thông tin", Width = 140, Height = 38 };
                UITheme.ApplyPrimaryButton(save);
                save.Location = new Point(220, 12);
                save.Click += SaveClicked;

                Button cancel = new Button { Text = "Hủy bỏ", Width = 100, Height = 38 };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Location = new Point(370, 12);
                cancel.Click += (s, e) => Close();

                footer.Controls.Add(save);
                footer.Controls.Add(cancel);

                Controls.Add(body);
                Controls.Add(footer);

                // Populate initial data
                if (current != null)
                {
                    name.Text = current.ProductName;
                    price.Text = current.UnitPrice.ToString("0.##");
                    description.Text = current.Description;
                    imagePath.Text = current.ImagePath ?? "";
                    available.Checked = current.IsAvailable;
                    for (int i = 0; i < category.Items.Count; i++)
                        if (((CategoryChoice)category.Items[i]).Id == current.CategoryId)
                            category.SelectedIndex = i;
                }
                else
                {
                    available.Checked = true;
                    if (category.Items.Count > 0) category.SelectedIndex = 0;
                }
            }

            private void BrowseImage()
            {
                using (OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "Chọn hình ảnh món",
                    Filter = "Tệp hình ảnh (*.png;*.jpg;*.jpeg;*.webp)|*.png;*.jpg;*.jpeg;*.webp|Tất cả tệp (*.*)|*.*",
                    CheckFileExists = true
                })
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                        imagePath.Text = dialog.FileName;
                }
            }

            private void SaveClicked(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(name.Text))
                {
                    MessageBox.Show(this, "Vui lòng nhập tên món.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    name.Focus();
                    return;
                }

                if (!(category.SelectedItem is CategoryChoice selected))
                {
                    MessageBox.Show(this, "Vui lòng chọn danh mục cho món.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    category.Focus();
                    return;
                }

                if (!decimal.TryParse(price.Text.Trim().Replace(",", ""), out decimal amount) || amount < 0)
                {
                    MessageBox.Show(this, "Vui lòng nhập đơn giá hợp lệ.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    price.Focus();
                    return;
                }

                if (!string.IsNullOrWhiteSpace(imagePath.Text) && !File.Exists(imagePath.Text))
                {
                    MessageBox.Show(this, "Đường dẫn hình ảnh không tồn tại. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Product = new ProductDto
                {
                    ProductId = existingId,
                    CategoryId = selected.Id,
                    ProductName = name.Text.Trim(),
                    Description = description.Text.Trim(),
                    UnitPrice = amount,
                    ImagePath = string.IsNullOrWhiteSpace(imagePath.Text) ? null : imagePath.Text.Trim(),
                    IsAvailable = available.Checked
                };

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
