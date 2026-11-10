using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel sidebar;
        private Panel brandPanel;
        private Label logo;
        private Label slogan;
        private Panel userPanel;
        private Label account;
        private Button logout;
        private Panel navContainer;
        private Label lblNavSection1;
        private Button dashboardNavBtn;
        private Button btnNavPos;
        private Button btnNavOrders;
        private Button btnNavTables;
        private Label lblNavSection2;
        private Button btnNavCatalog;
        private Button btnNavCategories;
        private Button btnNavInventory;
        private Button btnNavPromotions;
        private Label lblNavSection3;
        private Button btnNavEmployees;
        private Button btnNavReports;
        private Button btnNavAuditLog;
        private Button btnNavAccounts;
        private Panel header;
        private Label pageTitle;
        private Label pageSubtitle;
        private Label clockLabel;
        private Panel content;

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
            this.sidebar = new System.Windows.Forms.Panel();
            this.navContainer = new System.Windows.Forms.Panel();
            this.btnNavAccounts = new System.Windows.Forms.Button();
            this.btnNavAuditLog = new System.Windows.Forms.Button();
            this.btnNavReports = new System.Windows.Forms.Button();
            this.btnNavEmployees = new System.Windows.Forms.Button();
            this.lblNavSection3 = new System.Windows.Forms.Label();
            this.btnNavPromotions = new System.Windows.Forms.Button();
            this.btnNavInventory = new System.Windows.Forms.Button();
            this.btnNavCategories = new System.Windows.Forms.Button();
            this.btnNavCatalog = new System.Windows.Forms.Button();
            this.lblNavSection2 = new System.Windows.Forms.Label();
            this.btnNavTables = new System.Windows.Forms.Button();
            this.btnNavOrders = new System.Windows.Forms.Button();
            this.btnNavPos = new System.Windows.Forms.Button();
            this.dashboardNavBtn = new System.Windows.Forms.Button();
            this.lblNavSection1 = new System.Windows.Forms.Label();
            this.userPanel = new System.Windows.Forms.Panel();
            this.logout = new System.Windows.Forms.Button();
            this.account = new System.Windows.Forms.Label();
            this.brandPanel = new System.Windows.Forms.Panel();
            this.slogan = new System.Windows.Forms.Label();
            this.logo = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.Panel();
            this.clockLabel = new System.Windows.Forms.Label();
            this.pageSubtitle = new System.Windows.Forms.Label();
            this.pageTitle = new System.Windows.Forms.Label();
            this.content = new System.Windows.Forms.Panel();
            this.sidebar.SuspendLayout();
            this.navContainer.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.brandPanel.SuspendLayout();
            this.header.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.AutoScroll = true;
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.sidebar.Controls.Add(this.navContainer);
            this.sidebar.Controls.Add(this.userPanel);
            this.sidebar.Controls.Add(this.brandPanel);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(260, 720);
            this.sidebar.TabIndex = 0;
            // 
            // navContainer
            // 
            this.navContainer.AutoScroll = true;
            this.navContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.navContainer.Controls.Add(this.btnNavAccounts);
            this.navContainer.Controls.Add(this.btnNavAuditLog);
            this.navContainer.Controls.Add(this.btnNavReports);
            this.navContainer.Controls.Add(this.btnNavEmployees);
            this.navContainer.Controls.Add(this.lblNavSection3);
            this.navContainer.Controls.Add(this.btnNavPromotions);
            this.navContainer.Controls.Add(this.btnNavInventory);
            this.navContainer.Controls.Add(this.btnNavCategories);
            this.navContainer.Controls.Add(this.btnNavCatalog);
            this.navContainer.Controls.Add(this.lblNavSection2);
            this.navContainer.Controls.Add(this.btnNavTables);
            this.navContainer.Controls.Add(this.btnNavOrders);
            this.navContainer.Controls.Add(this.btnNavPos);
            this.navContainer.Controls.Add(this.dashboardNavBtn);
            this.navContainer.Controls.Add(this.lblNavSection1);
            this.navContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navContainer.Location = new System.Drawing.Point(0, 90);
            this.navContainer.Name = "navContainer";
            this.navContainer.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.navContainer.Size = new System.Drawing.Size(260, 515);
            this.navContainer.TabIndex = 2;
            // 
            // btnNavAccounts
            // 
            this.btnNavAccounts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavAccounts.FlatAppearance.BorderSize = 0;
            this.btnNavAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAccounts.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavAccounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavAccounts.Location = new System.Drawing.Point(0, 592);
            this.btnNavAccounts.Name = "btnNavAccounts";
            this.btnNavAccounts.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavAccounts.Size = new System.Drawing.Size(239, 44);
            this.btnNavAccounts.TabIndex = 14;
            this.btnNavAccounts.Text = "🔐  Tài khoản & Phân quyền";
            this.btnNavAccounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavAccounts.UseVisualStyleBackColor = false;
            // 
            // btnNavAuditLog
            // 
            this.btnNavAuditLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavAuditLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAuditLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavAuditLog.FlatAppearance.BorderSize = 0;
            this.btnNavAuditLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAuditLog.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavAuditLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavAuditLog.Location = new System.Drawing.Point(0, 548);
            this.btnNavAuditLog.Name = "btnNavAuditLog";
            this.btnNavAuditLog.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavAuditLog.Size = new System.Drawing.Size(239, 44);
            this.btnNavAuditLog.TabIndex = 13;
            this.btnNavAuditLog.Text = "📜  Nhật ký hoạt động";
            this.btnNavAuditLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavAuditLog.UseVisualStyleBackColor = false;
            // 
            // btnNavReports
            // 
            this.btnNavReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavReports.FlatAppearance.BorderSize = 0;
            this.btnNavReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavReports.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavReports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavReports.Location = new System.Drawing.Point(0, 504);
            this.btnNavReports.Name = "btnNavReports";
            this.btnNavReports.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavReports.Size = new System.Drawing.Size(239, 44);
            this.btnNavReports.TabIndex = 12;
            this.btnNavReports.Text = "📈  Báo cáo & Thống kê";
            this.btnNavReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavReports.UseVisualStyleBackColor = false;
            // 
            // btnNavEmployees
            // 
            this.btnNavEmployees.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavEmployees.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavEmployees.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavEmployees.FlatAppearance.BorderSize = 0;
            this.btnNavEmployees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavEmployees.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavEmployees.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavEmployees.Location = new System.Drawing.Point(0, 460);
            this.btnNavEmployees.Name = "btnNavEmployees";
            this.btnNavEmployees.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavEmployees.Size = new System.Drawing.Size(239, 44);
            this.btnNavEmployees.TabIndex = 11;
            this.btnNavEmployees.Text = "👥  Quản lý nhân viên";
            this.btnNavEmployees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavEmployees.UseVisualStyleBackColor = false;
            // 
            // lblNavSection3
            // 
            this.lblNavSection3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNavSection3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblNavSection3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(125)))), ((int)(((byte)(115)))));
            this.lblNavSection3.Location = new System.Drawing.Point(0, 428);
            this.lblNavSection3.Name = "lblNavSection3";
            this.lblNavSection3.Padding = new System.Windows.Forms.Padding(18, 0, 0, 6);
            this.lblNavSection3.Size = new System.Drawing.Size(239, 32);
            this.lblNavSection3.TabIndex = 10;
            this.lblNavSection3.Text = "HỆ THỐNG & NHÂN SỰ";
            this.lblNavSection3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnNavPromotions
            // 
            this.btnNavPromotions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavPromotions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavPromotions.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavPromotions.FlatAppearance.BorderSize = 0;
            this.btnNavPromotions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPromotions.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavPromotions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavPromotions.Location = new System.Drawing.Point(0, 384);
            this.btnNavPromotions.Name = "btnNavPromotions";
            this.btnNavPromotions.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavPromotions.Size = new System.Drawing.Size(239, 44);
            this.btnNavPromotions.TabIndex = 9;
            this.btnNavPromotions.Text = "🎁  Khuyến mãi & Giảm giá";
            this.btnNavPromotions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavPromotions.UseVisualStyleBackColor = false;
            // 
            // btnNavInventory
            // 
            this.btnNavInventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavInventory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavInventory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavInventory.FlatAppearance.BorderSize = 0;
            this.btnNavInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavInventory.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavInventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavInventory.Location = new System.Drawing.Point(0, 340);
            this.btnNavInventory.Name = "btnNavInventory";
            this.btnNavInventory.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavInventory.Size = new System.Drawing.Size(239, 44);
            this.btnNavInventory.TabIndex = 8;
            this.btnNavInventory.Text = "📦  Kho nguyên liệu";
            this.btnNavInventory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavInventory.UseVisualStyleBackColor = false;
            // 
            // btnNavCategories
            // 
            this.btnNavCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavCategories.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavCategories.FlatAppearance.BorderSize = 0;
            this.btnNavCategories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavCategories.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavCategories.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavCategories.Location = new System.Drawing.Point(0, 296);
            this.btnNavCategories.Name = "btnNavCategories";
            this.btnNavCategories.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavCategories.Size = new System.Drawing.Size(239, 44);
            this.btnNavCategories.TabIndex = 7;
            this.btnNavCategories.Text = "📂  Danh mục món";
            this.btnNavCategories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavCategories.UseVisualStyleBackColor = false;
            // 
            // btnNavCatalog
            // 
            this.btnNavCatalog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavCatalog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavCatalog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavCatalog.FlatAppearance.BorderSize = 0;
            this.btnNavCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavCatalog.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavCatalog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavCatalog.Location = new System.Drawing.Point(0, 252);
            this.btnNavCatalog.Name = "btnNavCatalog";
            this.btnNavCatalog.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavCatalog.Size = new System.Drawing.Size(239, 44);
            this.btnNavCatalog.TabIndex = 6;
            this.btnNavCatalog.Text = "☕  Thực đơn & Món";
            this.btnNavCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavCatalog.UseVisualStyleBackColor = false;
            // 
            // lblNavSection2
            // 
            this.lblNavSection2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNavSection2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblNavSection2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(125)))), ((int)(((byte)(115)))));
            this.lblNavSection2.Location = new System.Drawing.Point(0, 220);
            this.lblNavSection2.Name = "lblNavSection2";
            this.lblNavSection2.Padding = new System.Windows.Forms.Padding(18, 0, 0, 6);
            this.lblNavSection2.Size = new System.Drawing.Size(239, 32);
            this.lblNavSection2.TabIndex = 5;
            this.lblNavSection2.Text = "QUẢN TRỊ THỰC ĐƠN & KHO";
            this.lblNavSection2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnNavTables
            // 
            this.btnNavTables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavTables.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavTables.FlatAppearance.BorderSize = 0;
            this.btnNavTables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavTables.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavTables.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavTables.Location = new System.Drawing.Point(0, 176);
            this.btnNavTables.Name = "btnNavTables";
            this.btnNavTables.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavTables.Size = new System.Drawing.Size(239, 44);
            this.btnNavTables.TabIndex = 4;
            this.btnNavTables.Text = "🪑  Sơ đồ bàn & Khu vực";
            this.btnNavTables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavTables.UseVisualStyleBackColor = false;
            // 
            // btnNavOrders
            // 
            this.btnNavOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavOrders.FlatAppearance.BorderSize = 0;
            this.btnNavOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavOrders.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavOrders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavOrders.Location = new System.Drawing.Point(0, 132);
            this.btnNavOrders.Name = "btnNavOrders";
            this.btnNavOrders.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavOrders.Size = new System.Drawing.Size(239, 44);
            this.btnNavOrders.TabIndex = 3;
            this.btnNavOrders.Text = "🧾  Hóa đơn & Đơn hàng";
            this.btnNavOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavOrders.UseVisualStyleBackColor = false;
            // 
            // btnNavPos
            // 
            this.btnNavPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.btnNavPos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavPos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavPos.FlatAppearance.BorderSize = 0;
            this.btnNavPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavPos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(210)))));
            this.btnNavPos.Location = new System.Drawing.Point(0, 88);
            this.btnNavPos.Name = "btnNavPos";
            this.btnNavPos.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnNavPos.Size = new System.Drawing.Size(239, 44);
            this.btnNavPos.TabIndex = 2;
            this.btnNavPos.Text = "🛒  Điểm bán hàng (POS)";
            this.btnNavPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavPos.UseVisualStyleBackColor = false;
            // 
            // dashboardNavBtn
            // 
            this.dashboardNavBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(30)))), ((int)(((byte)(22)))));
            this.dashboardNavBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dashboardNavBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.dashboardNavBtn.FlatAppearance.BorderSize = 0;
            this.dashboardNavBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dashboardNavBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dashboardNavBtn.ForeColor = System.Drawing.Color.White;
            this.dashboardNavBtn.Location = new System.Drawing.Point(0, 44);
            this.dashboardNavBtn.Name = "dashboardNavBtn";
            this.dashboardNavBtn.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.dashboardNavBtn.Size = new System.Drawing.Size(239, 44);
            this.dashboardNavBtn.TabIndex = 1;
            this.dashboardNavBtn.Text = "📊  Tổng quan kinh doanh";
            this.dashboardNavBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dashboardNavBtn.UseVisualStyleBackColor = false;
            // 
            // lblNavSection1
            // 
            this.lblNavSection1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNavSection1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblNavSection1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(125)))), ((int)(((byte)(115)))));
            this.lblNavSection1.Location = new System.Drawing.Point(0, 8);
            this.lblNavSection1.Name = "lblNavSection1";
            this.lblNavSection1.Padding = new System.Windows.Forms.Padding(18, 0, 0, 6);
            this.lblNavSection1.Size = new System.Drawing.Size(239, 36);
            this.lblNavSection1.TabIndex = 0;
            this.lblNavSection1.Text = "VẬN HÀNH & BÁN HÀNG";
            this.lblNavSection1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userPanel
            // 
            this.userPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(13)))), ((int)(((byte)(9)))));
            this.userPanel.Controls.Add(this.logout);
            this.userPanel.Controls.Add(this.account);
            this.userPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userPanel.Location = new System.Drawing.Point(0, 605);
            this.userPanel.Name = "userPanel";
            this.userPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.userPanel.Size = new System.Drawing.Size(260, 115);
            this.userPanel.TabIndex = 1;
            // 
            // logout
            // 
            this.logout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(30)))), ((int)(((byte)(22)))));
            this.logout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logout.FlatAppearance.BorderSize = 0;
            this.logout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.logout.ForeColor = System.Drawing.Color.White;
            this.logout.Location = new System.Drawing.Point(16, 65);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(228, 38);
            this.logout.TabIndex = 1;
            this.logout.Text = "🚪  Đăng xuất hệ thống";
            this.logout.UseVisualStyleBackColor = false;
            // 
            // account
            // 
            this.account.Dock = System.Windows.Forms.DockStyle.Top;
            this.account.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.account.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(210)))), ((int)(((byte)(200)))));
            this.account.Location = new System.Drawing.Point(16, 12);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(228, 44);
            this.account.TabIndex = 0;
            this.account.Text = "👤 Quản trị viên\n🏷️ Quản trị hệ thống";
            this.account.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // brandPanel
            // 
            this.brandPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(12)))));
            this.brandPanel.Controls.Add(this.slogan);
            this.brandPanel.Controls.Add(this.logo);
            this.brandPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.brandPanel.Location = new System.Drawing.Point(0, 0);
            this.brandPanel.Name = "brandPanel";
            this.brandPanel.Padding = new System.Windows.Forms.Padding(20, 16, 20, 10);
            this.brandPanel.Size = new System.Drawing.Size(260, 90);
            this.brandPanel.TabIndex = 0;
            // 
            // slogan
            // 
            this.slogan.AutoSize = true;
            this.slogan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.slogan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(145)))), ((int)(((byte)(135)))));
            this.slogan.Location = new System.Drawing.Point(20, 48);
            this.slogan.Name = "slogan";
            this.slogan.Size = new System.Drawing.Size(214, 20);
            this.slogan.TabIndex = 1;
            this.slogan.Text = "Hệ thống Quản lý Quán Cà phê";
            // 
            // logo
            // 
            this.logo.AutoSize = true;
            this.logo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.logo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.logo.Location = new System.Drawing.Point(18, 20);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(273, 28);
            this.logo.TabIndex = 0;
            this.logo.Text = "☕  COFFEE MANAGEMENT";
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Controls.Add(this.clockLabel);
            this.header.Controls.Add(this.pageSubtitle);
            this.header.Controls.Add(this.pageTitle);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(260, 0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(28, 14, 28, 12);
            this.header.Size = new System.Drawing.Size(940, 72);
            this.header.TabIndex = 1;
            // 
            // clockLabel
            // 
            this.clockLabel.AutoSize = true;
            this.clockLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.clockLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.clockLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.clockLabel.Location = new System.Drawing.Point(804, 14);
            this.clockLabel.Name = "clockLabel";
            this.clockLabel.Padding = new System.Windows.Forms.Padding(0, 20, 16, 0);
            this.clockLabel.Size = new System.Drawing.Size(108, 40);
            this.clockLabel.TabIndex = 2;
            this.clockLabel.Text = "🕒 Đồng hồ";
            this.clockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pageSubtitle
            // 
            this.pageSubtitle.AutoSize = true;
            this.pageSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pageSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.pageSubtitle.Location = new System.Drawing.Point(30, 42);
            this.pageSubtitle.Name = "pageSubtitle";
            this.pageSubtitle.Size = new System.Drawing.Size(429, 20);
            this.pageSubtitle.TabIndex = 1;
            this.pageSubtitle.Text = "Hệ thống giám sát chỉ số bán hàng và số liệu vận hành trực tiếp";
            // 
            // pageTitle
            // 
            this.pageTitle.AutoSize = true;
            this.pageTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.pageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.pageTitle.Location = new System.Drawing.Point(28, 12);
            this.pageTitle.Name = "pageTitle";
            this.pageTitle.Size = new System.Drawing.Size(304, 37);
            this.pageTitle.TabIndex = 0;
            this.pageTitle.Text = "Tổng quan kinh doanh";
            // 
            // content
            // 
            this.content.AutoScroll = true;
            this.content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.content.Location = new System.Drawing.Point(260, 72);
            this.content.Name = "content";
            this.content.Padding = new System.Windows.Forms.Padding(28);
            this.content.Size = new System.Drawing.Size(940, 648);
            this.content.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.content);
            this.Controls.Add(this.header);
            this.Controls.Add(this.sidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coffee Management - Hệ thống Quản trị & Bán hàng Cà phê";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.sidebar.ResumeLayout(false);
            this.navContainer.ResumeLayout(false);
            this.userPanel.ResumeLayout(false);
            this.brandPanel.ResumeLayout(false);
            this.brandPanel.PerformLayout();
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
