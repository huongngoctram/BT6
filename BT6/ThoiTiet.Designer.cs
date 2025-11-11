using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace BT6
{
    partial class ThoiTiet
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbCity;
        private TextBox txtLatitude;
        private TextBox txtLongitude;
        private Label lblLatitude;
        private Label lblLongitude;
        private Button btnUpdate;
        private Label lblIcon;
        private Panel pnlWeatherDisplay;
        private System.Windows.Forms.Timer updateTimer;
        private Label lblCityTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmbCity = new ComboBox();
            lblLatitude = new Label();
            txtLatitude = new TextBox();
            lblLongitude = new Label();
            txtLongitude = new TextBox();
            btnUpdate = new Button();
            pnlWeatherDisplay = new Panel();
            lblIcon = new Label();
            updateTimer = new System.Windows.Forms.Timer(components);
            lblCityTitle = new Label();
            lblTempTitle = new Label();
            lblTemp = new Label();
            lblWindTitle = new Label();
            lblWind = new Label();
            lblTimeTitle = new Label();
            lblTime = new Label();
            pnlWeatherDisplay.SuspendLayout();
            SuspendLayout();
            // 
            // cmbCity
            // 
            cmbCity.BackColor = Color.FromArgb(40, 50, 70);
            cmbCity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCity.ForeColor = Color.White;
            cmbCity.Items.AddRange(new object[] { "Hồ Chí Minh", "Hà Nội", "Đà Nẵng", "Khác (Tọa độ)" });
            cmbCity.Location = new Point(49, 71);
            cmbCity.Name = "cmbCity";
            cmbCity.Size = new Size(205, 31);
            cmbCity.TabIndex = 0;
            cmbCity.SelectedIndexChanged += CmbCity_SelectedIndexChanged;
            // 
            // lblLatitude
            // 
            lblLatitude.AutoSize = true;
            lblLatitude.BackColor = Color.Transparent;
            lblLatitude.ForeColor = Color.Black;
            lblLatitude.Location = new Point(318, 36);
            lblLatitude.Name = "lblLatitude";
            lblLatitude.Size = new Size(73, 23);
            lblLatitude.TabIndex = 1;
            lblLatitude.Text = "Kinh độ:";
            lblLatitude.Visible = false;
            // 
            // txtLatitude
            // 
            txtLatitude.BackColor = Color.White;
            txtLatitude.ForeColor = Color.White;
            txtLatitude.Location = new Point(319, 72);
            txtLatitude.Name = "txtLatitude";
            txtLatitude.Size = new Size(110, 30);
            txtLatitude.TabIndex = 2;
            txtLatitude.Visible = false;
            // 
            // lblLongitude
            // 
            lblLongitude.AutoSize = true;
            lblLongitude.ForeColor = Color.Black;
            lblLongitude.Location = new Point(559, 40);
            lblLongitude.Name = "lblLongitude";
            lblLongitude.Size = new Size(54, 23);
            lblLongitude.TabIndex = 3;
            lblLongitude.Text = "Vĩ độ:";
            lblLongitude.Visible = false;
            // 
            // txtLongitude
            // 
            txtLongitude.BackColor = Color.White;
            txtLongitude.ForeColor = Color.White;
            txtLongitude.Location = new Point(559, 72);
            txtLongitude.Name = "txtLongitude";
            txtLongitude.Size = new Size(109, 30);
            txtLongitude.TabIndex = 4;
            txtLongitude.Visible = false;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.IndianRed;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(318, 134);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(196, 39);
            btnUpdate.TabIndex = 5;
            btnUpdate.Text = "Cập nhật ";
            btnUpdate.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += BtnUpdate_Click;
            // 
            // pnlWeatherDisplay
            // 
            pnlWeatherDisplay.BackColor = Color.IndianRed;
            pnlWeatherDisplay.BorderStyle = BorderStyle.FixedSingle;
            pnlWeatherDisplay.Controls.Add(lblTempTitle);
            pnlWeatherDisplay.Controls.Add(lblTemp);
            pnlWeatherDisplay.Controls.Add(lblWindTitle);
            pnlWeatherDisplay.Controls.Add(lblWind);
            pnlWeatherDisplay.Controls.Add(lblTimeTitle);
            pnlWeatherDisplay.Controls.Add(lblTime);
            pnlWeatherDisplay.Controls.Add(lblIcon);
            pnlWeatherDisplay.Location = new Point(25, 197);
            pnlWeatherDisplay.Name = "pnlWeatherDisplay";
            pnlWeatherDisplay.Size = new Size(736, 365);
            pnlWeatherDisplay.TabIndex = 6;
            // 
            // lblIcon
            // 
            lblIcon.Font = new Font("Arial", 80F, FontStyle.Bold);
            lblIcon.ForeColor = Color.White;
            lblIcon.Location = new Point(25, 33);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(290, 187);
            lblIcon.TabIndex = 0;
            lblIcon.Text = "❓";
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblIcon.Click += lblIcon_Click;
            // 
            // updateTimer
            // 
            updateTimer.Tick += UpdateTimer_Tick;
            // 
            // lblCityTitle
            // 
            lblCityTitle.AutoSize = true;
            lblCityTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCityTitle.ForeColor = SystemColors.ActiveCaptionText;
            lblCityTitle.Location = new Point(49, 19);
            lblCityTitle.Name = "lblCityTitle";
            lblCityTitle.Size = new Size(145, 25);
            lblCityTitle.TabIndex = 0;
            lblCityTitle.Text = "Chọn địa điểm:";
            // 
            // lblTempTitle
            // 
            lblTempTitle.AutoSize = true;
            lblTempTitle.Font = new Font("Segoe UI", 14F);
            lblTempTitle.ForeColor = Color.Black;
            lblTempTitle.Location = new Point(363, 53);
            lblTempTitle.Name = "lblTempTitle";
            lblTempTitle.Size = new Size(113, 32);
            lblTempTitle.TabIndex = 7;
            lblTempTitle.Text = "Nhiệt độ:";
            // 
            // lblTemp
            // 
            lblTemp.AutoSize = true;
            lblTemp.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblTemp.ForeColor = Color.Black;
            lblTemp.Location = new Point(533, 38);
            lblTemp.Name = "lblTemp";
            lblTemp.Size = new Size(84, 62);
            lblTemp.TabIndex = 8;
            lblTemp.Text = "---";
            // 
            // lblWindTitle
            // 
            lblWindTitle.AutoSize = true;
            lblWindTitle.Font = new Font("Segoe UI", 12F);
            lblWindTitle.ForeColor = Color.Black;
            lblWindTitle.Location = new Point(362, 133);
            lblWindTitle.Name = "lblWindTitle";
            lblWindTitle.Size = new Size(112, 28);
            lblWindTitle.TabIndex = 9;
            lblWindTitle.Text = "Tốc độ Gió:";
            // 
            // lblWind
            // 
            lblWind.AutoSize = true;
            lblWind.Font = new Font("Segoe UI", 14F);
            lblWind.ForeColor = Color.Black;
            lblWind.Location = new Point(550, 134);
            lblWind.Name = "lblWind";
            lblWind.Size = new Size(44, 32);
            lblWind.TabIndex = 10;
            lblWind.Text = "---";
            // 
            // lblTimeTitle
            // 
            lblTimeTitle.AutoSize = true;
            lblTimeTitle.Font = new Font("Segoe UI", 10F);
            lblTimeTitle.ForeColor = Color.Black;
            lblTimeTitle.Location = new Point(363, 197);
            lblTimeTitle.Name = "lblTimeTitle";
            lblTimeTitle.Size = new Size(111, 23);
            lblTimeTitle.TabIndex = 11;
            lblTimeTitle.Text = "Cập nhật lúc:";
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 10F);
            lblTime.ForeColor = Color.Black;
            lblTime.Location = new Point(557, 197);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(31, 23);
            lblTime.TabIndex = 12;
            lblTime.Text = "---";
            // 
            // ThoiTiet
            // 
            BackColor = Color.LightPink;
            ClientSize = new Size(795, 453);
            Controls.Add(lblCityTitle);
            Controls.Add(cmbCity);
            Controls.Add(lblLatitude);
            Controls.Add(txtLatitude);
            Controls.Add(lblLongitude);
            Controls.Add(txtLongitude);
            Controls.Add(btnUpdate);
            Controls.Add(pnlWeatherDisplay);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.LightPink;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ThoiTiet";
            Text = "Dự báo thời tiết";
            pnlWeatherDisplay.ResumeLayout(false);
            pnlWeatherDisplay.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTempTitle;
        private Label lblTemp;
        private Label lblWindTitle;
        private Label lblWind;
        private Label lblTimeTitle;
        private Label lblTime;
    }
}