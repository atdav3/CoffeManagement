using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class InventoryForm : Form
    {
        private readonly UserSessionDto session;
        private readonly CatalogService service = new CatalogService();
        private bool loading;

        public InventoryForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnAdd.Click += (s, e) => EditIngredient(null);
            btnEdit.Click += (s, e) => EditIngredient(Selected());
            btnUnits.Click += (s, e) => ManageUnits();
            btnReceipt.Click += (s, e) => Post("Receipt");
            btnIssue.Click += (s, e) => Post("Issue");
            btnHistory.Click += (s, e) => ShowHistory();
            btnRefresh.Click += (s, e) => LoadData();
            grid.CellDoubleClick += (s, e) => EditIngredient(Selected());

            Shown += (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
                LoadData();
            };
        }

        public InventoryForm(UserSessionDto session) : this()
        {
            this.session = session;
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplySuccessButton(btnAdd);
            UITheme.ApplyPrimaryButton(btnEdit);
            UITheme.ApplySecondaryButton(btnUnits);
            UITheme.ApplyPrimaryButton(btnReceipt);
            UITheme.ApplyWarningButton(btnIssue);
            UITheme.ApplySecondaryButton(btnHistory);
            UITheme.ApplySecondaryButton(btnRefresh);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private void LoadData()
        {
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (loading) return;
            loading = true;
            grid.Enabled = false;
            loadStatus.Text = "Đang kiểm tra mức tồn kho từ máy chủ…";
            Cursor = Cursors.WaitCursor;

            try
            {
                DataTable inventory = await Task.Run(() => service.GetInventory());
                if (IsDisposed || Disposing) return;

                grid.DataSource = inventory;
                if (grid.Columns.Contains("IngredientId")) grid.Columns["IngredientId"].Visible = false;
                if (grid.Columns.Contains("UnitId")) grid.Columns["UnitId"].Visible = false;

                if (grid.Columns.Contains("IngredientName"))
                {
                    grid.Columns["IngredientName"].HeaderText = "Tên nguyên liệu";
                    grid.Columns["IngredientName"].FillWeight = 160;
                }
                if (grid.Columns.Contains("Unit"))
                {
                    grid.Columns["Unit"].HeaderText = "Đơn vị tính";
                    grid.Columns["Unit"].FillWeight = 80;
                }
                if (grid.Columns.Contains("OnHand"))
                {
                    grid.Columns["OnHand"].HeaderText = "Tồn kho thực tế";
                    grid.Columns["OnHand"].DefaultCellStyle.Format = "0.###";
                    grid.Columns["OnHand"].FillWeight = 100;
                }
                if (grid.Columns.Contains("ReorderLevel"))
                {
                    grid.Columns["ReorderLevel"].HeaderText = "Định mức tối thiểu";
                    grid.Columns["ReorderLevel"].DefaultCellStyle.Format = "0.###";
                    grid.Columns["ReorderLevel"].FillWeight = 100;
                }

                int lowStock = 0;
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.Cells["OnHand"].Value != null && row.Cells["ReorderLevel"].Value != null)
                    {
                        decimal onHand = Convert.ToDecimal(row.Cells["OnHand"].Value);
                        decimal reorder = Convert.ToDecimal(row.Cells["ReorderLevel"].Value);
                        if (onHand <= reorder)
                        {
                            lowStock++;
                            row.DefaultCellStyle.BackColor = Color.FromArgb(254, 238, 238);
                            row.DefaultCellStyle.ForeColor = UITheme.Danger;
                            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 215, 215);
                            row.DefaultCellStyle.SelectionForeColor = UITheme.Danger;
                        }
                    }
                }

                loadStatus.Text = lowStock == 0
                    ? "✓ Toàn bộ " + inventory.Rows.Count.ToString("N0") + " nguyên liệu đạt mức tồn kho an toàn."
                    : "⚠️ Có " + lowStock.ToString("N0") + " nguyên liệu đang ở mức cảnh báo hoặc hết hàng (tô màu đỏ).";
                loadStatus.ForeColor = lowStock == 0 ? UITheme.Success : UITheme.Danger;
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                {
                    loadStatus.Text = "Không thể tải dữ liệu tồn kho.";
                    UiError.Show(ex, this);
                }
            }
            finally
            {
                loading = false;
                if (!IsDisposed && !Disposing)
                {
                    grid.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
        }

        private DataRowView Selected()
        {
            return loading || grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
        }

        private void EditIngredient(DataRowView current)
        {
            DataTable units = service.GetUnits();
            using (Form dialog = new Form())
            {
                string formTitle = current == null ? "Thêm nguyên liệu mới vào kho" : "Chỉnh sửa nguyên liệu";
                UITheme.StyleDialog(dialog, formTitle, 460, 360);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    current == null ? "Thêm nguyên liệu" : "Chỉnh sửa nguyên liệu",
                    "Nhập tên nguyên liệu, đơn vị đo lường và định mức cảnh báo đặt hàng."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = "Tên nguyên liệu *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                TextBox name = new TextBox { Width = 390, Font = UITheme.FontMedium, Location = new Point(28, 92) };

                Label l2 = new Label { Text = "Đơn vị tính *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                ComboBox unit = new ComboBox { Width = 390, Font = UITheme.FontMedium, DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(28, 152) };
                foreach (DataRow row in units.Rows)
                    unit.Items.Add(new UnitChoice(Convert.ToInt32(row["UnitId"]), Convert.ToString(row["UnitName"]) + " (" + Convert.ToString(row["Abbreviation"]) + ")"));
                if (unit.Items.Count > 0) unit.SelectedIndex = 0;

                Label l3 = new Label { Text = "Định mức tối thiểu (Mức cảnh báo đặt hàng) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 190) };
                TextBox reorder = new TextBox { Width = 390, Font = UITheme.FontMedium, Location = new Point(28, 212) };

                int ingredientId = 0;
                if (current != null)
                {
                    ingredientId = Convert.ToInt32(current["IngredientId"]);
                    name.Text = Convert.ToString(current["IngredientName"]);
                    reorder.Text = Convert.ToDecimal(current["ReorderLevel"]).ToString("0.###");
                    for (int i = 0; i < unit.Items.Count; i++)
                        if (((UnitChoice)unit.Items[i]).Id == Convert.ToInt32(current["UnitId"]))
                            unit.SelectedIndex = i;
                }

                body.Controls.AddRange(new Control[] { l1, name, l2, unit, l3, reorder });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu nguyên liệu", Width = 150, Height = 38, Location = new Point(180, 12) };
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
                        MessageBox.Show(dialog, "Vui lòng nhập tên nguyên liệu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        name.Focus();
                        return;
                    }

                    if (!decimal.TryParse(reorder.Text.Trim(), out decimal level) || level < 0)
                    {
                        MessageBox.Show(dialog, "Định mức tối thiểu phải là số hợp lệ.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reorder.Focus();
                        return;
                    }

                    if (!(unit.SelectedItem is UnitChoice selectedUnit))
                    {
                        MessageBox.Show(dialog, "Vui lòng chọn đơn vị tính.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        service.SaveIngredient(new IngredientDto
                        {
                            IngredientId = ingredientId,
                            IngredientName = name.Text.Trim(),
                            UnitId = selectedUnit.Id,
                            ReorderLevel = level
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

        private void ManageUnits()
        {
            using (Form dialog = new Form())
            {
                UITheme.StyleDialog(dialog, "Quản lý Đơn vị Tính", 680, 480);
                dialog.Controls.Add(UITheme.CreateDialogHeader("Đơn vị đo lường nguyên liệu", "Thêm và quản lý các đơn vị tính như kg, gram, lít, hộp, chai..."));

                Panel actions = new Panel { Dock = DockStyle.Top, Height = 58, Padding = new Padding(16, 10, 16, 10), BackColor = Color.White };
                Button add = new Button { Text = "➕ Thêm đơn vị", Width = 140, Height = 36 };
                UITheme.ApplySuccessButton(add);

                Button edit = new Button { Text = "✏️ Sửa", Width = 90, Height = 36, Left = 150 };
                UITheme.ApplyPrimaryButton(edit);

                Button remove = new Button { Text = "🔒 Xóa", Width = 90, Height = 36, Left = 250 };
                UITheme.ApplyDangerButton(remove);

                Button reload = new Button { Text = "🔄 Tải lại", Width = 90, Height = 36, Left = 350 };
                UITheme.ApplySecondaryButton(reload);

                actions.Controls.AddRange(new Control[] { add, edit, remove, reload });

                DataGridView unitsGrid = new DataGridView { Dock = DockStyle.Fill };
                UITheme.ApplyGridStyle(unitsGrid);

                Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = UITheme.Background };
                container.Controls.Add(unitsGrid);

                dialog.Controls.Add(container);
                dialog.Controls.Add(actions);

                Action load = () =>
                {
                    try
                    {
                        unitsGrid.DataSource = service.GetUnits();
                        if (unitsGrid.Columns.Contains("UnitId")) unitsGrid.Columns["UnitId"].Visible = false;
                        if (unitsGrid.Columns.Contains("UnitName")) unitsGrid.Columns["UnitName"].HeaderText = "Tên đơn vị";
                        if (unitsGrid.Columns.Contains("Abbreviation")) unitsGrid.Columns["Abbreviation"].HeaderText = "Ký hiệu viết tắt";
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, dialog);
                    }
                };

                Func<UnitDto> getSelected = () =>
                {
                    DataRowView row = unitsGrid.CurrentRow == null ? null : unitsGrid.CurrentRow.DataBoundItem as DataRowView;
                    return row == null ? null : new UnitDto
                    {
                        UnitId = Convert.ToInt32(row["UnitId"]),
                        UnitName = Convert.ToString(row["UnitName"]),
                        Abbreviation = Convert.ToString(row["Abbreviation"])
                    };
                };

                Action<UnitDto> editUnit = current =>
                {
                    using (Form editor = new Form())
                    {
                        UITheme.StyleDialog(editor, current == null ? "Thêm đơn vị tính" : "Chỉnh sửa đơn vị", 420, 290);
                        editor.Controls.Add(UITheme.CreateDialogHeader(current == null ? "Thêm đơn vị" : "Chỉnh sửa đơn vị"));

                        Panel b = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(24, 16, 24, 16) };
                        Label l1 = new Label { Text = "Tên đơn vị (vd: Kilogram, Lít) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(24, 70) };
                        TextBox unitName = new TextBox { Text = current == null ? "" : current.UnitName, Width = 350, Font = UITheme.FontMedium, Location = new Point(24, 92) };

                        Label l2 = new Label { Text = "Ký hiệu viết tắt (vd: kg, L, ml) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(24, 130) };
                        TextBox abbr = new TextBox { Text = current == null ? "" : current.Abbreviation, Width = 350, Font = UITheme.FontMedium, Location = new Point(24, 152) };

                        b.Controls.AddRange(new Control[] { l1, unitName, l2, abbr });

                        Panel f = UITheme.CreateDialogFooter(58);
                        Button s = new Button { Text = "💾 Lưu", Width = 100, Height = 36, Location = new Point(180, 10) };
                        UITheme.ApplyPrimaryButton(s);
                        Button c = new Button { Text = "Hủy", Width = 80, Height = 36, Location = new Point(290, 10) };
                        UITheme.ApplySecondaryButton(c);
                        c.Click += (sender, e) => editor.Close();

                        f.Controls.Add(s);
                        f.Controls.Add(c);
                        editor.Controls.Add(b);
                        editor.Controls.Add(f);

                        s.Click += (sender, e) =>
                        {
                            if (string.IsNullOrWhiteSpace(unitName.Text) || string.IsNullOrWhiteSpace(abbr.Text))
                            {
                                MessageBox.Show(editor, "Vui lòng nhập đầy đủ tên đơn vị và ký hiệu viết tắt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            try
                            {
                                service.SaveUnit(new UnitDto
                                {
                                    UnitId = current == null ? 0 : current.UnitId,
                                    UnitName = unitName.Text.Trim(),
                                    Abbreviation = abbr.Text.Trim()
                                });
                                editor.DialogResult = DialogResult.OK;
                                editor.Close();
                            }
                            catch (Exception ex)
                            {
                                UiError.Show(ex, editor);
                            }
                        };

                        if (editor.ShowDialog(dialog) == DialogResult.OK)
                            load();
                    }
                };

                add.Click += (s, e) => editUnit(null);
                edit.Click += (s, e) => { UnitDto u = getSelected(); if (u != null) editUnit(u); };
                reload.Click += (s, e) => load();
                remove.Click += (s, e) =>
                {
                    UnitDto u = getSelected();
                    if (u == null) return;
                    if (MessageBox.Show(dialog, "Bạn có chắc muốn xóa đơn vị “" + u.UnitName + "”? Đơn vị đã gán cho nguyên liệu sẽ không thể xóa.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;
                    try { service.DeleteUnit(u.UnitId); load(); }
                    catch (Exception ex) { UiError.Show(ex, dialog); }
                };

                dialog.Shown += (s, e) => load();
                dialog.ShowDialog(this);
            }
        }

        private void Post(string type)
        {
            DataRowView row = Selected();
            if (row == null)
            {
                MessageBox.Show(this, "Vui lòng chọn một nguyên liệu trên bảng trước khi nhập/xuất kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isReceipt = type == "Receipt";
            using (Form dialog = new Form())
            {
                string actionTitle = isReceipt ? "Tạo phiếu Nhập kho (+)" : "Tạo phiếu Xuất kho (-)";
                UITheme.StyleDialog(dialog, actionTitle, 480, 420);

                string ingName = Convert.ToString(row["IngredientName"]);
                string unitName = Convert.ToString(row["Unit"]);
                decimal onHand = Convert.ToDecimal(row["OnHand"]);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    actionTitle,
                    "Nguyên liệu: " + ingName + " · Tồn kho hiện tại: " + onHand.ToString("0.###") + " " + unitName));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = (isReceipt ? "Số lượng nhập thêm (" : "Số lượng xuất kho (") + unitName + ") *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                TextBox quantity = new TextBox { Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 92) };

                Label l2 = new Label { Text = "Đơn giá nhập mỗi đơn vị (₫, tùy chọn)", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                TextBox cost = new TextBox { Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 152) };
                if (!isReceipt) { l2.Visible = false; cost.Visible = false; }

                Label l3 = new Label { Text = "Ghi chú / Lý do (Nhà cung cấp, hủy hỏng...)", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, isReceipt ? 190 : 130) };
                TextBox note = new TextBox { Width = 410, Font = UITheme.FontRegular, Location = new Point(28, isReceipt ? 212 : 152) };

                body.Controls.AddRange(new Control[] { l1, quantity, l2, cost, l3, note });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = isReceipt ? "📥 Xác nhận nhập kho" : "📤 Xác nhận xuất kho", Width = 180, Height = 38, Location = new Point(170, 12) };
                if (isReceipt) UITheme.ApplySuccessButton(save); else UITheme.ApplyWarningButton(save);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 38, Location = new Point(360, 12) };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Click += (s, e) => dialog.Close();

                footer.Controls.Add(save);
                footer.Controls.Add(cancel);

                dialog.Controls.Add(body);
                dialog.Controls.Add(footer);

                save.Click += (s, e) =>
                {
                    if (!decimal.TryParse(quantity.Text.Trim(), out decimal qty) || qty <= 0)
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập số lượng hợp lệ lớn hơn 0.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        quantity.Focus();
                        return;
                    }

                    if (!isReceipt && qty > onHand)
                    {
                        if (MessageBox.Show(dialog, "Số lượng xuất kho (" + qty.ToString("0.###") + ") lớn hơn tồn kho hiện tại (" + onHand.ToString("0.###") + "). Bạn vẫn muốn tiếp tục xuất kho?", "Cảnh báo âm kho", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            return;
                    }

                    decimal? parsedCost = null;
                    if (!string.IsNullOrWhiteSpace(cost.Text))
                    {
                        if (decimal.TryParse(cost.Text.Trim().Replace(",", ""), out decimal cVal) && cVal >= 0)
                            parsedCost = cVal;
                        else
                        {
                            MessageBox.Show(dialog, "Đơn giá nhập không hợp lệ.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cost.Focus();
                            return;
                        }
                    }

                    int userId = session != null ? session.UserId : 0;
                    try
                    {
                        service.PostInventory(Convert.ToInt32(row["IngredientId"]), userId, type, qty, parsedCost, note.Text.Trim());
                        MessageBox.Show(dialog, (isReceipt ? "Đã nhập kho" : "Đã xuất kho") + " thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ShowHistory()
        {
            using (Form dialog = new Form())
            {
                UITheme.StyleDialog(dialog, "Lịch sử Giao dịch Nhập / Xuất Kho", 950, 560);
                dialog.Controls.Add(UITheme.CreateDialogHeader("Nhật ký nhập xuất nguyên liệu", "Tra cứu toàn bộ lịch sử biến động kho theo khoảng thời gian."));

                Panel filter = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = Color.White, Padding = new Padding(20, 14, 20, 14) };
                DateTime businessToday = new BusinessCalendarService().Today;

                Label l1 = new Label { Text = "Từ ngày:", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(20, 22) };
                DateTimePicker start = new DateTimePicker { Left = 85, Top = 16, Width = 140, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Value = businessToday.AddDays(-30), Font = UITheme.FontMedium };

                Label l2 = new Label { Text = "Đến ngày:", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(240, 22) };
                DateTimePicker end = new DateTimePicker { Left = 315, Top = 16, Width = 140, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Value = businessToday, Font = UITheme.FontMedium };

                Button search = new Button { Text = "🔍 Tra cứu", Left = 475, Top = 14, Width = 120, Height = 36 };
                UITheme.ApplyPrimaryButton(search);

                filter.Controls.AddRange(new Control[] { l1, start, l2, end, search });

                DataGridView historyGrid = new DataGridView { Dock = DockStyle.Fill };
                UITheme.ApplyGridStyle(historyGrid);

                Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), BackColor = UITheme.Background };
                container.Controls.Add(historyGrid);

                dialog.Controls.Add(container);
                dialog.Controls.Add(filter);

                Action load = () =>
                {
                    try
                    {
                        historyGrid.DataSource = service.GetInventoryHistory(start.Value, end.Value);
                        if (historyGrid.Columns.Contains("InventoryTransactionId")) historyGrid.Columns["InventoryTransactionId"].Visible = false;

                        if (historyGrid.Columns.Contains("TransactionTime"))
                        {
                            historyGrid.Columns["TransactionTime"].HeaderText = "Thời gian";
                            historyGrid.Columns["TransactionTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                            historyGrid.Columns["TransactionTime"].FillWeight = 110;
                        }
                        if (historyGrid.Columns.Contains("IngredientName"))
                        {
                            historyGrid.Columns["IngredientName"].HeaderText = "Tên nguyên liệu";
                            historyGrid.Columns["IngredientName"].FillWeight = 140;
                        }
                        if (historyGrid.Columns.Contains("TransactionType"))
                        {
                            historyGrid.Columns["TransactionType"].HeaderText = "Loại giao dịch";
                            historyGrid.Columns["TransactionType"].FillWeight = 90;
                        }
                        if (historyGrid.Columns.Contains("Quantity"))
                        {
                            historyGrid.Columns["Quantity"].HeaderText = "Số lượng";
                            historyGrid.Columns["Quantity"].DefaultCellStyle.Format = "0.###";
                            historyGrid.Columns["Quantity"].FillWeight = 80;
                        }
                        if (historyGrid.Columns.Contains("UnitCost"))
                        {
                            historyGrid.Columns["UnitCost"].HeaderText = "Đơn giá nhập (₫)";
                            historyGrid.Columns["UnitCost"].DefaultCellStyle.Format = "N0";
                            historyGrid.Columns["UnitCost"].FillWeight = 100;
                        }
                        if (historyGrid.Columns.Contains("Note"))
                        {
                            historyGrid.Columns["Note"].HeaderText = "Ghi chú";
                            historyGrid.Columns["Note"].FillWeight = 160;
                        }
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, dialog);
                    }
                };

                search.Click += (s, e) => load();
                dialog.Shown += (s, e) => load();
                dialog.ShowDialog(this);
            }
        }

        private sealed class UnitChoice
        {
            public int Id { get; private set; }
            private readonly string label;
            public UnitChoice(int id, string label) { Id = id; this.label = label; }
            public override string ToString() { return label; }
        }
    }
}
