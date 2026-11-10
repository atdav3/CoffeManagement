namespace CoffeeManagement.GUI.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblLogoIcon = new System.Windows.Forms.Label();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblBullet1 = new System.Windows.Forms.Label();
            this.lblBullet2 = new System.Windows.Forms.Label();
            this.lblBullet3 = new System.Windows.Forms.Label();
            this.lblBullet4 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblHeading = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.showPassword = new System.Windows.Forms.CheckBox();
            this.signIn = new System.Windows.Forms.Button();
            this.firstRun = new System.Windows.Forms.Button();
            this.pnlConnCard = new System.Windows.Forms.Panel();
            this.connectionStatus = new System.Windows.Forms.Label();
            this.testConnection = new System.Windows.Forms.Button();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlConnCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.pnlLeft.Controls.Add(this.lblLogoIcon);
            this.pnlLeft.Controls.Add(this.lblBrandTitle);
            this.pnlLeft.Controls.Add(this.lblSubtitle);
            this.pnlLeft.Controls.Add(this.lblBullet1);
            this.pnlLeft.Controls.Add(this.lblBullet2);
            this.pnlLeft.Controls.Add(this.lblBullet3);
            this.pnlLeft.Controls.Add(this.lblBullet4);
            this.pnlLeft.Controls.Add(this.lblVersion);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(340, 520);
            this.pnlLeft.TabIndex = 0;
            // 
            // lblLogoIcon
            // 
            this.lblLogoIcon.AutoSize = true;
            this.lblLogoIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 40F);
            this.lblLogoIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblLogoIcon.Location = new System.Drawing.Point(32, 38);
            this.lblLogoIcon.Name = "lblLogoIcon";
            this.lblLogoIcon.Size = new System.Drawing.Size(91, 71);
            this.lblLogoIcon.TabIndex = 0;
            this.lblLogoIcon.Text = "☕";
            // 
            // lblBrandTitle
            // 
            this.lblBrandTitle.AutoSize = true;
            this.lblBrandTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBrandTitle.ForeColor = System.Drawing.Color.White;
            this.lblBrandTitle.Location = new System.Drawing.Point(34, 96);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(182, 60);
            this.lblBrandTitle.TabIndex = 1;
            this.lblBrandTitle.Text = "COFFEE\nMANAGEMENT";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(190)))), ((int)(((byte)(182)))));
            this.lblSubtitle.Location = new System.Drawing.Point(34, 170);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(206, 34);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Hệ thống Quản lý Bán hàng &\nQuán Cà phê Chuyên nghiệp";
            // 
            // lblBullet1
            // 
            this.lblBullet1.AutoSize = true;
            this.lblBullet1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBullet1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.lblBullet1.Location = new System.Drawing.Point(34, 235);
            this.lblBullet1.Name = "lblBullet1";
            this.lblBullet1.Size = new System.Drawing.Size(235, 15);
            this.lblBullet1.TabIndex = 3;
            this.lblBullet1.Text = "✓  Thu ngân & Bán hàng (POS) mượt mà";
            // 
            // lblBullet2
            // 
            this.lblBullet2.AutoSize = true;
            this.lblBullet2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBullet2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.lblBullet2.Location = new System.Drawing.Point(34, 270);
            this.lblBullet2.Name = "lblBullet2";
            this.lblBullet2.Size = new System.Drawing.Size(217, 15);
            this.lblBullet2.TabIndex = 4;
            this.lblBullet2.Text = "✓  Quản lý thực đơn, bàn & khu vực";
            // 
            // lblBullet3
            // 
            this.lblBullet3.AutoSize = true;
            this.lblBullet3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBullet3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.lblBullet3.Location = new System.Drawing.Point(34, 305);
            this.lblBullet3.Name = "lblBullet3";
            this.lblBullet3.Size = new System.Drawing.Size(232, 15);
            this.lblBullet3.TabIndex = 5;
            this.lblBullet3.Text = "✓  Kiểm soát kho nguyên liệu & tồn kho";
            // 
            // lblBullet4
            // 
            this.lblBullet4.AutoSize = true;
            this.lblBullet4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBullet4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.lblBullet4.Location = new System.Drawing.Point(34, 340);
            this.lblBullet4.Name = "lblBullet4";
            this.lblBullet4.Size = new System.Drawing.Size(242, 15);
            this.lblBullet4.TabIndex = 6;
            this.lblBullet4.Text = "✓  Báo cáo doanh thu & hóa đơn chi tiết";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(130)))), ((int)(((byte)(122)))));
            this.lblVersion.Location = new System.Drawing.Point(34, 465);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(209, 15);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Phiên bản 2.0.0 · .NET Framework 4.8";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.lblHeading);
            this.pnlRight.Controls.Add(this.lblPrompt);
            this.pnlRight.Controls.Add(this.lblUsername);
            this.pnlRight.Controls.Add(this.username);
            this.pnlRight.Controls.Add(this.lblPassword);
            this.pnlRight.Controls.Add(this.password);
            this.pnlRight.Controls.Add(this.showPassword);
            this.pnlRight.Controls.Add(this.signIn);
            this.pnlRight.Controls.Add(this.firstRun);
            this.pnlRight.Controls.Add(this.pnlConnCard);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(340, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(48, 36, 48, 36);
            this.pnlRight.Size = new System.Drawing.Size(500, 520);
            this.pnlRight.TabIndex = 1;
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblHeading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblHeading.Location = new System.Drawing.Point(48, 36);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(252, 32);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "Đăng nhập hệ thống";
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPrompt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblPrompt.Location = new System.Drawing.Point(50, 74);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(288, 17);
            this.lblPrompt.TabIndex = 1;
            this.lblPrompt.Text = "Vui lòng nhập tên tài khoản và mật khẩu của bạn";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblUsername.Location = new System.Drawing.Point(50, 115);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 17);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Tên đăng nhập";
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.username.Location = new System.Drawing.Point(50, 138);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(390, 25);
            this.username.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.lblPassword.Location = new System.Drawing.Point(50, 185);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(66, 17);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Mật khẩu";
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.password.Location = new System.Drawing.Point(50, 208);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(390, 25);
            this.password.TabIndex = 5;
            this.password.UseSystemPasswordChar = true;
            // 
            // showPassword
            // 
            this.showPassword.AutoSize = true;
            this.showPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.showPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.showPassword.Location = new System.Drawing.Point(50, 248);
            this.showPassword.Name = "showPassword";
            this.showPassword.Size = new System.Drawing.Size(121, 19);
            this.showPassword.TabIndex = 6;
            this.showPassword.Text = "Hiển thị mật khẩu";
            this.showPassword.UseVisualStyleBackColor = true;
            // 
            // signIn
            // 
            this.signIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.signIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.signIn.FlatAppearance.BorderSize = 0;
            this.signIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.signIn.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.signIn.ForeColor = System.Drawing.Color.White;
            this.signIn.Location = new System.Drawing.Point(50, 282);
            this.signIn.Name = "signIn";
            this.signIn.Size = new System.Drawing.Size(390, 42);
            this.signIn.TabIndex = 7;
            this.signIn.Text = "Đăng nhập ngay";
            this.signIn.UseVisualStyleBackColor = false;
            // 
            // firstRun
            // 
            this.firstRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.firstRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.firstRun.FlatAppearance.BorderSize = 0;
            this.firstRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.firstRun.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.firstRun.ForeColor = System.Drawing.Color.White;
            this.firstRun.Location = new System.Drawing.Point(50, 332);
            this.firstRun.Name = "firstRun";
            this.firstRun.Size = new System.Drawing.Size(390, 36);
            this.firstRun.TabIndex = 8;
            this.firstRun.Text = "⚡ Thiết lập tài khoản Quản trị viên ban đầu";
            this.firstRun.UseVisualStyleBackColor = false;
            this.firstRun.Visible = false;
            // 
            // pnlConnCard
            // 
            this.pnlConnCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.pnlConnCard.Controls.Add(this.connectionStatus);
            this.pnlConnCard.Controls.Add(this.testConnection);
            this.pnlConnCard.Location = new System.Drawing.Point(50, 385);
            this.pnlConnCard.Name = "pnlConnCard";
            this.pnlConnCard.Size = new System.Drawing.Size(390, 85);
            this.pnlConnCard.TabIndex = 9;
            // 
            // connectionStatus
            // 
            this.connectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.connectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.connectionStatus.Location = new System.Drawing.Point(14, 12);
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(360, 26);
            this.connectionStatus.TabIndex = 0;
            this.connectionStatus.Text = "Đang kiểm tra kết nối cơ sở dữ liệu…";
            // 
            // testConnection
            // 
            this.testConnection.BackColor = System.Drawing.Color.White;
            this.testConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.testConnection.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(224)))), ((int)(((byte)(216)))));
            this.testConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testConnection.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.testConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.testConnection.Location = new System.Drawing.Point(14, 42);
            this.testConnection.Name = "testConnection";
            this.testConnection.Size = new System.Drawing.Size(175, 30);
            this.testConnection.TabIndex = 1;
            this.testConnection.Text = "Kiểm tra kết nối CSDL";
            this.testConnection.UseVisualStyleBackColor = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.signIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(840, 520);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coffee Management | Đăng nhập hệ thống";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlConnCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblLogoIcon;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblBullet1;
        private System.Windows.Forms.Label lblBullet2;
        private System.Windows.Forms.Label lblBullet3;
        private System.Windows.Forms.Label lblBullet4;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.CheckBox showPassword;
        private System.Windows.Forms.Button signIn;
        private System.Windows.Forms.Button firstRun;
        private System.Windows.Forms.Panel pnlConnCard;
        private System.Windows.Forms.Label connectionStatus;
        private System.Windows.Forms.Button testConnection;
    }
}
