using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Helpers
{
    internal static class UITheme
    {
        // ==========================================
        // COLOR PALETTE (Espresso & Warm Amber)
        // ==========================================
        public static readonly Color DarkRoast = Color.FromArgb(26, 17, 12);       // Sidebar deep brown
        public static readonly Color DarkRoastActive = Color.FromArgb(46, 30, 22); // Sidebar item active
        public static readonly Color DarkRoastHover = Color.FromArgb(38, 24, 17);  // Sidebar item hover

        public static readonly Color Espresso = Color.FromArgb(88, 53, 33);        // Primary brand brown (#583521)
        public static readonly Color EspressoHover = Color.FromArgb(108, 66, 42);  // Hover state
        public static readonly Color EspressoDark = Color.FromArgb(68, 40, 24);    // Pressed state
        public static readonly Color EspressoLight = Color.FromArgb(246, 240, 235); // Soft tint

        public static readonly Color WarmAmber = Color.FromArgb(217, 119, 6);      // Accent gold/amber (#D97706)
        public static readonly Color WarmAmberHover = Color.FromArgb(180, 83, 9);
        public static readonly Color WarmAmberLight = Color.FromArgb(254, 243, 199);

        public static readonly Color Background = Color.FromArgb(247, 245, 242);   // Clean warm linen (#F7F5F2)
        public static readonly Color CardBg = Color.White;
        public static readonly Color Border = Color.FromArgb(230, 224, 216);       // Crisp subtle border
        public static readonly Color BorderLight = Color.FromArgb(240, 236, 230);

        public static readonly Color TextPrimary = Color.FromArgb(33, 37, 41);
        public static readonly Color TextSecondary = Color.FromArgb(108, 117, 125);
        public static readonly Color TextMuted = Color.FromArgb(156, 163, 175);
        public static readonly Color TextLight = Color.White;

        public static readonly Color Success = Color.FromArgb(22, 163, 74);        // Emerald green
        public static readonly Color SuccessHover = Color.FromArgb(18, 138, 62);
        public static readonly Color SuccessLight = Color.FromArgb(220, 252, 231);
        public static readonly Color SuccessDark = Color.FromArgb(20, 83, 45);

        public static readonly Color Danger = Color.FromArgb(220, 38, 38);         // Crimson red
        public static readonly Color DangerHover = Color.FromArgb(185, 28, 28);
        public static readonly Color DangerLight = Color.FromArgb(254, 226, 226);

        public static readonly Color Warning = Color.FromArgb(234, 88, 12);        // Warm orange
        public static readonly Color WarningHover = Color.FromArgb(194, 65, 12);
        public static readonly Color WarningLight = Color.FromArgb(254, 237, 213);

        public static readonly Color Info = Color.FromArgb(37, 99, 235);           // Royal blue
        public static readonly Color InfoLight = Color.FromArgb(219, 234, 254);

        // ==========================================
        // TYPOGRAPHY
        // ==========================================
        public static readonly Font FontXs = new Font("Segoe UI", 8.5F);
        public static readonly Font FontSmall = new Font("Segoe UI", 9F);
        public static readonly Font FontRegular = new Font("Segoe UI", 9.5F);
        public static readonly Font FontMedium = new Font("Segoe UI", 10F);
        public static readonly Font FontSemibold = new Font("Segoe UI Semibold", 9.5F);
        public static readonly Font FontSemibold10 = new Font("Segoe UI Semibold", 10F);
        public static readonly Font FontSubheader = new Font("Segoe UI Semibold", 11.5F);
        public static readonly Font FontTitle = new Font("Segoe UI Semibold", 13.5F);
        public static readonly Font FontHeading = new Font("Segoe UI Semibold", 16F);
        public static readonly Font FontHeader = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font FontKpi = new Font("Segoe UI", 22F, FontStyle.Bold);

        // ==========================================
        // BUTTON STYLING
        // ==========================================
        public static void ApplyPrimaryButton(Button btn, string text = null, int width = 0)
        {
            if (!string.IsNullOrEmpty(text)) btn.Text = text;
            if (width > 0) btn.Width = width;
            btn.Height = Math.Max(btn.Height, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Espresso;
            btn.ForeColor = TextLight;
            btn.Font = FontSemibold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter -= OnButtonMouseEnter;
            btn.MouseLeave -= OnButtonMouseLeave;
            btn.Tag = Espresso;
            btn.MouseEnter += OnButtonMouseEnter;
            btn.MouseLeave += OnButtonMouseLeave;
        }

        public static void ApplySecondaryButton(Button btn, string text = null, int width = 0)
        {
            if (!string.IsNullOrEmpty(text)) btn.Text = text;
            if (width > 0) btn.Width = width;
            btn.Height = Math.Max(btn.Height, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Border;
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = Color.White;
            btn.ForeColor = TextPrimary;
            btn.Font = FontSemibold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter -= OnWhiteButtonMouseEnter;
            btn.MouseLeave -= OnWhiteButtonMouseLeave;
            btn.MouseEnter += OnWhiteButtonMouseEnter;
            btn.MouseLeave += OnWhiteButtonMouseLeave;
        }

        public static void ApplySuccessButton(Button btn, string text = null, int width = 0)
        {
            if (!string.IsNullOrEmpty(text)) btn.Text = text;
            if (width > 0) btn.Width = width;
            btn.Height = Math.Max(btn.Height, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Success;
            btn.ForeColor = TextLight;
            btn.Font = FontSemibold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter -= OnButtonMouseEnter;
            btn.MouseLeave -= OnButtonMouseLeave;
            btn.Tag = Success;
            btn.MouseEnter += OnButtonMouseEnter;
            btn.MouseLeave += OnButtonMouseLeave;
        }

        public static void ApplyDangerButton(Button btn, string text = null, int width = 0)
        {
            if (!string.IsNullOrEmpty(text)) btn.Text = text;
            if (width > 0) btn.Width = width;
            btn.Height = Math.Max(btn.Height, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Danger;
            btn.ForeColor = TextLight;
            btn.Font = FontSemibold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter -= OnButtonMouseEnter;
            btn.MouseLeave -= OnButtonMouseLeave;
            btn.Tag = Danger;
            btn.MouseEnter += OnButtonMouseEnter;
            btn.MouseLeave += OnButtonMouseLeave;
        }

        public static void ApplyWarningButton(Button btn, string text = null, int width = 0)
        {
            if (!string.IsNullOrEmpty(text)) btn.Text = text;
            if (width > 0) btn.Width = width;
            btn.Height = Math.Max(btn.Height, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = WarmAmber;
            btn.ForeColor = TextLight;
            btn.Font = FontSemibold;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter -= OnButtonMouseEnter;
            btn.MouseLeave -= OnButtonMouseLeave;
            btn.Tag = WarmAmber;
            btn.MouseEnter += OnButtonMouseEnter;
            btn.MouseLeave += OnButtonMouseLeave;
        }

        private static void OnButtonMouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Color baseColor)
            {
                if (baseColor == Espresso) btn.BackColor = EspressoHover;
                else if (baseColor == Success) btn.BackColor = SuccessHover;
                else if (baseColor == Danger) btn.BackColor = DangerHover;
                else if (baseColor == WarmAmber) btn.BackColor = WarmAmberHover;
            }
        }

        private static void OnButtonMouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Color baseColor)
            {
                btn.BackColor = baseColor;
            }
        }

        private static void OnWhiteButtonMouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = EspressoLight;
                btn.FlatAppearance.BorderColor = Espresso;
            }
        }

        private static void OnWhiteButtonMouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.White;
                btn.FlatAppearance.BorderColor = Border;
            }
        }

        // ==========================================
        // DATAGRIDVIEW STYLING
        // ==========================================
        public static void ApplyGridStyle(DataGridView grid)
        {
            grid.BackgroundColor = CardBg;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = BorderLight;

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 44;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 239, 233);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(58, 42, 32);
            grid.ColumnHeadersDefaultCellStyle.Font = FontSemibold;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 12, 0);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 40;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.Font = FontRegular;
            grid.DefaultCellStyle.Padding = new Padding(12, 0, 12, 0);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 224);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 24, 15);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 251, 249);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 235, 224);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 24, 15);

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==========================================
        // KPI CARD COMPONENT
        // ==========================================
        public static Panel CreateCard(string title, string value, string subtitle, Color accentColor, string icon = "")
        {
            Panel card = new Panel
            {
                Width = 240,
                Height = 120,
                Margin = new Padding(0, 0, 16, 12),
                BackColor = Color.White,
                Padding = new Padding(18, 14, 18, 14)
            };

            card.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(Border, 1))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, card.Width - 1, card.Height - 1);
                }
                using (SolidBrush accentBrush = new SolidBrush(accentColor))
                {
                    e.Graphics.FillRectangle(accentBrush, 0, 0, 4, card.Height);
                }
            };

            Label titleLabel = new Label
            {
                Text = (string.IsNullOrEmpty(icon) ? "" : icon + "  ") + title.ToUpperInvariant(),
                AutoSize = true,
                Font = FontXs,
                ForeColor = TextSecondary,
                Location = new Point(16, 14)
            };

            Label valLabel = new Label
            {
                Text = value,
                AutoSize = true,
                Font = FontHeading,
                ForeColor = accentColor,
                Location = new Point(14, 38)
            };

            Label subLabel = new Label
            {
                Text = subtitle,
                AutoSize = true,
                Font = FontXs,
                ForeColor = TextMuted,
                Location = new Point(16, 88)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valLabel);
            card.Controls.Add(subLabel);
            return card;
        }

        // ==========================================
        // DIALOG STYLING
        // ==========================================
        public static void StyleDialog(Form form, string title, int width, int height)
        {
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ClientSize = new Size(width, height);
            form.Font = FontRegular;
            form.BackColor = Background;
        }

        public static Panel CreateDialogHeader(string title, string subtitle = null, int height = 65)
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = height,
                BackColor = Color.White,
                Padding = new Padding(24, 14, 24, 10)
            };

            header.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = FontSubheader,
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(24, 12)
            };
            header.Controls.Add(lblTitle);

            if (!string.IsNullOrEmpty(subtitle))
            {
                Label lblSub = new Label
                {
                    Text = subtitle,
                    Font = FontSmall,
                    ForeColor = TextSecondary,
                    AutoSize = true,
                    Location = new Point(24, 36)
                };
                header.Controls.Add(lblSub);
            }

            return header;
        }

        public static Panel CreateDialogFooter(int height = 60)
        {
            Panel footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = height,
                BackColor = Color.FromArgb(249, 247, 244),
                Padding = new Padding(20, 12, 20, 12)
            };

            footer.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
                }
            };

            return footer;
        }

        // ==========================================
        // CURRENCY & FORMATTING HELPERS
        // ==========================================
        public static string FormatMoney(decimal amount)
        {
            return amount.ToString("N0") + " ₫";
        }
    }
}
