using System.Drawing;
using System.Windows.Forms;

namespace CoffeeManagement.GUI.Forms
{
    partial class ReportForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel topBar;
        private Label lblKind;
        private ComboBox kind;
        private Label lblFrom;
        private DateTimePicker start;
        private Label lblTo;
        private DateTimePicker end;
        private Button runButton;
        private Button export;
        private Button pdf;
        private Button print;
        private Label status;
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
            this.topBar = new System.Windows.Forms.Panel();
            this.lblKind = new System.Windows.Forms.Label();
            this.kind = new System.Windows.Forms.ComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.end = new System.Windows.Forms.DateTimePicker();
            this.runButton = new System.Windows.Forms.Button();
            this.export = new System.Windows.Forms.Button();
            this.pdf = new System.Windows.Forms.Button();
            this.print = new System.Windows.Forms.Button();
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
            this.topBar.Controls.Add(this.print);
            this.topBar.Controls.Add(this.pdf);
            this.topBar.Controls.Add(this.export);
            this.topBar.Controls.Add(this.runButton);
            this.topBar.Controls.Add(this.end);
            this.topBar.Controls.Add(this.lblTo);
            this.topBar.Controls.Add(this.start);
            this.topBar.Controls.Add(this.lblFrom);
            this.topBar.Controls.Add(this.kind);
            this.topBar.Controls.Add(this.lblKind);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.topBar.Size = new System.Drawing.Size(1084, 84);
            this.topBar.TabIndex = 0;
            // 
            // lblKind
            // 
            this.lblKind.AutoSize = true;
            this.lblKind.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblKind.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblKind.Location = new System.Drawing.Point(20, 14);
            this.lblKind.Name = "lblKind";
            this.lblKind.Size = new System.Drawing.Size(95, 17);
            this.lblKind.TabIndex = 0;
            this.lblKind.Text = "Loại báo cáo:";
            // 
            // kind
            // 
            this.kind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kind.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.kind.FormattingEnabled = true;
            this.kind.Location = new System.Drawing.Point(20, 36);
            this.kind.Name = "kind";
            this.kind.Size = new System.Drawing.Size(230, 25);
            this.kind.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblFrom.Location = new System.Drawing.Point(265, 14);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(61, 17);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "Từ ngày:";
            // 
            // start
            // 
            this.start.CustomFormat = "dd/MM/yyyy";
            this.start.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.start.Location = new System.Drawing.Point(265, 36);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(135, 25);
            this.start.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.lblTo.Location = new System.Drawing.Point(415, 14);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(70, 17);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "Đến ngày:";
            // 
            // end
            // 
            this.end.CustomFormat = "dd/MM/yyyy";
            this.end.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end.Location = new System.Drawing.Point(415, 36);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(135, 25);
            this.end.TabIndex = 5;
            // 
            // runButton
            // 
            this.runButton.BackColor = System.Drawing.Color.FromArgb(111, 78, 55);
            this.runButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.runButton.FlatAppearance.BorderSize = 0;
            this.runButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runButton.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.runButton.ForeColor = System.Drawing.Color.White;
            this.runButton.Location = new System.Drawing.Point(565, 34);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(130, 32);
            this.runButton.TabIndex = 6;
            this.runButton.Text = "🔍 Xem báo cáo";
            this.runButton.UseVisualStyleBackColor = false;
            // 
            // export
            // 
            this.export.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.export.Cursor = System.Windows.Forms.Cursors.Hand;
            this.export.Dock = System.Windows.Forms.DockStyle.Right;
            this.export.FlatAppearance.BorderSize = 0;
            this.export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.export.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.export.ForeColor = System.Drawing.Color.White;
            this.export.Location = new System.Drawing.Point(909, 14);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(155, 56);
            this.export.TabIndex = 9;
            this.export.Text = "📥 Xuất Excel / CSV";
            this.export.UseVisualStyleBackColor = false;
            // 
            // pdf
            // 
            this.pdf.BackColor = System.Drawing.Color.White;
            this.pdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pdf.Dock = System.Windows.Forms.DockStyle.Right;
            this.pdf.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.pdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pdf.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.pdf.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.pdf.Location = new System.Drawing.Point(784, 14);
            this.pdf.Name = "pdf";
            this.pdf.Size = new System.Drawing.Size(125, 56);
            this.pdf.TabIndex = 8;
            this.pdf.Text = "📄 Crystal PDF";
            this.pdf.UseVisualStyleBackColor = true;
            // 
            // print
            // 
            this.print.BackColor = System.Drawing.Color.White;
            this.print.Cursor = System.Windows.Forms.Cursors.Hand;
            this.print.Dock = System.Windows.Forms.DockStyle.Right;
            this.print.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(220, 214, 206);
            this.print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.print.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.print.ForeColor = System.Drawing.Color.FromArgb(43, 30, 22);
            this.print.Location = new System.Drawing.Point(674, 14);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(110, 56);
            this.print.TabIndex = 7;
            this.print.Text = "🖨️ In báo cáo";
            this.print.UseVisualStyleBackColor = true;
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.White;
            this.status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.status.ForeColor = System.Drawing.Color.FromArgb(120, 110, 100);
            this.status.Location = new System.Drawing.Point(0, 575);
            this.status.Name = "status";
            this.status.Padding = new System.Windows.Forms.Padding(20, 8, 20, 0);
            this.status.Size = new System.Drawing.Size(1084, 36);
            this.status.TabIndex = 2;
            this.status.Text = "Hệ thống báo cáo truy vấn trực tiếp từ cơ sở dữ liệu SQL Server.";
            // 
            // gridContainer
            // 
            this.gridContainer.BackColor = System.Drawing.Color.FromArgb(248, 246, 242);
            this.gridContainer.Controls.Add(this.grid);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(0, 84);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(20);
            this.gridContainer.Size = new System.Drawing.Size(1084, 491);
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
            this.grid.Size = new System.Drawing.Size(1044, 451);
            this.grid.TabIndex = 0;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 246, 242);
            this.ClientSize = new System.Drawing.Size(1084, 611);
            this.Controls.Add(this.gridContainer);
            this.Controls.Add(this.status);
            this.Controls.Add(this.topBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Báo cáo Thống kê Doanh thu";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.gridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
