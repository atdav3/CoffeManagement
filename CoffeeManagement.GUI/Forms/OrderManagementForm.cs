using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

using System.ComponentModel;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class OrderManagementForm : Form
    {
        private readonly UserSessionDto session;
        private readonly OrderService service = new OrderService();
        private readonly CatalogService catalog = new CatalogService();

        private bool loading;

        public OrderManagementForm()
        {
            InitializeComponent();
            UITheme.ApplyGridStyle(grid);
            AttachEvents();
        }

        public OrderManagementForm(UserSessionDto session) : this()
        {
            this.session = session;
            if (session != null && session.RoleName != "Admin" && session.RoleName != "Manager")
                cancel.Visible = false;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && !DesignMode)
            {
                DateTime businessToday = new BusinessCalendarService().Today;
                start.Value = businessToday.AddDays(-30);
                end.Value = businessToday;

                Shown += async (s, e) =>
                {
                    await LoadEmployeeFilterAsync();
                    await LoadDataAsync();
                };
            }
        }

        private void AttachEvents()
        {
            filterPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, filterPanel.Height - 1, filterPanel.Width, filterPanel.Height - 1);
                }
            };

            searchButton.Click += (s, e) => LoadData();
            details.Click += (s, e) => ShowDetails();
            pay.Click += PaySelected;
            cancel.Click += CancelSelected;
            transfer.Click += TransferSelected;
            merge.Click += MergeSelected;
            split.Click += SplitSelected;
            grid.CellDoubleClick += (s, e) => ShowDetails();

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "OrderStatus" && e.Value != null)
                {
                    string st = Convert.ToString(e.Value);
                    if (st == "Paid")
                    {
                        e.Value = "🟢 Đã thanh toán";
                        e.CellStyle.ForeColor = UITheme.Success;
                    }
                    else if (st == "Open")
                    {
                        e.Value = "🔵 Đang phục vụ";
                        e.CellStyle.ForeColor = UITheme.Info;
                    }
                    else if (st == "Cancelled")
                    {
                        e.Value = "🔴 Đã hủy";
                        e.CellStyle.ForeColor = UITheme.Danger;
                    }
                    e.FormattingApplied = true;
                }
                else if (grid.Columns[e.ColumnIndex].Name == "CreatedAt" && e.Value != null && e.Value != DBNull.Value)
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime dt))
                    {
                        e.Value = dt.ToString("dd/MM/yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                }
            };
        }

        private DataRowView Selected()
        {
            return loading || grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
        }

        private long SelectedId()
        {
            DataRowView row = Selected();
            return row == null ? 0 : Convert.ToInt64(row["OrderId"]);
        }

        private void LoadData() { _ = LoadDataAsync(); }

        private async Task LoadDataAsync()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            if (loading) return;
            loading = true;
            searchButton.Enabled = false;
            grid.Enabled = false;
            start.Enabled = end.Enabled = status.Enabled = employee.Enabled = false;
            searchButton.Text = "Đang tải…";
            loadStatus.Text = "Đang lấy danh sách hóa đơn từ SQL Server…";
            Cursor = Cursors.WaitCursor;

            try
            {
                string selStatus = null;
                if (status.SelectedIndex == 1) selStatus = "Open";
                else if (status.SelectedIndex == 2) selStatus = "Paid";
                else if (status.SelectedIndex == 3) selStatus = "Cancelled";

                EmployeeChoice selectedEmployee = employee.SelectedItem as EmployeeChoice;
                DateTime from = start.Value, to = end.Value;
                int? employeeId = selectedEmployee == null || selectedEmployee.Id == 0 ? (int?)null : selectedEmployee.Id;

                DataTable result = await Task.Run(() => service.GetAll(from, to, selStatus, employeeId));
                if (IsDisposed || Disposing) return;

                grid.DataSource = result;
                if (grid.Columns.Contains("OrderId")) grid.Columns["OrderId"].Visible = false;
                if (grid.Columns.Contains("TableId")) grid.Columns["TableId"].Visible = false;

                if (grid.Columns.Contains("OrderCode"))
                {
                    grid.Columns["OrderCode"].HeaderText = "Mã hóa đơn";
                    grid.Columns["OrderCode"].FillWeight = 110;
                }
                if (grid.Columns.Contains("TableName"))
                {
                    grid.Columns["TableName"].HeaderText = "Bàn phục vụ";
                    grid.Columns["TableName"].FillWeight = 110;
                }
                if (grid.Columns.Contains("EmployeeName"))
                {
                    grid.Columns["EmployeeName"].HeaderText = "Nhân viên tạo";
                    grid.Columns["EmployeeName"].FillWeight = 120;
                }
                if (grid.Columns.Contains("CreatedAt"))
                {
                    grid.Columns["CreatedAt"].HeaderText = "Thời gian tạo";
                    grid.Columns["CreatedAt"].FillWeight = 120;
                }
                foreach (string name in new[] { "Subtotal", "DiscountAmount", "TotalAmount", "PaidAmount" })
                {
                    if (grid.Columns.Contains(name))
                    {
                        grid.Columns[name].DefaultCellStyle.Format = "N0";
                        if (name == "Subtotal") grid.Columns[name].HeaderText = "Tạm tính (₫)";
                        else if (name == "DiscountAmount") grid.Columns[name].HeaderText = "Giảm giá (₫)";
                        else if (name == "TotalAmount") grid.Columns[name].HeaderText = "Tổng tiền (₫)";
                        else if (name == "PaidAmount") grid.Columns[name].HeaderText = "Đã trả (₫)";
                        grid.Columns[name].FillWeight = 90;
                    }
                }
                if (grid.Columns.Contains("OrderStatus"))
                {
                    grid.Columns["OrderStatus"].HeaderText = "Trạng thái";
                    grid.Columns["OrderStatus"].FillWeight = 110;
                }

                loadStatus.Text = "✓ Đã tải " + result.Rows.Count.ToString("N0") + " hóa đơn bán hàng.";
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing) UiError.Show(ex, this);
            }
            finally
            {
                loading = false;
                if (!IsDisposed && !Disposing)
                {
                    searchButton.Enabled = true;
                    grid.Enabled = true;
                    start.Enabled = end.Enabled = status.Enabled = employee.Enabled = true;
                    searchButton.Text = "🔍 Tìm kiếm";
                    Cursor = Cursors.Default;
                }
            }
        }

        private async Task LoadEmployeeFilterAsync()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            employee.Enabled = false;
            try
            {
                DataTable data = await Task.Run(() => catalog.GetEmployees(null));
                if (IsDisposed || Disposing) return;
                employee.Items.Clear();
                employee.Items.Add(new EmployeeChoice(0, "Tất cả nhân viên"));
                foreach (DataRow row in data.Rows)
                    employee.Items.Add(new EmployeeChoice(Convert.ToInt32(row["EmployeeId"]), Convert.ToString(row["FullName"])));
                employee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing) UiError.Show(ex, this);
            }
            finally
            {
                if (!IsDisposed && !Disposing) employee.Enabled = true;
            }
        }

        private void ShowDetails()
        {
            DataRowView order = Selected();
            if (order == null)
            {
                MessageBox.Show(this, "Vui lòng chọn một hóa đơn để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DataTable details = service.GetDetails(SelectedId());
                using (Form dialog = new Form())
                {
                    string orderCode = Convert.ToString(order["OrderCode"]);
                    UITheme.StyleDialog(dialog, "Chi tiết hóa đơn #" + orderCode, 780, 520);

                    dialog.Controls.Add(UITheme.CreateDialogHeader(
                        "Hóa đơn bán hàng: #" + orderCode,
                        "Bàn: " + Convert.ToString(order["TableName"]) + " · Nhân viên: " + Convert.ToString(order["EmployeeName"]) + " · Trạng thái: " + Convert.ToString(order["OrderStatus"])));

                    DataGridView lines = new DataGridView { Dock = DockStyle.Fill, DataSource = details };
                    UITheme.ApplyGridStyle(lines);

                    if (lines.Columns.Contains("OrderDetailId")) lines.Columns["OrderDetailId"].Visible = false;
                    if (lines.Columns.Contains("ProductId")) lines.Columns["ProductId"].Visible = false;

                    if (lines.Columns.Contains("ProductName"))
                    {
                        lines.Columns["ProductName"].HeaderText = "Tên món";
                        lines.Columns["ProductName"].FillWeight = 160;
                    }
                    if (lines.Columns.Contains("UnitPrice"))
                    {
                        lines.Columns["UnitPrice"].HeaderText = "Đơn giá (₫)";
                        lines.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                        lines.Columns["UnitPrice"].FillWeight = 90;
                    }
                    if (lines.Columns.Contains("Quantity"))
                    {
                        lines.Columns["Quantity"].HeaderText = "Số lượng";
                        lines.Columns["Quantity"].DefaultCellStyle.Format = "0.###";
                        lines.Columns["Quantity"].FillWeight = 60;
                    }
                    if (lines.Columns.Contains("LineTotal"))
                    {
                        lines.Columns["LineTotal"].HeaderText = "Thành tiền (₫)";
                        lines.Columns["LineTotal"].DefaultCellStyle.Format = "N0";
                        lines.Columns["LineTotal"].FillWeight = 100;
                    }

                    Panel billFooter = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = UITheme.EspressoLight, Padding = new Padding(20, 10, 20, 10) };
                    Label summary = new Label
                    {
                        Text = "Tạm tính: " + Convert.ToDecimal(order["Subtotal"]).ToString("N0") + " ₫    |    Giảm giá: " + Convert.ToDecimal(order["DiscountAmount"]).ToString("N0") + " ₫    |    TỔNG CỘNG: " + Convert.ToDecimal(order["TotalAmount"]).ToString("N0") + " ₫",
                        Dock = DockStyle.Fill,
                        Font = UITheme.FontSubheader,
                        ForeColor = UITheme.Espresso,
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    billFooter.Controls.Add(summary);

                    Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = UITheme.Background };
                    container.Controls.Add(lines);

                    dialog.Controls.Add(container);
                    dialog.Controls.Add(billFooter);
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void PaySelected(object sender, EventArgs e)
        {
            DataRowView row = Selected();
            if (row == null)
            {
                MessageBox.Show(this, "Vui lòng chọn hóa đơn cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Convert.ToString(row["OrderStatus"]) != "Open")
            {
                MessageBox.Show(this, "Chỉ có thể thu tiền đối với đơn hàng đang ở trạng thái 'Đang phục vụ' (Open).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                long id = SelectedId();
                decimal balance = service.GetBalance(id);
                if (balance <= 0)
                {
                    MessageBox.Show(this, "Hóa đơn này không còn số dư cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SettleForm payment = new SettleForm(balance))
                {
                    if (payment.ShowDialog(this) != DialogResult.OK) return;

                    decimal remaining = service.CollectPayment(id, payment.Method, payment.Amount, session.UserId, payment.Reference, payment.TenderedAmount);
                    decimal change = payment.TenderedAmount.HasValue ? payment.TenderedAmount.Value - payment.Amount : 0;
                    string result = remaining == 0
                        ? "Thanh toán hóa đơn hoàn tất thành công!" + (payment.Method == "Cash" ? "\n\nTiền thối lại cho khách: " + change.ToString("N0") + " ₫" : "")
                        : "Đã ghi nhận thanh toán. Số tiền còn lại chưa thanh toán: " + remaining.ToString("N0") + " ₫";

                    MessageBox.Show(this, result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void CancelSelected(object sender, EventArgs e)
        {
            DataRowView row = Selected();
            if (row == null || Convert.ToString(row["OrderStatus"]) != "Open")
            {
                MessageBox.Show(this, "Chỉ có thể hủy đối với đơn hàng đang mở.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn hủy hóa đơn “" + Convert.ToString(row["OrderCode"]) + "”? Bàn phục vụ sẽ được giải phóng trở về trạng thái Trống.", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                service.Cancel(SelectedId(), session);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void TransferSelected(object sender, EventArgs e)
        {
            DataRowView row = Selected();
            if (row == null || Convert.ToString(row["OrderStatus"]) != "Open")
            {
                MessageBox.Show(this, "Vui lòng chọn một đơn hàng đang phục vụ để chuyển bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (TableChoiceForm dialog = new TableChoiceForm(catalog.GetTables(), "Chuyển đơn hàng sang bàn khác"))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                string dest = dialog.TableId.HasValue ? "bàn đã chọn" : "Khách mang về (Take-away)";
                if (MessageBox.Show(this, "Chuyển đơn hàng này sang " + dest + "?", "Xác nhận chuyển bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                try
                {
                    service.TransferTable(SelectedId(), dialog.TableId, session.UserId);
                    LoadData();
                }
                catch (Exception ex)
                {
                    UiError.Show(ex, this);
                }
            }
        }

        private void MergeSelected(object sender, EventArgs e)
        {
            DataRowView source = Selected();
            if (source == null || Convert.ToString(source["OrderStatus"]) != "Open")
            {
                MessageBox.Show(this, "Vui lòng chọn đơn hàng nguồn đang phục vụ để thực hiện gộp bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable openOrders = service.GetAll(start.Value, end.Value, "Open", null);
            using (Form dialog = new Form())
            {
                UITheme.StyleDialog(dialog, "Gộp đơn hàng / Gộp bàn", 480, 240);
                dialog.Controls.Add(UITheme.CreateDialogHeader("Gộp vào đơn hàng đích", "Chọn đơn hàng bạn muốn gộp toàn bộ món của đơn hiện tại vào."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };
                ComboBox target = new ComboBox { Width = 410, Font = UITheme.FontMedium, DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(28, 80) };

                foreach (DataRow item in openOrders.Rows)
                {
                    if (Convert.ToInt64(item["OrderId"]) != SelectedId())
                        target.Items.Add(new OrderChoice(Convert.ToInt64(item["OrderId"]), Convert.ToString(item["OrderCode"]) + " · Bàn: " + Convert.ToString(item["TableName"]) + " (" + Convert.ToDecimal(item["TotalAmount"]).ToString("N0") + " ₫)"));
                }
                if (target.Items.Count > 0) target.SelectedIndex = 0;
                else
                {
                    MessageBox.Show(this, "Không có đơn hàng nào khác đang mở để thực hiện gộp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                body.Controls.Add(target);

                Panel footer = UITheme.CreateDialogFooter(58);
                Button confirm = new Button { Text = "🔗 Xác nhận gộp đơn", Width = 160, Height = 36, Location = new Point(190, 10) };
                UITheme.ApplyPrimaryButton(confirm);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 36, Location = new Point(360, 10) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, args) => dialog.Close();

                footer.Controls.Add(confirm);
                footer.Controls.Add(cancel);

                dialog.Controls.Add(body);
                dialog.Controls.Add(footer);

                confirm.Click += (s, args) =>
                {
                    if (target.SelectedItem == null) return;
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                };

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                if (MessageBox.Show(this, "Bạn có chắc muốn gộp đơn này vào đơn được chọn? Đơn nguồn sẽ được đóng và chuyển toàn bộ món sang đơn đích.", "Xác nhận gộp bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                try
                {
                    service.Merge(SelectedId(), ((OrderChoice)target.SelectedItem).Id, session.UserId);
                    LoadData();
                }
                catch (Exception ex)
                {
                    UiError.Show(ex, this);
                }
            }
        }

        private void SplitSelected(object sender, EventArgs e)
        {
            DataRowView source = Selected();
            if (source == null || Convert.ToString(source["OrderStatus"]) != "Open")
            {
                MessageBox.Show(this, "Vui lòng chọn một đơn hàng đang mở để tách bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable details = service.GetDetails(SelectedId());
                using (SplitForm dialog = new SplitForm(details, catalog.GetTables()))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;

                    decimal quantity = dialog.Lines.Sum(line => line.Quantity);
                    if (MessageBox.Show(this, "Tách " + quantity.ToString("0.###") + " món sang hóa đơn mới?", "Xác nhận tách bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    long newOrder = service.Split(SelectedId(), dialog.TableId, session.UserId, dialog.Lines);
                    MessageBox.Show(this, "Đã tạo hóa đơn mới sau khi tách: #" + newOrder, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private sealed class OrderChoice
        {
            public long Id { get; private set; }
            private readonly string text;
            public OrderChoice(long id, string text) { Id = id; this.text = text; }
            public override string ToString() { return text; }
        }

        private sealed class EmployeeChoice
        {
            public int Id { get; private set; }
            private readonly string text;
            public EmployeeChoice(int id, string text) { Id = id; this.text = text; }
            public override string ToString() { return text; }
        }

        private sealed class TableChoiceForm : Form
        {
            private readonly ComboBox table = new ComboBox();
            public int? TableId
            {
                get
                {
                    TableItem value = table.SelectedItem as TableItem;
                    return value == null ? null : value.Id;
                }
            }

            public TableChoiceForm(DataTable tables, string title)
            {
                UITheme.StyleDialog(this, title, 460, 220);
                Controls.Add(UITheme.CreateDialogHeader(title, "Chọn bàn trống đích cần chuyển đến:"));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };
                table.SetBounds(28, 75, 390, 32);
                table.DropDownStyle = ComboBoxStyle.DropDownList;
                table.Font = UITheme.FontMedium;
                table.Items.Add(new TableItem(null, "🥡 Khách mang về (Take-away)"));
                foreach (DataRow row in tables.Rows)
                    if (Convert.ToString(row["TableStatus"]) == "Available")
                        table.Items.Add(new TableItem(Convert.ToInt32(row["TableId"]), "🪑 " + Convert.ToString(row["TableName"]) + " · " + Convert.ToString(row["AreaName"])));
                table.SelectedIndex = 0;
                body.Controls.Add(table);

                Panel footer = UITheme.CreateDialogFooter(58);
                Button select = new Button { Text = "🔄 Xác nhận chuyển", Width = 160, Height = 36, Location = new Point(180, 10) };
                UITheme.ApplyPrimaryButton(select);
                select.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 36, Location = new Point(350, 10) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => Close();

                footer.Controls.Add(select);
                footer.Controls.Add(cancel);

                Controls.Add(body);
                Controls.Add(footer);
            }
        }

        private sealed class TableItem
        {
            public int? Id { get; private set; }
            private readonly string label;
            public TableItem(int? id, string label) { Id = id; this.label = label; }
            public override string ToString() { return label; }
        }

        private sealed class SettleForm : Form
        {
            private readonly ComboBox method = new ComboBox();
            private readonly NumericUpDown cash = new NumericUpDown(), amount = new NumericUpDown();
            private readonly TextBox reference = new TextBox();
            private readonly Label changeLabel = new Label();

            public string Method { get { return method.SelectedIndex == 0 ? "Cash" : "Transfer"; } }
            public string Reference { get { return Method == "Transfer" ? reference.Text : null; } }
            public decimal Amount { get { return amount.Value; } }
            public decimal? TenderedAmount { get { return Method == "Cash" ? (decimal?)cash.Value : null; } }

            public SettleForm(decimal due)
            {
                UITheme.StyleDialog(this, "Thu tiền Hóa đơn", 460, 440);
                Controls.Add(UITheme.CreateDialogHeader("Thanh toán hóa đơn", "Số tiền còn lại cần thanh toán: " + due.ToString("N0") + " ₫"));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = "Số tiền thanh toán lần này (₫) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                amount.SetBounds(28, 92, 390, 32);
                amount.Font = UITheme.FontMedium;
                amount.DecimalPlaces = 0;
                amount.ThousandsSeparator = true;
                amount.Minimum = 1;
                amount.Maximum = due;
                amount.Value = due;

                Label l2 = new Label { Text = "Phương thức thanh toán *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                method.SetBounds(28, 152, 390, 32);
                method.DropDownStyle = ComboBoxStyle.DropDownList;
                method.Font = UITheme.FontMedium;
                method.Items.AddRange(new object[] { "Tiền mặt (Cash)", "Chuyển khoản (Transfer / VietQR)" });
                method.SelectedIndex = 0;

                Label l3 = new Label { Text = "Tiền khách đưa (₫) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 190) };
                cash.SetBounds(28, 212, 390, 32);
                cash.Font = UITheme.FontMedium;
                cash.DecimalPlaces = 0;
                cash.ThousandsSeparator = true;
                cash.Maximum = 999999999999m;
                cash.Value = due;

                reference.SetBounds(28, 212, 390, 32);
                reference.Font = UITheme.FontMedium;
                reference.Visible = false;

                changeLabel.Text = "Tiền thối lại cho khách: 0 ₫";
                changeLabel.Font = UITheme.FontSemibold10;
                changeLabel.ForeColor = UITheme.Success;
                changeLabel.AutoSize = true;
                changeLabel.Location = new Point(28, 255);

                body.Controls.AddRange(new Control[] { l1, amount, l2, method, l3, cash, reference, changeLabel });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button confirm = new Button { Text = "💳 Hoàn tất thu tiền", Width = 170, Height = 38, Location = new Point(170, 12) };
                UITheme.ApplySuccessButton(confirm);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 38, Location = new Point(350, 12) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => Close();

                footer.Controls.Add(confirm);
                footer.Controls.Add(cancel);

                Controls.Add(body);
                Controls.Add(footer);

                method.SelectedIndexChanged += (s, e) =>
                {
                    bool isTransfer = method.SelectedIndex == 1;
                    cash.Visible = !isTransfer;
                    reference.Visible = isTransfer;
                    changeLabel.Visible = !isTransfer;
                    l3.Text = isTransfer ? "Mã giao dịch / Ghi chú chuyển khoản *" : "Tiền khách đưa (₫) *";
                };

                cash.ValueChanged += (s, e) =>
                {
                    decimal change = cash.Value - amount.Value;
                    changeLabel.Text = change >= 0
                        ? "Tiền thối lại cho khách: " + change.ToString("N0") + " ₫"
                        : "⚠️ Khách đưa chưa đủ tiền (" + Math.Abs(change).ToString("N0") + " ₫)";
                    changeLabel.ForeColor = change >= 0 ? UITheme.Success : UITheme.Danger;
                };

                confirm.Click += (s, e) =>
                {
                    if (method.SelectedIndex == 0 && cash.Value < amount.Value)
                    {
                        MessageBox.Show(this, "Số tiền khách đưa phải lớn hơn hoặc bằng số tiền cần thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (method.SelectedIndex == 1 && string.IsNullOrWhiteSpace(reference.Text))
                    {
                        MessageBox.Show(this, "Vui lòng nhập mã giao dịch hoặc thông tin chuyển khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reference.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                };
            }
        }

        private sealed class SplitForm : Form
        {
            private readonly DataGridView lines = new DataGridView();
            private readonly ComboBox table = new ComboBox();
            public List<OrderSplitLineDto> Lines { get; private set; }
            public int? TableId
            {
                get
                {
                    TableItem option = table.SelectedItem as TableItem;
                    return option == null ? null : option.Id;
                }
            }

            public SplitForm(DataTable details, DataTable tables)
            {
                UITheme.StyleDialog(this, "Tách món sang hóa đơn mới", 750, 500);
                Controls.Add(UITheme.CreateDialogHeader("Tách món sang hóa đơn mới", "Nhập số lượng cần tách trên từng dòng món."));

                if (!details.Columns.Contains("SplitQuantity"))
                    details.Columns.Add("SplitQuantity", typeof(decimal));

                lines.DataSource = details;
                lines.Dock = DockStyle.Fill;
                lines.ReadOnly = false;
                lines.AllowUserToAddRows = false;
                lines.SelectionMode = DataGridViewSelectionMode.CellSelect;
                lines.MultiSelect = false;
                lines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                UITheme.ApplyGridStyle(lines);

                foreach (DataGridViewColumn col in lines.Columns)
                    col.ReadOnly = col.Name != "SplitQuantity";

                if (lines.Columns.Contains("OrderDetailId")) lines.Columns["OrderDetailId"].Visible = false;
                if (lines.Columns.Contains("ProductId")) lines.Columns["ProductId"].Visible = false;

                if (lines.Columns.Contains("ProductName"))
                {
                    lines.Columns["ProductName"].HeaderText = "Tên món";
                    lines.Columns["ProductName"].FillWeight = 160;
                }
                if (lines.Columns.Contains("Quantity"))
                {
                    lines.Columns["Quantity"].HeaderText = "SL hiện có";
                    lines.Columns["Quantity"].DefaultCellStyle.Format = "0.###";
                    lines.Columns["Quantity"].FillWeight = 80;
                }
                if (lines.Columns.Contains("SplitQuantity"))
                {
                    lines.Columns["SplitQuantity"].HeaderText = "SL muốn tách sang";
                    lines.Columns["SplitQuantity"].DefaultCellStyle.Format = "0.###";
                    lines.Columns["SplitQuantity"].FillWeight = 100;
                    lines.Columns["SplitQuantity"].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                }

                Panel bottom = new Panel { Dock = DockStyle.Bottom, Height = 95, Padding = new Padding(20, 10, 20, 10), BackColor = Color.White };
                Label lblTable = new Label { Text = "Bàn đích mới:", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(20, 18) };
                table.SetBounds(120, 14, 250, 32);
                table.DropDownStyle = ComboBoxStyle.DropDownList;
                table.Font = UITheme.FontMedium;
                table.Items.Add(new TableItem(null, "🥡 Khách mang về (Take-away)"));
                foreach (DataRow row in tables.Rows)
                    if (Convert.ToString(row["TableStatus"]) == "Available")
                        table.Items.Add(new TableItem(Convert.ToInt32(row["TableId"]), "🪑 " + Convert.ToString(row["TableName"])));
                table.SelectedIndex = 0;

                Button splitBtn = new Button { Text = "✂️ Xác nhận tách món", Width = 180, Height = 36, Location = new Point(530, 12) };
                UITheme.ApplyPrimaryButton(splitBtn);

                Label note = new Label
                {
                    Text = "Lưu ý: Hóa đơn gốc phải giữ lại ít nhất 1 món. Hóa đơn đã thanh toán không thể tách.",
                    Font = UITheme.FontSmall,
                    ForeColor = UITheme.TextSecondary,
                    AutoSize = true,
                    Location = new Point(20, 58)
                };

                bottom.Controls.AddRange(new Control[] { lblTable, table, splitBtn, note });

                Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = UITheme.Background };
                container.Controls.Add(lines);

                Controls.Add(container);
                Controls.Add(bottom);

                splitBtn.Click += (s, e) =>
                {
                    lines.EndEdit();
                    List<OrderSplitLineDto> requested = new List<OrderSplitLineDto>();
                    foreach (DataRow row in details.Rows)
                    {
                        decimal requestedQty = row["SplitQuantity"] == DBNull.Value ? 0m : Convert.ToDecimal(row["SplitQuantity"]);
                        decimal availableQty = Convert.ToDecimal(row["Quantity"]);
                        if (requestedQty < 0m || requestedQty > availableQty)
                        {
                            MessageBox.Show(this, "Số lượng muốn tách phải từ 0 đến số lượng hiện có.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (requestedQty > 0m)
                            requested.Add(new OrderSplitLineDto { OrderDetailId = Convert.ToInt64(row["OrderDetailId"]), Quantity = requestedQty });
                    }

                    if (requested.Count == 0)
                    {
                        MessageBox.Show(this, "Vui lòng nhập số lượng muốn tách cho ít nhất một món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (!details.AsEnumerable().Any(row => Convert.ToDecimal(row["Quantity"]) - (row["SplitQuantity"] == DBNull.Value ? 0m : Convert.ToDecimal(row["SplitQuantity"])) > 0m))
                    {
                        MessageBox.Show(this, "Đơn hàng gốc phải giữ lại ít nhất một món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Lines = requested;
                    DialogResult = DialogResult.OK;
                    Close();
                };
            }
        }
    }
}
