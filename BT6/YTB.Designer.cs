using Microsoft.Web.WebView2.WinForms;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace BT6
{
    partial class YTB
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private Button btnTrending;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnPlay;
        private Label lblStatus;
        private Label lblLogo;
        private Label lblTitle;
        private SplitContainer splitContainerMain;
        private DataGridView dgvVideos;
        private Panel panelRight;
        private PictureBox picThumbnail;
        private WebView2 webView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panelTop = new Panel();
            lblLogo = new Label();
            lblTitle = new Label();
            btnTrending = new Button();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnPlay = new Button();
            lblStatus = new Label();
            splitContainerMain = new SplitContainer();
            dgvVideos = new DataGridView();
            panelRight = new Panel();
            webView = new WebView2();
            picThumbnail = new PictureBox();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVideos).BeginInit();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(224, 224, 224);
            panelTop.Controls.Add(lblLogo);
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnTrending);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(btnSearch);
            panelTop.Controls.Add(btnPlay);
            panelTop.Controls.Add(lblStatus);
            panelTop.Dock = DockStyle.Top;
            panelTop.ForeColor = Color.White;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12, 10, 12, 10);
            panelTop.Size = new Size(1428, 117);
            panelTop.TabIndex = 1;
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(255, 44, 85);
            lblLogo.Location = new Point(32, 10);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(50, 46);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "▸";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(89, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(203, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "YouTube Việt Nam";
            // 
            // btnTrending
            // 
            btnTrending.BackColor = Color.IndianRed;
            btnTrending.FlatAppearance.BorderSize = 0;
            btnTrending.FlatStyle = FlatStyle.Flat;
            btnTrending.ForeColor = Color.White;
            btnTrending.Location = new Point(1144, 25);
            btnTrending.Name = "btnTrending";
            btnTrending.Size = new Size(150, 40);
            btnTrending.TabIndex = 2;
            btnTrending.Text = "Top trending";
            btnTrending.UseVisualStyleBackColor = false;
            btnTrending.Click += BtnTrending_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(353, 25);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm video...";
            txtSearch.Size = new Size(235, 30);
            txtSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(255, 128, 128);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(624, 25);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 40);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(255, 128, 128);
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(786, 25);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(110, 40);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "Phát";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += BtnPlay_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI Black", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(15, 82);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(88, 25);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Let's Go";
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 117);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.BackColor = Color.FromArgb(245, 247, 250);
            splitContainerMain.Panel1.Controls.Add(dgvVideos);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.BackColor = Color.White;
            splitContainerMain.Panel2.Controls.Add(panelRight);
            splitContainerMain.Size = new Size(1428, 707);
            splitContainerMain.SplitterDistance = 1000;
            splitContainerMain.TabIndex = 0;
            // 
            // dgvVideos
            // 
            dgvVideos.AllowUserToAddRows = false;
            dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVideos.BackgroundColor = Color.White;
            dgvVideos.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvVideos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvVideos.ColumnHeadersHeight = 34;
            dgvVideos.Dock = DockStyle.Fill;
            dgvVideos.EnableHeadersVisualStyles = false;
            dgvVideos.Location = new Point(0, 0);
            dgvVideos.MultiSelect = false;
            dgvVideos.Name = "dgvVideos";
            dgvVideos.ReadOnly = true;
            dgvVideos.RowHeadersWidth = 62;
            dgvVideos.RowTemplate.Height = 52;
            dgvVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVideos.Size = new Size(1000, 707);
            dgvVideos.TabIndex = 0;
            dgvVideos.CellDoubleClick += DgvVideos_CellDoubleClick;
            dgvVideos.SelectionChanged += DgvVideos_SelectionChanged;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.Silver;
            panelRight.Controls.Add(webView);
            panelRight.Controls.Add(picThumbnail);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(0, 0);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(12);
            panelRight.Size = new Size(424, 707);
            panelRight.TabIndex = 0;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(12, 332);
            webView.Name = "webView";
            webView.Size = new Size(400, 363);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // picThumbnail
            // 
            picThumbnail.BackColor = Color.Silver;
            picThumbnail.Dock = DockStyle.Top;
            picThumbnail.Location = new Point(12, 12);
            picThumbnail.Margin = new Padding(0, 0, 0, 10);
            picThumbnail.Name = "picThumbnail";
            picThumbnail.Size = new Size(400, 320);
            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;
            picThumbnail.TabIndex = 1;
            picThumbnail.TabStop = false;
            // 
            // YTB
            // 
            ClientSize = new Size(1428, 824);
            Controls.Add(splitContainerMain);
            Controls.Add(panelTop);
            Name = "YTB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YouTube Trending (VN)";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVideos).EndInit();
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).EndInit();
            ResumeLayout(false);
        }
    }
}
