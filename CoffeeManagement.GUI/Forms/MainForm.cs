using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CoffeeManagement.BUS.Services;
using CoffeeManagement.DTO.Entities;
using CoffeeManagement.GUI.Helpers;

using System.ComponentModel;

namespace CoffeeManagement.GUI.Forms
{
    internal partial class MainForm : Form
    {
        private readonly UserSessionDto session;
        private readonly DashboardService dashboard = new DashboardService();
        private readonly Timer clockTimer = new Timer();

        private Form currentChildForm;
        private Button activeNavButton;
        private int pageVersion;

        public MainForm()
        {
            InitializeComponent();
            WireNavigation();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && !DesignMode)
            {
                StartClock();
                ShowDashboard();
            }
        }

        public MainForm(UserSessionDto session) : this()
        {
            this.session = session;
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            if (session == null) return;
            string roleVN = session.RoleName == "Admin" ? "Quản trị viên" : session.RoleName == "Manager" ? "Quản lý cửa hàng" : "Thu ngân / Bán hàng";
            account.Text = "👤 " + (session.EmployeeName ?? session.UserName) + "\n🏷️ " + roleVN;

            bool isManagement = session.RoleName == "Admin" || session.RoleName == "Manager";
            lblNavSection2.Visible = isManagement;
            btnNavCatalog.Visible = isManagement;
            btnNavCategories.Visible = isManagement;
            btnNavInventory.Visible = isManagement;
            btnNavPromotions.Visible = isManagement;

            lblNavSection3.Visible = isManagement;
            btnNavEmployees.Visible = isManagement;
            btnNavReports.Visible = isManagement;
            btnNavAuditLog.Visible = isManagement;
            btnNavAccounts.Visible = isManagement;
        }

        private void StartClock()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) =>
            {
                clockLabel.Text = "🕒 " + DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss");
            };
            clockTimer.Start();
        }

        private void WireNavigation()
        {
            brandPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(45, 30, 22), 1))
                {
                    e.Graphics.DrawLine(pen, 20, brandPanel.Height - 1, brandPanel.Width - 20, brandPanel.Height - 1);
                }
            };

            userPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(45, 30, 22), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, userPanel.Width, 0);
                }
            };

            header.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
            };

            logout.Click += (s, e) =>
            {
                if (MessageBox.Show(this, "Bạn có chắc chắn muốn đăng xuất tài khoản hiện tại?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Close();
            };

            SetupNavItem(dashboardNavBtn, ShowDashboard);
            SetupNavItem(btnNavPos, OpenPos);
            SetupNavItem(btnNavOrders, OpenOrders);
            SetupNavItem(btnNavTables, OpenTables);
            SetupNavItem(btnNavCatalog, OpenCatalog);
            SetupNavItem(btnNavCategories, OpenCategories);
            SetupNavItem(btnNavInventory, OpenInventory);
            SetupNavItem(btnNavPromotions, OpenPromotions);
            SetupNavItem(btnNavEmployees, OpenEmployees);
            SetupNavItem(btnNavReports, OpenReports);
            SetupNavItem(btnNavAuditLog, OpenAuditLog);
            SetupNavItem(btnNavAccounts, OpenAccounts);
        }

        private void SetupNavItem(Button btn, Action action)
        {
            btn.Click += (s, e) =>
            {
                SetActiveButton(btn);
                action();
            };

            btn.MouseEnter += (s, e) =>
            {
                if (btn != activeNavButton)
                    btn.BackColor = UITheme.DarkRoastHover;
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn != activeNavButton)
                    btn.BackColor = UITheme.DarkRoast;
            };

            btn.Paint += (s, e) =>
            {
                if (btn == activeNavButton)
                {
                    using (SolidBrush brush = new SolidBrush(UITheme.WarmAmber))
                    {
                        e.Graphics.FillRectangle(brush, 0, 0, 4, btn.Height);
                    }
                }
            };
        }

        private void SetActiveButton(Button btn)
        {
            if (activeNavButton != null)
            {
                activeNavButton.BackColor = UITheme.DarkRoast;
                activeNavButton.ForeColor = Color.FromArgb(220, 215, 210);
                activeNavButton.Invalidate();
            }

            activeNavButton = btn;
            if (activeNavButton != null)
            {
                activeNavButton.BackColor = UITheme.DarkRoastActive;
                activeNavButton.ForeColor = Color.White;
                activeNavButton.Invalidate();
            }
        }

        private void SetHeader(string title, string subtitle)
        {
            pageTitle.Text = title;
            pageSubtitle.Text = subtitle;
        }

        private void OpenChildPage(Form childForm, string title, string subtitle)
        {
            pageVersion++;
            SetHeader(title, subtitle);

            if (currentChildForm != null)
            {
                content.Controls.Remove(currentChildForm);
                currentChildForm.Dispose();
                currentChildForm = null;
            }

            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            content.Padding = new Padding(0);
            content.Controls.Clear();
            content.Controls.Add(childForm);
            childForm.Show();
        }

        // ==========================================
        // DASHBOARD VIEW
        // ==========================================
        private void ShowDashboard()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            pageVersion++;
            SetActiveButton(dashboardNavBtn);
            SetHeader("Tổng quan kinh doanh", "Số liệu doanh thu, đơn hàng trực tiếp và xu hướng bán hàng 7 ngày qua");

            if (currentChildForm != null)
            {
                content.Controls.Remove(currentChildForm);
                currentChildForm.Dispose();
                currentChildForm = null;
            }

            content.Padding = new Padding(28);
            content.Controls.Clear();

            int requestVersion = pageVersion;
            Label loadingLbl = new Label
            {
                Text = "⏳ Đang tải dữ liệu tổng quan từ cơ sở dữ liệu…",
                AutoSize = true,
                Font = UITheme.FontMedium,
                ForeColor = UITheme.TextSecondary,
                Location = new Point(4, 8)
            };
            content.Controls.Add(loadingLbl);
            Cursor = Cursors.WaitCursor;

            Task.Run(() => dashboard.GetSummary())
                .ContinueWith(t =>
                {
                    if (IsDisposed || Disposing || requestVersion != pageVersion) return;
                    Cursor = Cursors.Default;

                    if (t.IsFaulted)
                    {
                        UiError.Show(t.Exception, this);
                        return;
                    }

                    RenderDashboard(t.Result);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void RenderDashboard(DataSet data)
        {
            content.Controls.Clear();

            DataRow totals = data.Tables[0].Rows[0];
            DataRow tables = data.Tables[1].Rows[0];

            decimal rev = Convert.ToDecimal(totals["Revenue"]);
            long invCount = Convert.ToInt64(totals["InvoiceCount"]);
            decimal itemsSold = Convert.ToDecimal(totals["ItemsSold"]);
            int occupied = Convert.ToInt32(tables["OccupiedTables"]);
            int avail = Convert.ToInt32(tables["AvailableTables"]);

            // KPI Cards row
            FlowLayoutPanel cards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 135,
                WrapContents = false,
                AutoScroll = true
            };

            cards.Controls.Add(UITheme.CreateCard("DOANH THU HÔM NAY", rev.ToString("N0") + " ₫", "Doanh thu chốt ca hôm nay", UITheme.Espresso, "💰"));
            cards.Controls.Add(UITheme.CreateCard("HÓA ĐƠN ĐÃ LẬP", invCount.ToString("N0"), "Số lượt thanh toán thành công", UITheme.Success, "🧾"));
            cards.Controls.Add(UITheme.CreateCard("MÓN ĐÃ BÁN", itemsSold.ToString("N0"), "Ly cà phê & đồ uống", UITheme.WarmAmber, "☕"));
            cards.Controls.Add(UITheme.CreateCard("BÀN ĐANG PHỤC VỤ", occupied.ToString("N0"), "Bàn đang có khách ngồi", UITheme.Danger, "🔴"));
            cards.Controls.Add(UITheme.CreateCard("BÀN CÒN TRỐNG", avail.ToString("N0"), "Sẵn sàng đón khách mới", UITheme.Info, "🟢"));

            content.Controls.Add(cards);

            // Chart Section Title
            Label chartTitle = new Label
            {
                Text = "📈 Biểu đồ Xu hướng Doanh thu & Top Sản phẩm bán chạy",
                AutoSize = true,
                ForeColor = UITheme.TextPrimary,
                Font = UITheme.FontSubheader,
                Top = 155,
                Left = 4
            };
            content.Controls.Add(chartTitle);

            // Charts Flow Panel
            FlowLayoutPanel charts = new FlowLayoutPanel
            {
                Left = 0,
                Top = 190,
                Width = Math.Max(content.ClientSize.Width - 60, 850),
                Height = 350,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                WrapContents = false,
                AutoScroll = true
            };

            charts.Controls.Add(BuildChart(
                "Doanh thu 7 ngày gần nhất (₫)",
                "SalesDate",
                "Revenue",
                data.Tables.Count > 2 ? data.Tables[2] : null,
                SeriesChartType.Spline,
                UITheme.Espresso));

            charts.Controls.Add(BuildChart(
                "Top món bán chạy nhất trong 30 ngày (Ly)",
                "ProductName",
                "QuantitySold",
                data.Tables.Count > 3 ? data.Tables[3] : null,
                SeriesChartType.Bar,
                UITheme.WarmAmber));

            content.Controls.Add(charts);

            // Bottom bar: Live notice & Refresh
            Panel footerBar = new Panel
            {
                Left = 0,
                Top = 555,
                Width = Math.Max(content.ClientSize.Width - 60, 850),
                Height = 60,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            string businessDate = data.Tables.Count > 2 && data.Tables[2].Rows.Count > 0
                ? Convert.ToDateTime(data.Tables[2].Rows[data.Tables[2].Rows.Count - 1]["SalesDate"]).ToString("dddd, dd/MM/yyyy")
                : DateTime.Now.ToString("dddd, dd/MM/yyyy");

            Label dateNote = new Label
            {
                Text = "⚡ Dữ liệu trực tiếp từ SQL Server (CoffeeManagementDB) · Ngày kinh doanh: " + businessDate,
                AutoSize = true,
                ForeColor = UITheme.TextSecondary,
                Font = UITheme.FontSmall,
                Location = new Point(4, 18)
            };

            Button refresh = new Button
            {
                Text = "🔄  Làm mới dữ liệu",
                Width = 160,
                Height = 38,
                Location = new Point(footerBar.Width - 170, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            UITheme.ApplyPrimaryButton(refresh);
            refresh.Click += (s, e) => ShowDashboard();

            footerBar.Controls.Add(dateNote);
            footerBar.Controls.Add(refresh);
            content.Controls.Add(footerBar);
        }

        private static Chart BuildChart(string title, string labelColumn, string valueColumn, DataTable data, SeriesChartType type, Color themeColor)
        {
            Chart chart = new Chart
            {
                Width = 520,
                Height = 330,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0)
            };

            chart.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, chart.Width - 1, chart.Height - 1);
                }
            };

            ChartArea area = new ChartArea("main")
            {
                BackColor = Color.White
            };
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = type == SeriesChartType.Bar ? 0 : -25;
            area.AxisX.LabelStyle.Font = UITheme.FontSmall;
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(242, 238, 234);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(242, 238, 234);
            area.AxisY.LabelStyle.Font = UITheme.FontSmall;
            area.AxisY.LabelStyle.Format = type == SeriesChartType.Bar ? "0" : "N0";
            chart.ChartAreas.Add(area);

            Title chartTitle = new Title(title, Docking.Top, UITheme.FontSemibold, UITheme.TextPrimary)
            {
                Alignment = ContentAlignment.MiddleLeft
            };
            chart.Titles.Add(chartTitle);

            Series series = new Series("data")
            {
                ChartType = type,
                Color = themeColor,
                BorderWidth = 3,
                IsValueShownAsLabel = true,
                XValueType = ChartValueType.String,
                Font = UITheme.FontXs
            };
            chart.Series.Add(series);

            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    string labelVal = Convert.ToString(row[labelColumn]);
                    if (DateTime.TryParse(labelVal, out DateTime dtVal))
                        labelVal = dtVal.ToString("dd/MM");

                    double numVal = Convert.ToDouble(row[valueColumn]);
                    int ptIdx = series.Points.AddXY(labelVal, numVal);
                    if (type == SeriesChartType.Spline || type == SeriesChartType.Line)
                    {
                        series.Points[ptIdx].MarkerStyle = MarkerStyle.Circle;
                        series.Points[ptIdx].MarkerSize = 7;
                        series.Points[ptIdx].MarkerColor = UITheme.WarmAmber;
                        series.Points[ptIdx].Label = numVal.ToString("N0");
                    }
                    else
                    {
                        series.Points[ptIdx].Label = numVal.ToString("N0");
                    }
                }
            }

            chart.Legends.Clear();
            return chart;
        }

        // ==========================================
        // PAGE NAVIGATION ACTIONS
        // ==========================================
        private void OpenPos()
        {
            OpenChildPage(new PosForm(session), "Điểm Bán hàng & Thu ngân (POS)", "Tạo hóa đơn bán hàng, chọn bàn, gọi món và thu tiền");
        }

        private void OpenOrders()
        {
            OpenChildPage(new OrderManagementForm(session), "Quản lý Hóa đơn & Đơn hàng", "Theo dõi lịch sử đơn hàng, thanh toán, chuyển bàn, gộp bàn và hủy hóa đơn");
        }

        private void OpenCatalog()
        {
            OpenChildPage(new CatalogForm(), "Quản lý Thực đơn & Món", "Danh sách các loại cà phê, đồ uống, đơn giá niêm yết và trạng thái kinh doanh");
        }

        private void OpenCategories()
        {
            OpenChildPage(new CategoryForm(), "Danh mục Món & Phân loại", "Phân nhóm sản phẩm trong thực đơn (Cà phê, Trà, Bánh ngọt, Sinh tố...)");
        }

        private void OpenEmployees()
        {
            OpenChildPage(new EmployeeForm(), "Quản lý Nhân sự & Nhân viên", "Danh sách nhân viên cửa hàng, chức vụ chuyên môn và thông tin liên lạc");
        }

        private void OpenReports()
        {
            OpenChildPage(new ReportForm(), "Báo cáo Thống kê & Doanh thu", "Báo cáo doanh số bán hàng theo ngày, tháng, năm, xuất Excel và in phiếu");
        }

        private void OpenAuditLog()
        {
            OpenChildPage(new AuditLogForm(), "Nhật ký Hoạt động Hệ thống", "Theo dõi lịch sử truy cập và các thao tác thay đổi dữ liệu của người dùng");
        }

        private void OpenAccounts()
        {
            OpenChildPage(new UserManagementForm(session), "Tài khoản & Phân quyền", "Cấp tài khoản đăng nhập và phân quyền truy cập cho nhân viên");
        }

        private void OpenTables()
        {
            OpenChildPage(new TableManagementForm(), "Quản lý Sơ đồ Bàn & Khu vực", "Theo dõi tình trạng bàn trống, bàn có khách, đặt bàn và phân chia khu vực");
        }

        private void OpenInventory()
        {
            OpenChildPage(new InventoryForm(session), "Quản lý Kho Nguyên liệu", "Theo dõi lượng tồn kho, cảnh báo hết hàng, nhập kho và xuất kho");
        }

        private void OpenPromotions()
        {
            OpenChildPage(new PromotionForm(), "Chương trình Khuyến mãi & Giảm giá", "Thiết lập chiết khấu ưu đãi %, thời gian áp dụng chương trình khuyến mãi");
        }
    }
}
