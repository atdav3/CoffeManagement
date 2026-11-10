namespace CoffeeManagement.GUI.Forms
{
    partial class InventoryForm
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
            this.topBar = new System.Windows.Forms.Panel();
            this.flowActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUnits = new System.Windows.Forms.Button();
            this.btnReceipt = new System.Windows.Forms.Button();
            this.btnIssue = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.loadStatus = new System.Windows.Forms.Label();
            this.gridContainer = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.topBar.SuspendLayout();
            this.flowActions.SuspendLayout();
            this.gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.Controls.Add(this.flowActions);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.topBar.Size = new System.Drawing.Size(1000, 72);
            this.topBar.TabIndex = 0;
            // 
            // flowActions
            // 
            this.flowActions.AutoScroll = true;
            this.flowActions.Controls.Add(this.btnAdd);
            this.flowActions.Controls.Add(this.btnEdit);
            this.flowActions.Controls.Add(this.btnUnits);
            this.flowActions.Controls.Add(this.btnReceipt);
            this.flowActions.Controls.Add(this.btnIssue);
            this.flowActions.Controls.Add(this.btnHistory);
            this.flowActions.Controls.Add(this.btnRefresh);
            this.flowActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowActions.Location = new System.Drawing.Point(20, 16);
            this.flowActions.Name = "flowActions";
            this.flowActions.Size = new System.Drawing.Size(960, 40);
            this.flowActions.TabIndex = 0;
            this.flowActions.WrapContents = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(3, 1);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(160, 38);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "➕ Thêm nguyên liệu";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(169, 1);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 38);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnUnits
            // 
            this.btnUnits.BackColor = System.Drawing.Color.White;
            this.btnUnits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUnits.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(224)))), ((int)(((byte)(216)))));
            this.btnUnits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnits.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnUnits.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnUnits.Location = new System.Drawing.Point(295, 1);
            this.btnUnits.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnUnits.Name = "btnUnits";
            this.btnUnits.Size = new System.Drawing.Size(130, 38);
            this.btnUnits.TabIndex = 2;
            this.btnUnits.Text = "📏 Đơn vị tính";
            this.btnUnits.UseVisualStyleBackColor = false;
            // 
            // btnReceipt
            // 
            this.btnReceipt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.btnReceipt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceipt.FlatAppearance.BorderSize = 0;
            this.btnReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReceipt.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnReceipt.ForeColor = System.Drawing.Color.White;
            this.btnReceipt.Location = new System.Drawing.Point(431, 1);
            this.btnReceipt.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnReceipt.Name = "btnReceipt";
            this.btnReceipt.Size = new System.Drawing.Size(135, 38);
            this.btnReceipt.TabIndex = 3;
            this.btnReceipt.Text = "📥 Nhập kho (+)";
            this.btnReceipt.UseVisualStyleBackColor = false;
            // 
            // btnIssue
            // 
            this.btnIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.btnIssue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIssue.FlatAppearance.BorderSize = 0;
            this.btnIssue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnIssue.ForeColor = System.Drawing.Color.White;
            this.btnIssue.Location = new System.Drawing.Point(572, 1);
            this.btnIssue.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(135, 38);
            this.btnIssue.TabIndex = 4;
            this.btnIssue.Text = "📤 Xuất kho (-)";
            this.btnIssue.UseVisualStyleBackColor = false;
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = System.Drawing.Color.White;
            this.btnHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(224)))), ((int)(((byte)(216)))));
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnHistory.Location = new System.Drawing.Point(713, 1);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(130, 38);
            this.btnHistory.TabIndex = 5;
            this.btnHistory.Text = "📜 Lịch sử kho";
            this.btnHistory.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(224)))), ((int)(((byte)(216)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnRefresh.Location = new System.Drawing.Point(849, 1);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 38);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "🔄 Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // loadStatus
            // 
            this.loadStatus.BackColor = System.Drawing.Color.White;
            this.loadStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.loadStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.loadStatus.Location = new System.Drawing.Point(0, 564);
            this.loadStatus.Name = "loadStatus";
            this.loadStatus.Padding = new System.Windows.Forms.Padding(20, 8, 20, 0);
            this.loadStatus.Size = new System.Drawing.Size(1000, 36);
            this.loadStatus.TabIndex = 1;
            this.loadStatus.Text = "Đang kết nối hệ thống kho...";
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 72);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20);
            this.gridContainer.Size = new System.Drawing.Size(1000, 492);
            this.gridContainer.TabIndex = 2;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(20, 20);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.Height = 40;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(960, 452);
            this.grid.TabIndex = 0;
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.loadStatus);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "InventoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý Kho Nguyên liệu";
            this.topBar.ResumeLayout(false);
            this.flowActions.ResumeLayout(false);
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.FlowLayoutPanel flowActions;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUnits;
        private System.Windows.Forms.Button btnReceipt;
        private System.Windows.Forms.Button btnIssue;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label loadStatus;
        private System.Windows.Forms.Panel gridContainer;
        private System.Windows.Forms.DataGridView grid;
    }
}
