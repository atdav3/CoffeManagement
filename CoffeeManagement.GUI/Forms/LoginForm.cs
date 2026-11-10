using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class LoginForm : Form
    {
        private readonly AuthenticationService authentication = new AuthenticationService();
        private readonly DatabaseConnectionService databaseConnection = new DatabaseConnectionService();
        private bool checkingConnection;

        public UserSessionDto Session { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            ApplyCustomStyles();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            signIn.Click += SignInClicked;
            firstRun.Click += FirstRunClicked;
            testConnection.Click += async (s, e) => await CheckDatabaseConnectionAsync(true);
            showPassword.CheckedChanged += (s, e) => password.UseSystemPasswordChar = !showPassword.Checked;

            Shown += async (s, e) =>
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                    return;
                await CheckDatabaseConnectionAsync(false);
                username.Focus();
            };
        }

        private void ApplyCustomStyles()
        {
            UITheme.ApplyPrimaryButton(signIn);
            UITheme.ApplyWarningButton(firstRun);
            UITheme.ApplySecondaryButton(testConnection);

            pnlLeft.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    pnlLeft.ClientRectangle,
                    UITheme.DarkRoast,
                    Color.FromArgb(45, 28, 19),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, pnlLeft.ClientRectangle);
                }

                using (Pen pen = new Pen(UITheme.WarmAmber, 3))
                {
                    e.Graphics.DrawLine(pen, 36, 155, 120, 155);
                }
            };

            pnlConnCard.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlConnCard.Width - 1, pnlConnCard.Height - 1);
                }
            };
        }

        private async Task<bool> CheckDatabaseConnectionAsync(bool showSuccess)
        {
            if (checkingConnection) return false;
            checkingConnection = true;
            testConnection.Enabled = false;
            signIn.Enabled = false;
            connectionStatus.Text = "Đang kiểm tra kết nối SQL Server…";
            connectionStatus.ForeColor = UITheme.TextSecondary;
            Cursor = Cursors.WaitCursor;

            try
            {
                DatabaseConnectionResultDto result = await Task.Run(() => databaseConnection.TestConnection());
                if (IsDisposed || Disposing) return false;

                firstRun.Visible = result.UserCountProcedureAvailable && result.UserCount == 0;
                signIn.Enabled = result.UserCountProcedureAvailable;

                if (!result.UserCountProcedureAvailable)
                {
                    connectionStatus.Text = "⚠️ Đã kết nối CSDL nhưng thiếu Stored Procedure hệ thống.";
                    connectionStatus.ForeColor = UITheme.Warning;
                }
                else if (result.UserCount == 0)
                {
                    connectionStatus.Text = "ℹ️ Đã kết nối. Hệ thống cần thiết lập tài khoản ban đầu.";
                    connectionStatus.ForeColor = UITheme.WarmAmber;
                }
                else
                {
                    connectionStatus.Text = "🟢 Kết nối thành công CSDL " + result.DatabaseName;
                    connectionStatus.ForeColor = UITheme.Success;
                }

                if (showSuccess)
                {
                    string instance = string.IsNullOrWhiteSpace(result.InstanceName) ? "Default instance" : result.InstanceName;
                    string userRows = result.UsersTableAvailable && result.UserCount.HasValue
                        ? result.UserCount.Value.ToString()
                        : "Không tìm thấy bảng dbo.Users";
                    string procedure = result.UserCountProcedureAvailable ? "Đầy đủ" : "CHƯA CÓ";

                    string message = "Kết nối máy chủ SQL Server thành công!" + Environment.NewLine + Environment.NewLine +
                        "• Máy chủ (Server): " + result.ServerName + Environment.NewLine +
                        "• Thể hiện (Instance): " + instance + Environment.NewLine +
                        "• Cơ sở dữ liệu: " + result.DatabaseName + Environment.NewLine +
                        "• Số tài khoản người dùng: " + userRows + Environment.NewLine +
                        "• Thủ tục hệ thống sp_User_Count: " + procedure;

                    MessageBox.Show(this, message, "Kiểm tra kết nối CSDL", MessageBoxButtons.OK,
                        result.UserCountProcedureAvailable ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                {
                    connectionStatus.Text = "🔴 Không thể kết nối SQL Server. Vui lòng kiểm tra App.config";
                    connectionStatus.ForeColor = UITheme.Danger;
                    UiError.Show(ex, this);
                }
                return false;
            }
            finally
            {
                checkingConnection = false;
                if (!IsDisposed && !Disposing)
                {
                    testConnection.Enabled = true;
                    Cursor = Cursors.Default;
                }
            }
        }

        private void SignInClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text))
            {
                MessageBox.Show(this, "Vui lòng nhập tên đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                username.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show(this, "Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                password.Focus();
                return;
            }

            try
            {
                Session = authentication.SignIn(username.Text.Trim(), password.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiError.Show(ex, this);
                password.Clear();
                password.Focus();
            }
        }

        private void FirstRunClicked(object sender, EventArgs e)
        {
            using (Form setup = new Form())
            {
                UITheme.StyleDialog(setup, "Thiết lập tài khoản Quản trị viên ban đầu", 440, 360);
                setup.Controls.Add(UITheme.CreateDialogHeader("Khởi tạo hệ thống", "Tạo tài khoản quản trị viên tối cao (Admin) để bắt đầu sử dụng."));

                Panel body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(28, 16, 28, 16), BackColor = UITheme.Background };
                TextBox user = new TextBox { Width = 370, Font = UITheme.FontMedium };
                TextBox pass = new TextBox { Width = 370, Font = UITheme.FontMedium, UseSystemPasswordChar = true };
                TextBox confirm = new TextBox { Width = 370, Font = UITheme.FontMedium, UseSystemPasswordChar = true };

                Label l1 = new Label { Text = "Tên đăng nhập quản trị viên", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 75) };
                user.Location = new Point(28, 98);
                Label l2 = new Label { Text = "Mật khẩu (tối thiểu 10 ký tự)", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 138) };
                pass.Location = new Point(28, 161);
                Label l3 = new Label { Text = "Nhập lại mật khẩu", AutoSize = true, Font = UITheme.FontSemibold, Location = new Point(28, 201) };
                confirm.Location = new Point(28, 224);

                body.Controls.AddRange(new Control[] { l1, user, l2, pass, l3, confirm });

                Panel footer = UITheme.CreateDialogFooter(62);
                Button create = new Button { Text = "Tạo tài khoản Quản trị", Width = 180, Height = 38 };
                UITheme.ApplyPrimaryButton(create);
                create.Location = new Point(140, 12);

                Button cancel = new Button { Text = "Hủy bỏ", Width = 90, Height = 38 };
                UITheme.ApplySecondaryButton(cancel);
                cancel.Location = new Point(330, 12);
                cancel.Click += (s, args) => setup.Close();

                footer.Controls.Add(create);
                footer.Controls.Add(cancel);

                setup.Controls.Add(body);
                setup.Controls.Add(footer);

                create.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(user.Text))
                    {
                        MessageBox.Show(setup, "Vui lòng nhập tên đăng nhập quản trị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        user.Focus();
                        return;
                    }
                    if (pass.Text != confirm.Text)
                    {
                        MessageBox.Show(setup, "Mật khẩu xác nhận không trùng khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (pass.Text.Length < 10)
                    {
                        MessageBox.Show(setup, "Mật khẩu phải có độ dài tối thiểu 10 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        authentication.CreateInitialAdministrator(user.Text.Trim(), pass.Text);
                        MessageBox.Show(setup, "Đã tạo tài khoản quản trị thành công! Vui lòng đăng nhập để tiếp tục.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        setup.DialogResult = DialogResult.OK;
                        setup.Close();
                    }
                    catch (Exception ex)
                    {
                        UiError.Show(ex, setup);
                    }
                };

                setup.ShowDialog(this);
            }

            try
            {
                firstRun.Visible = authentication.IsInitialSetupRequired();
            }
            catch
            {
                firstRun.Visible = false;
            }
        }
    }
}
