using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace BT6
{
    partial class UserForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblAlert;
        private Label lblLastCheck;
        private Label lblAlertTime;

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblAlert = new Label();
            lblLastCheck = new Label();
            lblAlertTime = new Label();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(169, 65);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(543, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "HỆ THỐNG CẢNH BÁO NHIỆT ĐỘ HÀ NỘI";
            // 
            // lblAlert
            // 
            lblAlert.BackColor = Color.Black;
            lblAlert.BorderStyle = BorderStyle.FixedSingle;
            lblAlert.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblAlert.ForeColor = Color.White;
            lblAlert.Location = new Point(128, 148);
            lblAlert.Name = "lblAlert";
            lblAlert.Padding = new Padding(10);
            lblAlert.Size = new Size(606, 270);
            lblAlert.TabIndex = 1;
            lblAlert.Text = "Đang tải dữ liệu...";
            lblAlert.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLastCheck
            // 
            lblLastCheck.AutoSize = true;
            lblLastCheck.Font = new Font("Segoe UI", 9F);
            lblLastCheck.ForeColor = Color.Black;
            lblLastCheck.Location = new Point(68, 516);
            lblLastCheck.Name = "lblLastCheck";
            lblLastCheck.Size = new Size(147, 20);
            lblLastCheck.TabIndex = 3;
            lblLastCheck.Text = "Lần kiểm tra cuối: ---";
            // 
            // lblAlertTime
            // 
            lblAlertTime.AutoSize = true;
            lblAlertTime.Font = new Font("Segoe UI", 10F);
            lblAlertTime.ForeColor = Color.Black;
            lblAlertTime.Location = new Point(68, 467);
            lblAlertTime.Name = "lblAlertTime";
            lblAlertTime.Size = new Size(187, 23);
            lblAlertTime.TabIndex = 2;
            lblAlertTime.Text = "Thời gian cảnh báo: ---";
            // 
            // UserForm
            // 
            BackColor = Color.LightPink;
            ClientSize = new Size(872, 619);
            Controls.Add(lblTitle);
            Controls.Add(lblAlert);
            Controls.Add(lblAlertTime);
            Controls.Add(lblLastCheck);
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "UserForm";
            Text = "Cảnh báo Thời tiết";
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