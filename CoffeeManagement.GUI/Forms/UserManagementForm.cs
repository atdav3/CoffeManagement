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
    internal partial class UserManagementForm : Form
    {
        private readonly UserSessionDto actor;
        private readonly AuthenticationService auth = new AuthenticationService();
        private readonly CatalogService catalog = new CatalogService();

        public UserManagementForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnAdd.Click += (s, e) => CreateAccount();
            btnToggle.Click += ToggleSelected;

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "AccountStatus" && e.Value is string status)
                {
                    e.CellStyle.ForeColor = status.StartsWith("🟢", StringComparison.Ordinal)
                        ? UITheme.Success
                        : UITheme.Danger;
                }
                else if (grid.Columns[e.ColumnIndex].Name == "LastLoginAt" && e.Value != null && e.Value != DBNull.Value)
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
                LoadAccounts();
            };
        }

        public UserManagementForm(UserSessionDto actor) : this()
        {
            this.actor = actor;
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplySuccessButton(btnAdd);
            UITheme.ApplyPrimaryButton(btnToggle);

            topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
                }
            };
        }

        private DataRowView Selected()
        {
            return grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as DataRowView;
        }

        private void LoadAccounts()
        {
            try
            {
                DataTable accounts = auth.GetAccounts();
                AddBooleanTextColumn(accounts, "IsActive", "AccountStatus", "🟢 Hoạt động", "🔴 Bị khóa");
                grid.DataSource = accounts;

                if (grid.Columns.Contains("UserId")) grid.Columns["UserId"].Visible = false;
                if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"].Visible = false;

                if (grid.Columns.Contains("UserName"))
                {
                    grid.Columns["UserName"].HeaderText = "Tên đăng nhập";
                    grid.Columns["UserName"].FillWeight = 110;
                }
                if (grid.Columns.Contains("FullName"))
                {
                    grid.Columns["FullName"].HeaderText = "Nhân viên liên kết";
                    grid.Columns["FullName"].FillWeight = 140;
                }
                if (grid.Columns.Contains("RoleName"))
                {
                    grid.Columns["RoleName"].HeaderText = "Vai trò / Phân quyền";
                    grid.Columns["RoleName"].FillWeight = 100;
                }
                if (grid.Columns.Contains("LastLoginAt"))
                {
                    grid.Columns["LastLoginAt"].HeaderText = "Lần đăng nhập gần nhất";
                    grid.Columns["LastLoginAt"].FillWeight = 120;
                }
                if (grid.Columns.Contains("AccountStatus"))
                {
                    grid.Columns["AccountStatus"].HeaderText = "Trạng thái";
                    grid.Columns["AccountStatus"].FillWeight = 90;
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

        private void CreateAccount()
        {
            DataTable employees = catalog.GetEmployees(null);
            DataTable roles = auth.GetRoles();

            using (Form dialog = new Form())
            {
                UITheme.StyleDialog(dialog, "Tạo tài khoản người dùng mới", 480, 480);
                dialog.Controls.Add(UITheme.CreateDialogHeader("Tạo tài khoản", "Gán thông tin tài khoản đăng nhập cho nhân viên chưa có tài khoản."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = "Tên đăng nhập *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                TextBox username = new TextBox { Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 92) };

                Label l2 = new Label { Text = "Mật khẩu (tối thiểu 10 ký tự) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                TextBox password = new TextBox { Width = 410, Font = UITheme.FontMedium, UseSystemPasswordChar = true, Location = new Point(28, 152) };

                Label l3 = new Label { Text = "Nhập lại mật khẩu xác nhận *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 190) };
                TextBox confirm = new TextBox { Width = 410, Font = UITheme.FontMedium, UseSystemPasswordChar = true, Location = new Point(28, 212) };

                Label l4 = new Label { Text = "Nhân viên liên kết *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 250) };
                ComboBox employee = new ComboBox { Width = 410, Font = UITheme.FontMedium, DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(28, 272) };

                Label l5 = new Label { Text = "Vai trò / Phân quyền *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 310) };
                ComboBox role = new ComboBox { Width = 410, Font = UITheme.FontMedium, DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(28, 332) };

                foreach (DataRow row in employees.Rows)
                {
                    if (!Convert.ToBoolean(row["IsActive"]) || (row["UserName"] != DBNull.Value && !string.IsNullOrWhiteSpace(Convert.ToString(row["UserName"]))))
                        continue;
                    employee.Items.Add(new EmployeeChoice(Convert.ToInt32(row["EmployeeId"]), Convert.ToString(row["FullName"]) + " · " + Convert.ToString(row["EmployeeCode"])));
                }

                foreach (DataRow row in roles.Rows)
                {
                    string roleName = Convert.ToString(row["RoleName"]);
                    if (actor == null || actor.RoleName == "Admin" || roleName != "Admin")
                        role.Items.Add(roleName);
                }

                if (employee.Items.Count > 0) employee.SelectedIndex = 0;
                if (role.Items.Count > 0) role.SelectedIndex = 0;

                body.Controls.AddRange(new Control[] { l1, username, l2, password, l3, confirm, l4, employee, l5, role });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Tạo tài khoản", Width = 150, Height = 38, Location = new Point(200, 12) };
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
                    if (string.IsNullOrWhiteSpace(username.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập tên đăng nhập.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        username.Focus();
                        return;
                    }
                    if (password.Text != confirm.Text)
                    {
                        MessageBox.Show(dialog, "Mật khẩu xác nhận không khớp.", "Lỗi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (password.Text.Length < 10)
                    {
                        MessageBox.Show(dialog, "Mật khẩu phải có độ dài từ 10 ký tự trở lên.", "Lỗi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    EmployeeChoice selected = employee.SelectedItem as EmployeeChoice;
                    if (selected == null || role.SelectedItem == null)
                    {
                        MessageBox.Show(dialog, "Vui lòng tạo nhân viên trước hoặc chọn một nhân viên chưa có tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        auth.CreateAccount(actor, username.Text.Trim(), password.Text, Convert.ToString(role.SelectedItem), selected.Id);
                        MessageBox.Show(dialog, "Tạo tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, dialog);
                    }
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                    LoadAccounts();
            }
        }

        private void ToggleSelected(object sender, EventArgs e)
        {
            DataRowView row = Selected();
            if (row == null)
            {
                MessageBox.Show(this, "Vui lòng chọn tài khoản cần thay đổi trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(row["UserId"]);
            bool active = Convert.ToBoolean(row["IsActive"]);
            string targetRole = Convert.ToString(row["RoleName"]);
            string actionText = active ? "khóa tài khoản" : "mở khóa tài khoản";

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn " + actionText + " “" + Convert.ToString(row["UserName"]) + "”?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                auth.SetAccountActive(actor, id, !active, targetRole);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private sealed class EmployeeChoice
        {
            public int Id { get; private set; }
            private readonly string name;
            public EmployeeChoice(int id, string name) { Id = id; this.name = name; }
            public override string ToString() { return name; }
        }
    }
}
