using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BT6
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblTempLabel;
        private Label lblTemp;
        private Label lblStatus;

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblTempLabel = new Label();
            lblTemp = new Label();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Times New Roman", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(66, 48);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(565, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "APP CẬP NHẬT NHIỆT ĐỘ HÀ NỘI (5s/lần)";
            // 
            // lblTempLabel
            // 
            lblTempLabel.AutoSize = true;
            lblTempLabel.Font = new Font("Times New Roman", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTempLabel.ForeColor = SystemColors.ActiveCaptionText;
            lblTempLabel.Location = new Point(186, 146);
            lblTempLabel.Name = "lblTempLabel";
            lblTempLabel.Size = new Size(143, 39);
            lblTempLabel.TabIndex = 1;
            lblTempLabel.Text = "Nhiệt độ:";
            // 
            // lblTemp
            // 
            lblTemp.AutoSize = true;
            lblTemp.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTemp.ForeColor = Color.Black;
            lblTemp.Location = new Point(383, 129);
            lblTemp.Name = "lblTemp";
            lblTemp.Size = new Size(96, 72);
            lblTemp.TabIndex = 2;
            lblTemp.Text = "---";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblStatus.ForeColor = Color.Brown;
            lblStatus.Location = new Point(298, 231);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(96, 23);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Khởi động...";
            // 
            // AdminForm
            // 
            BackColor = Color.LightPink;
            ClientSize = new Size(700, 300);
            Controls.Add(lblTitle);
            Controls.Add(lblTempLabel);
            Controls.Add(lblTemp);
            Controls.Add(lblStatus);
            ForeColor = Color.IndianRed;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AdminForm";
            Text = "App Admin ";
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}