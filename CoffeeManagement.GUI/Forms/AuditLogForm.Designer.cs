namespace CoffeeManagement.GUI.Forms
{
    partial class AuditLogForm
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
            this.lblFrom = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.end = new System.Windows.Forms.DateTimePicker();
            this.refresh = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.Label();
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
            this.topBar.Controls.Add(this.refresh);
            this.topBar.Controls.Add(this.end);
            this.topBar.Controls.Add(this.lblTo);
            this.topBar.Controls.Add(this.start);
            this.topBar.Controls.Add(this.lblFrom);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.topBar.Size = new System.Drawing.Size(950, 72);
            this.topBar.TabIndex = 0;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblFrom.Location = new System.Drawing.Point(20, 26);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(60, 17);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Từ ngày:";
            // 
            // start
            // 
            this.start.CustomFormat = "dd/MM/yyyy";
            this.start.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.start.Location = new System.Drawing.Point(85, 20);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(140, 25);
            this.start.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblTo.Location = new System.Drawing.Point(240, 26);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(68, 17);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "Đến ngày:";
            // 
            // end
            // 
            this.end.CustomFormat = "dd/MM/yyyy";
            this.end.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end.Location = new System.Drawing.Point(315, 20);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(140, 25);
            this.end.TabIndex = 3;
            // 
            // refresh
            // 
            this.refresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.refresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refresh.FlatAppearance.BorderSize = 0;
            this.refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.refresh.ForeColor = System.Drawing.Color.White;
            this.refresh.Location = new System.Drawing.Point(475, 18);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(190, 38);
            this.refresh.TabIndex = 4;
            this.refresh.Text = "🔄 Tải nhật ký hệ thống";
            this.refresh.UseVisualStyleBackColor = false;
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.White;
            this.status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.status.Location = new System.Drawing.Point(0, 524);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(20, 8, 20, 0);
            this.status.Size = new System.Drawing.Size(950, 36);
            this.status.TabIndex = 1;
            this.status.Text = "Nhật ký ghi nhận lịch sử các thao tác thay đổi dữ liệu từ SQL Server.";
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 72);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20);
            this.gridContainer.Size = new System.Drawing.Size(950, 452);
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
            this.grid.Size = new System.Drawing.Size(910, 412);
            this.grid.TabIndex = 0;
            // 
            // AuditLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(950, 560);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.status);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(950, 560);
            this.Name = "AuditLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nhật ký Hoạt động Hệ thống";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker start;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker end;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Panel gridContainer;
        private System.Windows.Forms.DataGridView grid;
    }
}
