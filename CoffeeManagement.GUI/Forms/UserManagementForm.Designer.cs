namespace CoffeeManagement.GUI.Forms
{
    partial class UserManagementForm
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
            this.btnToggle = new System.Windows.Forms.Button();
            this.gridContainer = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.topBar.SuspendLayout();
            this.gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.Controls.Add(this.btnToggle);
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
            this.lblTitleNote.Size = new System.Drawing.Size(394, 17);
            this.lblTitleNote.TabIndex = 0;
            this.lblTitleNote.Text = "🔐 Quản lý quyền truy cập và tài khoản người dùng hệ thống";
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
            this.btnAdd.Location = new System.Drawing.Point(590, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(180, 40);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "➕ Tạo tài khoản mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnToggle
            // 
            this.btnToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.btnToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnToggle.FlatAppearance.BorderSize = 0;
            this.btnToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnToggle.ForeColor = System.Drawing.Color.White;
            this.btnToggle.Location = new System.Drawing.Point(770, 16);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(160, 40);
            this.btnToggle.TabIndex = 2;
            this.btnToggle.Text = "🔒 Khóa / Kích hoạt";
            this.btnToggle.UseVisualStyleBackColor = false;
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 72);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20);
            this.gridContainer.Size = new System.Drawing.Size(950, 488);
            this.gridContainer.TabIndex = 1;
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
            this.grid.Size = new System.Drawing.Size(910, 448);
            this.grid.TabIndex = 0;
            // 
            // UserManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(950, 560);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(850, 530);
            this.Name = "UserManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tài khoản & Phân quyền Người dùng";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblTitleNote;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.Panel gridContainer;
        private System.Windows.Forms.DataGridView grid;
    }
}
