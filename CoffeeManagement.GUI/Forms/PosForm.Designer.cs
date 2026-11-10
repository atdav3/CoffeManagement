using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Forms
{
    partial class PosForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel top;
        private TextBox search;
        private ComboBox table;
        private ComboBox promotion;
        private Button find;
        private Button clearCart;
        private SplitContainer split;
        private Panel productPanel;
        private Label prodHeader;
        private DataGridView products;
        private Panel prodActions;
        private Button addBtn;
        private Panel cartPanel;
        private Label cartHeader;
        private DataGridView cartGrid;
        private FlowLayoutPanel cartActions;
        private Button plusBtn;
        private Button minusBtn;
        private Button remove;
        private Panel footer;
        private Label subtotalLabel;
        private Label discountLabel;
        private Label totalLabel;
        private Button pay;

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
            this.top = new System.Windows.Forms.Panel();
            this.clearCart = new System.Windows.Forms.Button();
            this.find = new System.Windows.Forms.Button();
            this.promotion = new System.Windows.Forms.ComboBox();
            this.table = new System.Windows.Forms.ComboBox();
            this.search = new System.Windows.Forms.TextBox();
            this.split = new System.Windows.Forms.SplitContainer();
            this.productPanel = new System.Windows.Forms.Panel();
            this.products = new System.Windows.Forms.DataGridView();
            this.prodActions = new System.Windows.Forms.Panel();
            this.addBtn = new System.Windows.Forms.Button();
            this.prodHeader = new System.Windows.Forms.Label();
            this.cartPanel = new System.Windows.Forms.Panel();
            this.cartGrid = new System.Windows.Forms.DataGridView();
            this.cartActions = new System.Windows.Forms.FlowLayoutPanel();
            this.plusBtn = new System.Windows.Forms.Button();
            this.minusBtn = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.footer = new System.Windows.Forms.Panel();
            this.pay = new System.Windows.Forms.Button();
            this.totalLabel = new System.Windows.Forms.Label();
            this.discountLabel = new System.Windows.Forms.Label();
            this.subtotalLabel = new System.Windows.Forms.Label();
            this.cartHeader = new System.Windows.Forms.Label();
            this.top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.productPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products)).BeginInit();
            this.prodActions.SuspendLayout();
            this.cartPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cartGrid)).BeginInit();
            this.cartActions.SuspendLayout();
            this.footer.SuspendLayout();
            this.SuspendLayout();
            // 
            // top
            // 
            this.top.BackColor = System.Drawing.Color.White;
            this.top.Controls.Add(this.clearCart);
            this.top.Controls.Add(this.find);
            this.top.Controls.Add(this.promotion);
            this.top.Controls.Add(this.table);
            this.top.Controls.Add(this.search);
            this.top.Dock = System.Windows.Forms.DockStyle.Top;
            this.top.Location = new System.Drawing.Point(0, 0);
            this.top.Name = "top";
            this.top.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.top.Size = new System.Drawing.Size(1100, 72);
            this.top.TabIndex = 0;
            // 
            // clearCart
            // 
            this.clearCart.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.clearCart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearCart.Dock = System.Windows.Forms.DockStyle.Right;
            this.clearCart.FlatAppearance.BorderSize = 0;
            this.clearCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearCart.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.clearCart.ForeColor = System.Drawing.Color.White;
            this.clearCart.Location = new System.Drawing.Point(970, 16);
            this.clearCart.Name = "clearCart";
            this.clearCart.Size = new System.Drawing.Size(110, 40);
            this.clearCart.TabIndex = 4;
            this.clearCart.Text = "🗑️ Hủy giỏ";
            this.clearCart.UseVisualStyleBackColor = false;
            // 
            // find
            // 
            this.find.BackColor = System.Drawing.Color.White;
            this.find.Cursor = System.Windows.Forms.Cursors.Hand;
            this.find.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.find.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.find.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.find.Location = new System.Drawing.Point(805, 16);
            this.find.Name = "find";
            this.find.Size = new System.Drawing.Size(110, 38);
            this.find.TabIndex = 3;
            this.find.Text = "🔍 Tìm món";
            this.find.UseVisualStyleBackColor = true;
            // 
            // promotion
            // 
            this.promotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.promotion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.promotion.FormattingEnabled = true;
            this.promotion.Location = new System.Drawing.Point(530, 18);
            this.promotion.Name = "promotion";
            this.promotion.Size = new System.Drawing.Size(260, 25);
            this.promotion.TabIndex = 2;
            // 
            // table
            // 
            this.table.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.table.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.table.FormattingEnabled = true;
            this.table.Location = new System.Drawing.Point(295, 18);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(220, 25);
            this.table.TabIndex = 1;
            // 
            // search
            // 
            this.search.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.search.Location = new System.Drawing.Point(20, 18);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(260, 25);
            this.search.TabIndex = 0;
            // 
            // split
            // 
            this.split.BackColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 72);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.productPanel);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.cartPanel);
            this.split.Size = new System.Drawing.Size(1100, 608);
            this.split.SplitterDistance = 580;
            this.split.TabIndex = 1;
            // 
            // productPanel
            // 
            this.productPanel.BackColor = System.Drawing.Color.White;
            this.productPanel.Controls.Add(this.products);
            this.productPanel.Controls.Add(this.prodActions);
            this.productPanel.Controls.Add(this.prodHeader);
            this.productPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productPanel.Location = new System.Drawing.Point(0, 0);
            this.productPanel.Name = "productPanel";
            this.productPanel.Padding = new System.Windows.Forms.Padding(16);
            this.productPanel.Size = new System.Drawing.Size(580, 608);
            this.productPanel.TabIndex = 0;
            // 
            // products
            // 
            this.products.BackgroundColor = System.Drawing.Color.White;
            this.products.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.products.Dock = System.Windows.Forms.DockStyle.Fill;
            this.products.Location = new System.Drawing.Point(16, 48);
            this.products.Name = "products";
            this.products.RowHeadersVisible = false;
            this.products.Size = new System.Drawing.Size(548, 496);
            this.products.TabIndex = 1;
            // 
            // prodActions
            // 
            this.prodActions.BackColor = System.Drawing.Color.White;
            this.prodActions.Controls.Add(this.addBtn);
            this.prodActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prodActions.Location = new System.Drawing.Point(16, 544);
            this.prodActions.Name = "prodActions";
            this.prodActions.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.prodActions.Size = new System.Drawing.Size(548, 48);
            this.prodActions.TabIndex = 2;
            // 
            // addBtn
            // 
            this.addBtn.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.addBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addBtn.FlatAppearance.BorderSize = 0;
            this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.addBtn.ForeColor = System.Drawing.Color.White;
            this.addBtn.Location = new System.Drawing.Point(0, 10);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(548, 38);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "➕ Thêm vào giỏ hàng";
            this.addBtn.UseVisualStyleBackColor = false;
            // 
            // prodHeader
            // 
            this.prodHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.prodHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.prodHeader.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.prodHeader.Location = new System.Drawing.Point(16, 16);
            this.prodHeader.Name = "prodHeader";
            this.prodHeader.Size = new System.Drawing.Size(548, 32);
            this.prodHeader.TabIndex = 0;
            this.prodHeader.Text = "📋 THỰC ĐƠN MÓN (Nhấp đúp chuột để thêm nhanh vào giỏ)";
            // 
            // cartPanel
            // 
            this.cartPanel.BackColor = System.Drawing.Color.White;
            this.cartPanel.Controls.Add(this.cartGrid);
            this.cartPanel.Controls.Add(this.cartActions);
            this.cartPanel.Controls.Add(this.footer);
            this.cartPanel.Controls.Add(this.cartHeader);
            this.cartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartPanel.Location = new System.Drawing.Point(0, 0);
            this.cartPanel.Name = "cartPanel";
            this.cartPanel.Padding = new System.Windows.Forms.Padding(16);
            this.cartPanel.Size = new System.Drawing.Size(516, 608);
            this.cartPanel.TabIndex = 0;
            // 
            // cartGrid
            // 
            this.cartGrid.BackgroundColor = System.Drawing.Color.White;
            this.cartGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cartGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cartGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartGrid.Location = new System.Drawing.Point(16, 48);
            this.cartGrid.Name = "cartGrid";
            this.cartGrid.RowHeadersVisible = false;
            this.cartGrid.Size = new System.Drawing.Size(484, 348);
            this.cartGrid.TabIndex = 1;
            // 
            // cartActions
            // 
            this.cartActions.BackColor = System.Drawing.Color.White;
            this.cartActions.Controls.Add(this.plusBtn);
            this.cartActions.Controls.Add(this.minusBtn);
            this.cartActions.Controls.Add(this.remove);
            this.cartActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cartActions.Location = new System.Drawing.Point(16, 396);
            this.cartActions.Name = "cartActions";
            this.cartActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.cartActions.Size = new System.Drawing.Size(484, 46);
            this.cartActions.TabIndex = 2;
            // 
            // plusBtn
            // 
            this.plusBtn.BackColor = System.Drawing.Color.White;
            this.plusBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plusBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.plusBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.plusBtn.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.plusBtn.Location = new System.Drawing.Point(0, 6);
            this.plusBtn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.plusBtn.Name = "plusBtn";
            this.plusBtn.Size = new System.Drawing.Size(115, 34);
            this.plusBtn.TabIndex = 0;
            this.plusBtn.Text = "➕ Tăng (+1)";
            this.plusBtn.UseVisualStyleBackColor = true;
            // 
            // minusBtn
            // 
            this.minusBtn.BackColor = System.Drawing.Color.White;
            this.minusBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minusBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.minusBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusBtn.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.minusBtn.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.minusBtn.Location = new System.Drawing.Point(123, 6);
            this.minusBtn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.minusBtn.Name = "minusBtn";
            this.minusBtn.Size = new System.Drawing.Size(115, 34);
            this.minusBtn.TabIndex = 1;
            this.minusBtn.Text = "➖ Giảm (-1)";
            this.minusBtn.UseVisualStyleBackColor = true;
            // 
            // remove
            // 
            this.remove.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.remove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.remove.FlatAppearance.BorderSize = 0;
            this.remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.remove.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.remove.ForeColor = System.Drawing.Color.White;
            this.remove.Location = new System.Drawing.Point(246, 6);
            this.remove.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(115, 34);
            this.remove.TabIndex = 2;
            this.remove.Text = "🗑️ Xóa món";
            this.remove.UseVisualStyleBackColor = false;
            // 
            // footer
            // 
            this.footer.BackColor = System.Drawing.Color.FromArgb(243, 237, 230);
            this.footer.Controls.Add(this.pay);
            this.footer.Controls.Add(this.totalLabel);
            this.footer.Controls.Add(this.discountLabel);
            this.footer.Controls.Add(this.subtotalLabel);
            this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer.Location = new System.Drawing.Point(16, 442);
            this.footer.Name = "footer";
            this.footer.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.footer.Size = new System.Drawing.Size(484, 150);
            this.footer.TabIndex = 3;
            // 
            // pay
            // 
            this.pay.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.pay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pay.FlatAppearance.BorderSize = 0;
            this.pay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pay.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.pay.ForeColor = System.Drawing.Color.White;
            this.pay.Location = new System.Drawing.Point(16, 94);
            this.pay.Name = "pay";
            this.pay.Size = new System.Drawing.Size(452, 44);
            this.pay.TabIndex = 3;
            this.pay.Text = "💳 THANH TOÁN & IN HÓA ĐƠN";
            this.pay.UseVisualStyleBackColor = false;
            // 
            // totalLabel
            // 
            this.totalLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.totalLabel.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.totalLabel.Location = new System.Drawing.Point(16, 56);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(380, 36);
            this.totalLabel.TabIndex = 2;
            this.totalLabel.Text = "TỔNG:  0 ₫";
            // 
            // discountLabel
            // 
            this.discountLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.discountLabel.ForeColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.discountLabel.Location = new System.Drawing.Point(16, 34);
            this.discountLabel.Name = "discountLabel";
            this.discountLabel.Size = new System.Drawing.Size(280, 22);
            this.discountLabel.TabIndex = 1;
            this.discountLabel.Text = "Giảm giá:  0 ₫ (0%)";
            // 
            // subtotalLabel
            // 
            this.subtotalLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.subtotalLabel.ForeColor = System.Drawing.Color.FromArgb(120, 110, 100);
            this.subtotalLabel.Location = new System.Drawing.Point(16, 12);
            this.subtotalLabel.Name = "subtotalLabel";
            this.subtotalLabel.Size = new System.Drawing.Size(280, 22);
            this.subtotalLabel.TabIndex = 0;
            this.subtotalLabel.Text = "Tạm tính:  0 ₫";
            // 
            // cartHeader
            // 
            this.cartHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.cartHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cartHeader.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.cartHeader.Location = new System.Drawing.Point(16, 16);
            this.cartHeader.Name = "cartHeader";
            this.cartHeader.Size = new System.Drawing.Size(484, 32);
            this.cartHeader.TabIndex = 0;
            this.cartHeader.Text = "🛒 CHI TIẾT GIỎ HÀNG & HÓA ĐƠN";
            // 
            // PosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 246, 242);
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.split);
            this.Controls.Add(this.top);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1050, 650);
            this.Name = "PosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Điểm Bán hàng & Thu ngân (POS)";
            this.top.ResumeLayout(false);
            this.top.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.productPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.products)).EndInit();
            this.prodActions.ResumeLayout(false);
            this.cartPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cartGrid)).EndInit();
            this.cartActions.ResumeLayout(false);
            this.footer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
