using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.IO;
using System.Threading;

namespace BT6
{
    public partial class AdminForm : Form
    {
        private const string DB_FILE = "weather_alert.db";
        private const string HANOI_LAT = "21.028";
        private const string HANOI_LON = "105.834";
        private readonly HttpClient httpClient = new HttpClient();
        private readonly System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
        private double lastTemp = double.NaN;
        private bool isInitialUpdate = true;
        public AdminForm()
        {
            InitializeComponent();
            EnsureWeatherDatabase();
            this.Text = "App 1 (Admin) - Cập nhật Hà Nội";

            updateTimer.Interval = 5000; // 5 giây
            updateTimer.Tick += UpdateTimer_Tick;
            this.Load += AdminForm_Load;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            updateTimer.Start();
            _ = UpdateWeatherAndDbAsync();
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            await UpdateWeatherAndDbAsync();
        }

        private async Task UpdateWeatherAndDbAsync()
        {
            // B1: Thông báo đang lấy dữ liệu 
            lblStatus.Text = $"Đang lấy dữ liệu ({DateTime.Now:HH:mm:ss})...";
            this.Refresh();

            try
            {
                string url = $"https://api.open-meteo.com/v1/forecast?latitude={HANOI_LAT}&longitude={HANOI_LON}&current_weather=true&timezone=Asia/Ho_Chi_Minh";
                var resp = await httpClient.GetAsync(url);
                resp.EnsureSuccessStatusCode();

                var json = await resp.Content.ReadAsStringAsync();
                var j = JObject.Parse(json);

                double currentTemp = j["current_weather"]?["temperature"]?.Value<double>() ?? double.NaN;

                if (!double.IsNaN(currentTemp))
                {
                    // B2: Cập nhật UI Admin
                    // Cập nhật nhiệt độ và màu sắc
                    lblTemp.Text = $"{currentTemp.ToString("0.0", CultureInfo.InvariantCulture)}°C";
                    lblTemp.ForeColor = GetColorByTemp(currentTemp);
                    lblStatus.Text = $"Đã cập nhật: {DateTime.Now:HH:mm:ss}";
                    lastTemp = currentTemp;

                    // B3: Ép buộc UI vẽ lại lần nữa để đảm bảo nhiệt độ MỚI đã hiển thị.
                    this.Refresh();

                    // B4: LƯU VÀO CSDL (sau khi đã cập nhật UI thành công)
                    SaveCurrentTemp(currentTemp);

                    // B5: Phát tín hiệu cho UserForm (CHỈ LẦN ĐẦU)
                    if (isInitialUpdate)
                    {
                        Program.AdminReadySignal.Set();
                        isInitialUpdate = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Lỗi API/DB: {ex.Message.Substring(0, Math.Min(ex.Message.Length, 30))}...";
                this.Refresh();
            }
        }
        private Color GetColorByTemp(double temp)
        {
            if (temp >= 35) return Color.FromArgb(255, 100, 100);
            if (temp <= 15) return Color.FromArgb(100, 150, 255);
            return Color.Black;
        }
        private void EnsureWeatherDatabase()
        {
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            string sql = @"
            CREATE TABLE IF NOT EXISTS CurrentWeather (
                Id INTEGER PRIMARY KEY,
                Temperature REAL,
                UpdateTime TEXT
            );";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private void SaveCurrentTemp(double temp)
        {
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            string updateTime = DateTime.Now.ToString("o");
            string sql = @"INSERT OR REPLACE INTO CurrentWeather (Id, Temperature, UpdateTime) VALUES (1, @temp, @time)";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@temp", temp.ToString(CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@time", updateTime); // Lưu thời gian cập nhật
            cmd.ExecuteNonQuery();
        }
    }
}