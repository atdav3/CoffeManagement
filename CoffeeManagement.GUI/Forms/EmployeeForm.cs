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
    internal partial class EmployeeForm : Form
    {
        private readonly CatalogService service = new CatalogService();

        public EmployeeForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            btnFind.Click += (s, e) => LoadData();
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            cboJobFilter.SelectedIndexChanged += (s, e) => LoadData();

            btnAdd.Click += (s, e) => Edit(null);
            btnEdit.Click += (s, e) => Edit(Selected());
            btnDeactivate.Click += DeactivateSelected;
            grid.CellDoubleClick += (s, e) => Edit(Selected());

            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "EmploymentStatus" && e.Value is string status)
                {
                    e.CellStyle.ForeColor = status.StartsWith("🟢", StringComparison.Ordinal)
                        ? UITheme.Success
                        : UITheme.Danger;
                }
            };

            Shown += (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
                LoadJobTitles();
            };
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyGridStyle(grid);
            UITheme.ApplySecondaryButton(btnFind);
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

        private EmployeeDto Selected()
        {
            if (grid.CurrentRow == null) return null;
            DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
            if (row == null) return null;

            return new EmployeeDto
            {
                EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                EmployeeCode = Convert.ToString(row["EmployeeCode"]),
                FullName = Convert.ToString(row["FullName"]),
                Phone = row["Phone"] == DBNull.Value ? null : Convert.ToString(row["Phone"]),
                Email = row["Email"] == DBNull.Value ? null : Convert.ToString(row["Email"]),
                JobTitle = row["JobTitle"] == DBNull.Value ? null : Convert.ToString(row["JobTitle"])
            };
        }

        private void LoadData()
        {
            try
            {
                string title = cboJobFilter.SelectedIndex <= 0 ? null : Convert.ToString(cboJobFilter.SelectedItem);
                DataTable data = service.GetEmployees(txtSearch.Text.Trim(), title);
                AddBooleanTextColumn(data, "IsActive", "EmploymentStatus", "🟢 Đang làm việc", "🔴 Đã nghỉ việc / Khóa");
                grid.DataSource = data;

                if (grid.Columns.Contains("EmployeeId")) grid.Columns["EmployeeId"].Visible = false;
                if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"].Visible = false;

                if (grid.Columns.Contains("EmployeeCode"))
                {
                    grid.Columns["EmployeeCode"].HeaderText = "Mã nhân viên";
                    grid.Columns["EmployeeCode"].FillWeight = 80;
                }
                if (grid.Columns.Contains("FullName"))
                {
                    grid.Columns["FullName"].HeaderText = "Họ và tên";
                    grid.Columns["FullName"].FillWeight = 140;
                }
                if (grid.Columns.Contains("JobTitle"))
                {
                    grid.Columns["JobTitle"].HeaderText = "Chức vụ";
                    grid.Columns["JobTitle"].FillWeight = 100;
                }
                if (grid.Columns.Contains("Phone"))
                {
                    grid.Columns["Phone"].HeaderText = "Số điện thoại";
                    grid.Columns["Phone"].FillWeight = 100;
                }
                if (grid.Columns.Contains("Email"))
                {
                    grid.Columns["Email"].HeaderText = "Email";
                    grid.Columns["Email"].FillWeight = 120;
                }
                if (grid.Columns.Contains("EmploymentStatus"))
                {
                    grid.Columns["EmploymentStatus"].HeaderText = "Trạng thái";
                    grid.Columns["EmploymentStatus"].FillWeight = 100;
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

        private void LoadJobTitles()
        {
            try
            {
                DataTable employees = service.GetEmployees(null);
                cboJobFilter.Items.Clear();
                cboJobFilter.Items.Add("Tất cả chức danh");
                foreach (string title in employees.AsEnumerable()
                    .Select(row => row["JobTitle"] == DBNull.Value ? "" : Convert.ToString(row["JobTitle"]))
                    .Where(val => !string.IsNullOrWhiteSpace(val))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(val => val))
                {
                    cboJobFilter.Items.Add(title);
                }
                cboJobFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }

        private void Edit(EmployeeDto current)
        {
            using (Form dialog = new Form())
            {
                string formTitle = current == null ? "Thêm nhân viên mới" : "Chỉnh sửa thông tin: " + current.FullName;
                UITheme.StyleDialog(dialog, formTitle, 480, 480);

                dialog.Controls.Add(UITheme.CreateDialogHeader(
                    current == null ? "Thêm nhân viên" : "Chỉnh sửa nhân viên",
                    "Điền thông tin cá nhân và chức vụ chuyên môn."));

                Panel body = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.Background, Padding = new Padding(28, 16, 28, 16) };

                Label l1 = new Label { Text = "Mã nhân viên (vd: NV01, NV02) *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 70) };
                TextBox code = new TextBox { Text = current == null ? "" : current.EmployeeCode, Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 92) };

                Label l2 = new Label { Text = "Họ và tên nhân viên *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 130) };
                TextBox name = new TextBox { Text = current == null ? "" : current.FullName, Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 152) };

                Label l3 = new Label { Text = "Chức vụ chuyên môn *", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 190) };
                ComboBox job = new ComboBox { Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 212) };
                job.Items.AddRange(new object[] { "Quản lý cửa hàng", "Thu ngân", "Pha chế (Barista)", "Nhân viên phục vụ", "Bảo vệ" });
                job.Text = current == null ? "Thu ngân" : current.JobTitle;

                Label l4 = new Label { Text = "Số điện thoại liên hệ", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 250) };
                TextBox phone = new TextBox { Text = current == null ? "" : current.Phone, Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 272) };

                Label l5 = new Label { Text = "Địa chỉ Email", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 310) };
                TextBox email = new TextBox { Text = current == null ? "" : current.Email, Width = 410, Font = UITheme.FontMedium, Location = new Point(28, 332) };

                body.Controls.AddRange(new Control[] { l1, code, l2, name, l3, job, l4, phone, l5, email });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button save = new Button { Text = "💾 Lưu nhân viên", Width = 140, Height = 38, Location = new Point(210, 12) };
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
                    if (string.IsNullOrWhiteSpace(code.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập mã nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        code.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(name.Text))
                    {
                        MessageBox.Show(dialog, "Vui lòng nhập họ và tên nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        name.Focus();
                        return;
                    }

                    try
                    {
                        service.SaveEmployee(new EmployeeDto
                        {
                            EmployeeId = current == null ? 0 : current.EmployeeId,
                            EmployeeCode = code.Text.Trim(),
                            FullName = name.Text.Trim(),
                            Phone = string.IsNullOrWhiteSpace(phone.Text) ? null : phone.Text.Trim(),
                            Email = string.IsNullOrWhiteSpace(email.Text) ? null : email.Text.Trim(),
                            JobTitle = string.IsNullOrWhiteSpace(job.Text) ? null : job.Text.Trim()
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
                    LoadJobTitles();
            }
        }

        private void DeactivateSelected(object sender, EventArgs e)
        {
            EmployeeDto employee = Selected();
            if (employee == null)
            {
                MessageBox.Show(this, "Vui lòng chọn nhân viên cần khóa / ngừng làm việc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, "Bạn có chắc chắn muốn khóa nhân viên “" + employee.FullName + "”? Tài khoản đăng nhập liên kết cũng sẽ bị ngừng kích hoạt.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                service.DeactivateEmployee(employee.EmployeeId);
                LoadData();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
            }
        }
    }
}
