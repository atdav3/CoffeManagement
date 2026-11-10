namespace CoffeeManagement.GUI.Forms
{
    partial class TableManagementForm
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
            this.lblTitleNote = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.statContainer = new System.Windows.Forms.Panel();
            this.flowCards = new System.Windows.Forms.FlowLayoutPanel();
            this.gridContainer = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.topBar.SuspendLayout();
            this.statContainer.SuspendLayout();
            this.gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.Controls.Add(this.btnDeactivate);
            this.topBar.Controls.Add(this.btnReserve);
            this.topBar.Controls.Add(this.btnEdit);
            this.topBar.Controls.Add(this.btnAdd);
            this.topBar.Controls.Add(this.lblTitleNote);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.topBar.Size = new System.Drawing.Size(950, 72);
            this.topBar.TabIndex = 0;
            // 
            // lblTitleNote
            // 
            this.lblTitleNote.AutoSize = true;
            this.lblTitleNote.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblTitleNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblTitleNote.Location = new System.Drawing.Point(20, 26);
            this.lblTitleNote.Name = "lblTitleNote";
            this.lblTitleNote.Size = new System.Drawing.Size(378, 17);
            this.lblTitleNote.TabIndex = 0;
            this.lblTitleNote.Text = "🪑 Quản lý danh sách bàn, trạng thái phục vụ & phân khu vực";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(380, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(150, 40);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "➕ Thêm bàn mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(530, 16);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "✏️ Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnReserve
            // 
            this.btnReserve.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.btnReserve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReserve.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReserve.FlatAppearance.BorderSize = 0;
            this.btnReserve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReserve.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnReserve.ForeColor = System.Drawing.Color.White;
            this.btnReserve.Location = new System.Drawing.Point(650, 16);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(170, 40);
            this.btnReserve.TabIndex = 3;
            this.btnReserve.Text = "🟡 Đặt bàn / Hủy đặt";
            this.btnReserve.UseVisualStyleBackColor = false;
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnDeactivate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeactivate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeactivate.FlatAppearance.BorderSize = 0;
            this.btnDeactivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeactivate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnDeactivate.ForeColor = System.Drawing.Color.White;
            this.btnDeactivate.Location = new System.Drawing.Point(820, 16);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(110, 40);
            this.btnDeactivate.TabIndex = 4;
            this.btnDeactivate.Text = "🔒 Ngừng dùng";
            this.btnDeactivate.UseVisualStyleBackColor = false;
            // 
            // statContainer
            // 
            this.statContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.statContainer.Controls.Add(this.flowCards);
            this.statContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.statContainer.Location = new System.Drawing.Point(0, 72);
            this.statContainer.Name = "statContainer";
            this.statContainer.Padding = new System.Windows.Forms.Padding(20, 12, 20, 0);
            this.statContainer.Size = new System.Drawing.Size(950, 110);
            this.statContainer.TabIndex = 1;
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Location = new System.Drawing.Point(20, 12);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(910, 98);
            this.flowCards.TabIndex = 0;
            this.flowCards.WrapContents = false;
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 182);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20, 10, 20, 20);
            this.gridContainer.Size = new System.Drawing.Size(950, 368);
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
            this.grid.Location = new System.Drawing.Point(20, 10);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.Height = 40;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(910, 338);
            this.grid.TabIndex = 0;
            // 
            // TableManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(950, 550);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.statContainer);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(850, 550);
            this.Name = "TableManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý Sơ đồ Bàn & Khu vực";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.statContainer.ResumeLayout(false);
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitleNote;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Panel statContainer;
        private System.Windows.Forms.FlowLayoutPanel flowCards;
        private System.Windows.Forms.Panel gridContainer;
        private System.Windows.Forms.DataGridView grid;
    }
}
