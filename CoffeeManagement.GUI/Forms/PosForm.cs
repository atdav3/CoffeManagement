using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

using System.ComponentModel;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class PosForm : Form
    {
        private readonly UserSessionDto session;
        private readonly CatalogService catalog = new CatalogService();
        private readonly OrderService orders = new OrderService();
        
        private readonly DataTable cart = new DataTable("Cart");
        private readonly Dictionary<int, ProductDto> selectedProducts = new Dictionary<int, ProductDto>();

        public PosForm()
        {
            InitializeComponent();
            BuildCart();
            UITheme.ApplyGridStyle(products);
            UITheme.ApplyGridStyle(cartGrid);
            AttachEvents();
        }

        public PosForm(UserSessionDto session) : this()
        {
            this.session = session;
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && !DesignMode)
            {
                Shown += (s, e) => LoadData();
            }
        }

        private void BuildCart()
        {
            cart.Columns.Add("ProductId", typeof(int));
            cart.Columns.Add("Product", typeof(string));
            cart.Columns.Add("UnitPrice", typeof(decimal));
            cart.Columns.Add("Quantity", typeof(decimal));
            cart.Columns.Add("LineTotal", typeof(decimal));
            cart.PrimaryKey = new[] { cart.Columns["ProductId"] };
        }

        private void AttachEvents()
        {
            top.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, top.Height - 1, top.Width, top.Height - 1);
                }
            };

            footer.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, footer.Width - 1, footer.Height - 1);
                }
            };

            find.Click += (s, e) => LoadProducts();
            search.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadProducts(); };
            promotion.SelectedIndexChanged += (s, e) => UpdateTotal();

            addBtn.Click += (s, e) => AddSelectedProduct();
            products.CellDoubleClick += (s, e) => AddSelectedProduct();

            plusBtn.Click += (s, e) => AdjustQuantity(1m);
            minusBtn.Click += (s, e) => AdjustQuantity(-1m);
            remove.Click += (s, e) => RemoveSelectedProduct();
            clearCart.Click += (s, e) =>
            {
                if (cart.Rows.Count > 0 && MessageBox.Show(this, "Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    ResetCart();
            };

            pay.Click += PayClicked;
        }

        private void LoadData()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            try
            {
                DataTable tables = catalog.GetTables();
                table.Items.Clear();
                table.Items.Add(new TableChoice(null, "🥡 Khách mang về (Take-away)"));
                foreach (DataRow row in tables.Rows)
                    if (Convert.ToString(row["TableStatus"]) == "Available")
                        table.Items.Add(new TableChoice(Convert.ToInt32(row["TableId"]), "🪑 " + Convert.ToString(row["TableName"]) + " · " + Convert.ToString(row["AreaName"])));
                table.SelectedIndex = 0;

                DataTable promotions = catalog.GetPromotions(true);
                promotion.Items.Clear();
                promotion.Items.Add(new PromotionChoice(null, "Không áp dụng khuyến mãi", 0));
                foreach (DataRow row in promotions.Rows)
                    promotion.Items.Add(new PromotionChoice(Convert.ToInt32(row["PromotionId"]), Convert.ToString(row["PromotionName"]), Convert.ToDecimal(row["DiscountPercent"])));
                promotion.SelectedIndex = 0;

                LoadProducts();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void LoadProducts()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable data = catalog.GetProducts(search.Text.Trim(), null);
                DataView view = new DataView(data) { RowFilter = "IsAvailable = true" };
                products.DataSource = view;

                foreach (string col in new[] { "ProductId", "CategoryId", "IsActive", "IsAvailable", "ImagePath", "Description" })
                    if (products.Columns.Contains(col)) products.Columns[col].Visible = false;

                if (products.Columns.Contains("ProductName"))
                {
                    products.Columns["ProductName"].HeaderText = "Tên món";
                    products.Columns["ProductName"].FillWeight = 160;
                }
                if (products.Columns.Contains("CategoryName"))
                {
                    products.Columns["CategoryName"].HeaderText = "Danh mục";
                    products.Columns["CategoryName"].FillWeight = 110;
                }
                if (products.Columns.Contains("UnitPrice"))
                {
                    products.Columns["UnitPrice"].HeaderText = "Đơn giá (₫)";
                    products.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                    products.Columns["UnitPrice"].FillWeight = 90;
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private ProductDto SelectedProduct()
        {
            DataRowView row = products.CurrentRow == null ? null : products.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;
            return new ProductDto
            {
                ProductId = Convert.ToInt32(row["ProductId"]),
                ProductName = Convert.ToString(row["ProductName"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"])
            };
        }

        private void AddSelectedProduct()
        {
            ProductDto item = SelectedProduct();
            if (item == null) return;

            DataRow row = cart.Rows.Find(item.ProductId);
            if (row == null)
                cart.Rows.Add(item.ProductId, item.ProductName, item.UnitPrice, 1m, item.UnitPrice);
            else
            {
                row["Quantity"] = Convert.ToDecimal(row["Quantity"]) + 1m;
                row["LineTotal"] = Convert.ToDecimal(row["Quantity"]) * Convert.ToDecimal(row["UnitPrice"]);
            }

            selectedProducts[item.ProductId] = item;
            cartGrid.DataSource = cart;
            UpdateTotal();
        }

        private void AdjustQuantity(decimal delta)
        {
            if (cartGrid.CurrentRow == null) return;
            int id = Convert.ToInt32(cartGrid.CurrentRow.Cells["ProductId"].Value);
            DataRow row = cart.Rows.Find(id);
            if (row != null)
            {
                decimal currentQty = Convert.ToDecimal(row["Quantity"]);
                decimal newQty = currentQty + delta;
                if (newQty <= 0)
                {
                    cart.Rows.Remove(row);
                    selectedProducts.Remove(id);
                }
                else
                {
                    row["Quantity"] = newQty;
                    row["LineTotal"] = newQty * Convert.ToDecimal(row["UnitPrice"]);
                }
                UpdateTotal();
            }
        }

        private void RemoveSelectedProduct()
        {
            if (cartGrid.CurrentRow == null) return;
            int id = Convert.ToInt32(cartGrid.CurrentRow.Cells["ProductId"].Value);
            DataRow row = cart.Rows.Find(id);
            if (row != null) cart.Rows.Remove(row);
            selectedProducts.Remove(id);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal amount = cart.Rows.Cast<DataRow>().Sum(row => Convert.ToDecimal(row["LineTotal"]));
            PromotionChoice offer = promotion.SelectedItem as PromotionChoice;
            decimal discount = offer == null ? 0 : Math.Round(amount * offer.Percent / 100m, 2);
            decimal finalTotal = amount - discount;

            subtotalLabel.Text = "Tạm tính:  " + amount.ToString("N0") + " ₫";
            discountLabel.Text = "Chiết khấu:  " + (discount > 0 ? "-" + discount.ToString("N0") + " ₫ (" + offer.Percent.ToString("0.##") + "%)" : "0 ₫");
            totalLabel.Text = "TỔNG:  " + finalTotal.ToString("N0") + " ₫";

            if (cartGrid.Columns.Contains("ProductId")) cartGrid.Columns["ProductId"].Visible = false;
            if (cartGrid.Columns.Contains("Product"))
            {
                cartGrid.Columns["Product"].HeaderText = "Tên món";
                cartGrid.Columns["Product"].FillWeight = 160;
            }
            if (cartGrid.Columns.Contains("UnitPrice"))
            {
                cartGrid.Columns["UnitPrice"].HeaderText = "Đơn giá (₫)";
                cartGrid.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                cartGrid.Columns["UnitPrice"].FillWeight = 85;
            }
            if (cartGrid.Columns.Contains("Quantity"))
            {
                cartGrid.Columns["Quantity"].HeaderText = "SL";
                cartGrid.Columns["Quantity"].DefaultCellStyle.Format = "0.###";
                cartGrid.Columns["Quantity"].FillWeight = 50;
            }
            if (cartGrid.Columns.Contains("LineTotal"))
            {
                cartGrid.Columns["LineTotal"].HeaderText = "Thành tiền (₫)";
                cartGrid.Columns["LineTotal"].DefaultCellStyle.Format = "N0";
                cartGrid.Columns["LineTotal"].FillWeight = 95;
            }
        }

        private void PayClicked(object sender, EventArgs e)
        {
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show(this, "Vui lòng chọn ít nhất một món vào giỏ hàng trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!session.EmployeeId.HasValue)
            {
                MessageBox.Show(this, "Tài khoản hiện tại chưa được liên kết với hồ sơ nhân viên để lập hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TableChoice selectedTable = table.SelectedItem as TableChoice;
                PromotionChoice selectedPromotion = promotion.SelectedItem as PromotionChoice;
                OrderDto order = new OrderDto
                {
                    EmployeeId = session.EmployeeId.Value,
                    TableId = selectedTable == null ? null : selectedTable.Id,
                    PromotionId = selectedPromotion == null ? null : selectedPromotion.Id
                };

                foreach (DataRow row in cart.Rows)
                    order.Lines.Add(new OrderLineDto { ProductId = Convert.ToInt32(row["ProductId"]), Quantity = Convert.ToDecimal(row["Quantity"]) });

                long id = orders.Save(order);
                ResetCart();
                decimal due = orders.GetTotal(id);

                using (PaymentDialog payment = new PaymentDialog(due))
                {
                    if (payment.ShowDialog(this) != DialogResult.OK)
                    {
                        MessageBox.Show(this, "Hóa đơn #" + id + " đã được tạo và lưu ở trạng thái chờ thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCart();
                        return;
                    }

                    decimal balance = orders.CollectPayment(id, payment.Method, payment.Amount, session.UserId, payment.Reference, payment.TenderedAmount);
                    if (balance == 0)
                    {
                        decimal change = payment.TenderedAmount.HasValue ? payment.TenderedAmount.Value - payment.Amount : 0;
                        string msg = "Thanh toán hóa đơn #" + id + " thành công!" + (payment.Method == "Cash" ? "\n\nTiền khách đưa: " + payment.TenderedAmount.Value.ToString("N0") + " ₫\nTiền thối lại: " + change.ToString("N0") + " ₫" : "");
                        MessageBox.Show(this, msg, "Thanh toán hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "Đã ghi nhận thanh toán. Số tiền còn thiếu: " + balance.ToString("N0") + " ₫");
                    }
                    ResetCart();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void ResetCart()
        {
            cart.Clear();
            cartGrid.DataSource = cart;
            UpdateTotal();
        }

        private sealed class TableChoice
        {
            public int? Id { get; private set; }
            public string Name { get; private set; }
            public TableChoice(int? id, string name) { Id = id; Name = name; }
            public override string ToString() { return Name; }
        }

        private sealed class PromotionChoice
        {
            public int? Id { get; private set; }
            public decimal Percent { get; private set; }
            private readonly string name;
            public PromotionChoice(int? id, string name, decimal percent)
            {
                Id = id;
                this.name = percent > 0 ? name + " · Giảm " + percent.ToString("0.##") + "%" : name;
                Percent = percent;
            }
            public override string ToString() { return name; }
        }

        private sealed class PaymentDialog : Form
        {
            private readonly TextBox received = new TextBox();
            private readonly ComboBox method = new ComboBox();
            private readonly TextBox reference = new TextBox();
            private readonly NumericUpDown amount = new NumericUpDown();
            private readonly Label changeLabel = new Label();
            private readonly decimal totalDue;

            public string Method { get { return method.SelectedIndex == 0 ? "Cash" : "Transfer"; } }
            public string Reference { get { return reference.Text; } }
            public decimal Amount { get { return amount.Value; } }
            public decimal? TenderedAmount { get; private set; }

            public PaymentDialog(decimal due)
            {
                totalDue = due;
                UITheme.StyleDialog(this, "Thanh toán Hóa đơn", 460, 480);
                Controls.Add(UITheme.CreateDialogHeader("Xác nhận thanh toán", "Chọn phương thức thanh toán và nhập số tiền khách trả."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label dueBox = new Label
                {
                    Text = "TỔNG TIỀN PHẢI TRẢ:  " + due.ToString("N0") + " ₫",
                    Font = UITheme.FontSubheader,
                    ForeColor = UITheme.Espresso,
                    AutoSize = true,
                    Location = new Point(28, 70)
                };

                Label l1 = new Label { Text = "Số tiền thanh toán lần này (₫) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 105) };
                amount.SetBounds(28, 127, 390, 32);
                amount.Font = UITheme.FontMedium;
                amount.DecimalPlaces = 0;
                amount.ThousandsSeparator = true;
                amount.Minimum = 1;
                amount.Maximum = due;
                amount.Value = due;

                Label l2 = new Label { Text = "Phương thức thanh toán *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 165) };
                method.SetBounds(28, 187, 390, 32);
                method.DropDownStyle = ComboBoxStyle.DropDownList;
                method.Font = UITheme.FontMedium;
                method.Items.AddRange(new object[] { "Tiền mặt (Cash)", "Chuyển khoản / Quét mã VietQR (Transfer)" });
                method.SelectedIndex = 0;

                Label l3 = new Label { Text = "Tiền khách đưa / Mã chuẩn chi ngân hàng *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 225) };
                received.SetBounds(28, 247, 390, 32);
                received.Font = UITheme.FontMedium;
                received.Text = due.ToString("0.##");

                changeLabel.Text = "Tiền thối lại cho khách:  0 ₫";
                changeLabel.Font = UITheme.FontSemibold10;
                changeLabel.ForeColor = UITheme.Success;
                changeLabel.AutoSize = true;
                changeLabel.Location = new Point(28, 290);

                // Quick cash buttons
                FlowLayoutPanel quickCash = new FlowLayoutPanel
                {
                    Location = new Point(28, 318),
                    Size = new Size(390, 36),
                    WrapContents = false
                };

                Button exactBtn = new Button { Text = "Đủ tiền", Width = 85, Height = 32 };
                UITheme.ApplySecondaryButton(exactBtn);
                exactBtn.Click += (s, e) => { received.Text = amount.Value.ToString("0.##"); RecalculateChange(); };

                Button cash100 = new Button { Text = "+100k", Width = 65, Height = 32 };
                UITheme.ApplySecondaryButton(cash100);
                cash100.Click += (s, e) => AddCash(100000m);

                Button cash200 = new Button { Text = "+200k", Width = 65, Height = 32 };
                UITheme.ApplySecondaryButton(cash200);
                cash200.Click += (s, e) => AddCash(200000m);

                Button cash500 = new Button { Text = "+500k", Width = 65, Height = 32 };
                UITheme.ApplySecondaryButton(cash500);
                cash500.Click += (s, e) => AddCash(500000m);

                quickCash.Controls.AddRange(new Control[] { exactBtn, cash100, cash200, cash500 });

                body.Controls.AddRange(new Control[] { dueBox, l1, amount, l2, method, l3, received, changeLabel, quickCash });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button confirm = new Button { Text = "💳 Hoàn tất thanh toán", Width = 180, Height = 38, Location = new Point(160, 12) };
                UITheme.ApplySuccessButton(confirm);
                confirm.Click += ConfirmClicked;

                Button cancel = new Button { Text = "Hủy bỏ", Width = 80, Height = 38, Location = new Point(350, 12) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => Close();

                footer.Controls.Add(confirm);
                footer.Controls.Add(cancel);

                Controls.Add(body);
                Controls.Add(footer);

                method.SelectedIndexChanged += (s, e) =>
                {
                    quickCash.Visible = method.SelectedIndex == 0;
                    changeLabel.Visible = method.SelectedIndex == 0;
                    if (method.SelectedIndex == 0)
                        received.Text = amount.Value.ToString("0.##");
                    else
                        received.Text = "QR-" + DateTime.Now.ToString("HHmmss");
                    RecalculateChange();
                };

                received.TextChanged += (s, e) => RecalculateChange();
                amount.ValueChanged += (s, e) => RecalculateChange();
                RecalculateChange();
            }

            private void AddCash(decimal extra)
            {
                decimal.TryParse(received.Text.Trim().Replace(",", ""), out decimal cur);
                received.Text = (cur + extra).ToString("0.##");
                RecalculateChange();
            }

            private void RecalculateChange()
            {
                if (method.SelectedIndex == 0 && decimal.TryParse(received.Text.Trim().Replace(",", ""), out decimal cash))
                {
                    decimal change = cash - amount.Value;
                    if (change >= 0)
                    {
                        changeLabel.Text = "Tiền thối lại cho khách:  " + change.ToString("N0") + " ₫";
                        changeLabel.ForeColor = UITheme.Success;
                    }
                    else
                    {
                        changeLabel.Text = "⚠️ Tiền khách đưa còn thiếu:  " + Math.Abs(change).ToString("N0") + " ₫";
                        changeLabel.ForeColor = UITheme.Danger;
                    }
                }
            }

            private void ConfirmClicked(object sender, EventArgs e)
            {
                if (method.SelectedIndex == 0)
                {
                    if (!decimal.TryParse(received.Text.Trim().Replace(",", ""), out decimal cash) || cash < amount.Value)
                    {
                        MessageBox.Show(this, "Số tiền khách đưa phải lớn hơn hoặc bằng số tiền cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        received.Focus();
                        return;
                    }
                    TenderedAmount = cash;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(received.Text))
                    {
                        MessageBox.Show(this, "Vui lòng nhập mã giao dịch chuyển khoản hoặc ghi chú ngân hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        received.Focus();
                        return;
                    }
                }

                if (MessageBox.Show(this, "Xác nhận thu số tiền " + amount.Value.ToString("N0") + " ₫?", "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (Method == "Cash") reference.Text = null;
                else reference.Text = received.Text.Trim();

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
