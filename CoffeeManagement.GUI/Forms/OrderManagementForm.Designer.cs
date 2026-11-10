using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Forms
{
    partial class OrderManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel filterPanel;
        private Label lblStart;
        private DateTimePicker start;
        private Label lblEnd;
        private DateTimePicker end;
        private Label lblStatus;
        private ComboBox status;
        private Label lblEmployee;
        private ComboBox employee;
        private Button searchButton;
        private Button details;
        private Button pay;
        private Button transfer;
        private Button merge;
        private Button split;
        private Button cancel;
        private Label loadStatus;
        private Panel gridContainer;
        private DataGridView grid;

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
            this.filterPanel = new System.Windows.Forms.Panel();
            this.lblStart = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.end = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.employee = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.details = new System.Windows.Forms.Button();
            this.pay = new System.Windows.Forms.Button();
            this.transfer = new System.Windows.Forms.Button();
            this.merge = new System.Windows.Forms.Button();
            this.split = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.loadStatus = new System.Windows.Forms.Label();
            this.gridContainer = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.filterPanel.SuspendLayout();
            this.gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // filterPanel
            // 
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.Controls.Add(this.cancel);
            this.filterPanel.Controls.Add(this.split);
            this.filterPanel.Controls.Add(this.merge);
            this.filterPanel.Controls.Add(this.transfer);
            this.filterPanel.Controls.Add(this.pay);
            this.filterPanel.Controls.Add(this.details);
            this.filterPanel.Controls.Add(this.searchButton);
            this.filterPanel.Controls.Add(this.employee);
            this.filterPanel.Controls.Add(this.lblEmployee);
            this.filterPanel.Controls.Add(this.status);
            this.filterPanel.Controls.Add(this.lblStatus);
            this.filterPanel.Controls.Add(this.end);
            this.filterPanel.Controls.Add(this.lblEnd);
            this.filterPanel.Controls.Add(this.start);
            this.filterPanel.Controls.Add(this.lblStart);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(0, 0);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.filterPanel.Size = new System.Drawing.Size(1100, 110);
            this.filterPanel.TabIndex = 0;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStart.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblStart.Location = new System.Drawing.Point(20, 16);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(61, 17);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "Từ ngày:";
            // 
            // start
            // 
            this.start.CustomFormat = "dd/MM/yyyy";
            this.start.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.start.Location = new System.Drawing.Point(85, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(130, 25);
            this.start.TabIndex = 1;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEnd.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblEnd.Location = new System.Drawing.Point(230, 16);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(70, 17);
            this.lblEnd.TabIndex = 2;
            this.lblEnd.Text = "Đến ngày:";
            // 
            // end
            // 
            this.end.CustomFormat = "dd/MM/yyyy";
            this.end.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end.Location = new System.Drawing.Point(305, 12);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(130, 25);
            this.end.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblStatus.Location = new System.Drawing.Point(455, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(75, 17);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // status
            // 
            this.status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.status.FormattingEnabled = true;
            this.status.Items.AddRange(new object[] {
            "Tất cả trạng thái",
            "Đang phục vụ (Open)",
            "Đã thanh toán (Paid)",
            "Đã hủy (Cancelled)"});
            this.status.Location = new System.Drawing.Point(535, 12);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(175, 25);
            this.status.TabIndex = 5;
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEmployee.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblEmployee.Location = new System.Drawing.Point(730, 16);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(75, 17);
            this.lblEmployee.TabIndex = 6;
            this.lblEmployee.Text = "Nhân viên:";
            // 
            // employee
            // 
            this.employee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.employee.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.employee.FormattingEnabled = true;
            this.employee.Location = new System.Drawing.Point(810, 12);
            this.employee.Name = "employee";
            this.employee.Size = new System.Drawing.Size(220, 25);
            this.employee.TabIndex = 7;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(111, 78, 55);
            this.searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(20, 58);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(115, 38);
            this.searchButton.TabIndex = 8;
            this.searchButton.Text = "🔍 Tìm kiếm";
            this.searchButton.UseVisualStyleBackColor = false;
            // 
            // details
            // 
            this.details.BackColor = System.Drawing.Color.White;
            this.details.Cursor = System.Windows.Forms.Cursors.Hand;
            this.details.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.details.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.details.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.details.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.details.Location = new System.Drawing.Point(145, 58);
            this.details.Name = "details";
            this.details.Size = new System.Drawing.Size(130, 38);
            this.details.TabIndex = 9;
            this.details.Text = "👁️ Xem chi tiết";
            this.details.UseVisualStyleBackColor = true;
            // 
            // pay
            // 
            this.pay.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.pay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pay.FlatAppearance.BorderSize = 0;
            this.pay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pay.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.pay.ForeColor = System.Drawing.Color.White;
            this.pay.Location = new System.Drawing.Point(285, 58);
            this.pay.Name = "pay";
            this.pay.Size = new System.Drawing.Size(115, 38);
            this.pay.TabIndex = 10;
            this.pay.Text = "💳 Thu tiền";
            this.pay.UseVisualStyleBackColor = false;
            // 
            // transfer
            // 
            this.transfer.BackColor = System.Drawing.Color.White;
            this.transfer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transfer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.transfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.transfer.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.transfer.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.transfer.Location = new System.Drawing.Point(410, 58);
            this.transfer.Name = "transfer";
            this.transfer.Size = new System.Drawing.Size(125, 38);
            this.transfer.TabIndex = 11;
            this.transfer.Text = "🔄 Chuyển bàn";
            this.transfer.UseVisualStyleBackColor = true;
            // 
            // merge
            // 
            this.merge.BackColor = System.Drawing.Color.White;
            this.merge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.merge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.merge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.merge.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.merge.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.merge.Location = new System.Drawing.Point(545, 58);
            this.merge.Name = "merge";
            this.merge.Size = new System.Drawing.Size(110, 38);
            this.merge.TabIndex = 12;
            this.merge.Text = "🔗 Gộp bàn";
            this.merge.UseVisualStyleBackColor = true;
            // 
            // split
            // 
            this.split.BackColor = System.Drawing.Color.White;
            this.split.Cursor = System.Windows.Forms.Cursors.Hand;
            this.split.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.split.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.split.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.split.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.split.Location = new System.Drawing.Point(665, 58);
            this.split.Name = "split";
            this.split.Size = new System.Drawing.Size(110, 38);
            this.split.TabIndex = 13;
            this.split.Text = "✂️ Tách bàn";
            this.split.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancel.FlatAppearance.BorderSize = 0;
            this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.cancel.ForeColor = System.Drawing.Color.White;
            this.cancel.Location = new System.Drawing.Point(785, 58);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(135, 38);
            this.cancel.TabIndex = 14;
            this.cancel.Text = "❌ Hủy hóa đơn";
            this.cancel.UseVisualStyleBackColor = false;
            // 
            // loadStatus
            // 
            this.loadStatus.BackColor = System.Drawing.Color.White;
            this.loadStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.loadStatus.ForeColor = System.Drawing.Color.FromArgb(120, 110, 100);
            this.loadStatus.Location = new System.Drawing.Point(0, 564);
            this.loadStatus.Name = "loadStatus";
            this.loadStatus.Padding = new System.Windows.Forms.Padding(20, 8, 20, 0);
            this.loadStatus.Size = new System.Drawing.Size(1100, 36);
            this.loadStatus.TabIndex = 2;
            this.loadStatus.Text = "Đang tải danh sách đơn hàng…";
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(248, 246, 242);
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 110);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20);
            this.gridContainer.Size = new System.Drawing.Size(1100, 454);
            this.gridContainer.TabIndex = 1;
            // 
            // grid
            // 
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(20, 20);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.Size = new System.Drawing.Size(1060, 414);
            this.grid.TabIndex = 0;
            // 
            // OrderManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 246, 242);
            this.ClientSize = new System.Drawing.Size(1100, 600);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.loadStatus);
            this.Controls.Add(this.filterPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "OrderManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý Hóa đơn & Đơn hàng";
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
