using BT6;
using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT6
{

    public partial class UserForm : Form
    {
        private struct WeatherData
        {
            public double Temperature;
            public DateTime UpdateTime;
        }

        private const string DB_FILE = "weather_alert.db";
        private readonly System.Windows.Forms.Timer pollingTimer = new System.Windows.Forms.Timer();
        private double lastKnownTemp = double.NaN;
        private DateTime lastKnownUpdateTime = DateTime.MinValue;
        private Color lastKnownColor = Color.Empty;

        public UserForm()
        {
            InitializeComponent();
            this.Text = "App 2 (User) - Cảnh báo Thời tiết";
            pollingTimer.Interval = 1000;
            pollingTimer.Tick += PollingTimer_Tick;
            this.Load += UserForm_Load;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Program.AdminReadySignal.WaitOne();
                if (this.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pollingTimer.Start();
                    });
                }
            });
        }

        private async void PollingTimer_Tick(object sender, EventArgs e)
        {
            await CheckForUpdatesAsync();
            lblLastCheck.Text = $"Lần kiểm tra cuối: {DateTime.Now:HH:mm:ss}";
        }

        private async Task CheckForUpdatesAsync()
        {
            pollingTimer.Stop();

            WeatherData data = GetLatestWeather();
            double currentTemp = data.Temperature;
            DateTime currentUpdateTime = data.UpdateTime;

            if (double.IsNaN(currentTemp))
            {
                lblAlert.Text = "Chưa có dữ liệu Admin.";
                lblAlert.BackColor = Color.Black;
                lblAlert.ForeColor = Color.White;

                pollingTimer.Start();
                return;
            }

            if (currentUpdateTime > lastKnownUpdateTime)
            {
                // 1. Tính toán Trạng thái Cảnh báo hiện tại
                string alertMessage;
                Color alertColor;
                bool isExtreme = false;

                if (currentTemp >= 35)
                {
                    alertMessage = $"🔥 CẢNH BÁO NÓNG GAY GẮT: {currentTemp:0.0}°C";
                    alertColor = Color.FromArgb(220, 50, 50);
                    isExtreme = true;
                }
                else if (currentTemp <= 15)
                {
                    alertMessage = $"❄️ CẢNH BÁO RÉT: {currentTemp:0.0}°C";
                    alertColor = Color.FromArgb(50, 100, 220);
                    isExtreme = true;
                }
                else
                {
                    // Trạng thái An toàn
                    alertMessage = $"Nhiệt độ hiện tại: {currentTemp:0.0}°C";
                    alertColor = Color.Black;
                }


                bool stateChanged = lastKnownColor != Color.Empty && alertColor.ToArgb() != lastKnownColor.ToArgb();

                lastKnownTemp = currentTemp;
                lastKnownUpdateTime = currentUpdateTime;

                if (stateChanged || lastKnownColor == Color.Empty)
                {
                    if (stateChanged)
                    {
                        SystemSounds.Exclamation.Play();
                    }
                    lblAlert.BackColor = alertColor;
                    lblAlert.ForeColor = Color.White;
                    lastKnownColor = alertColor;
                }

                lblAlert.Text = alertMessage;
                lblAlertTime.Text = $"Cập nhật lúc: {currentUpdateTime:HH:mm:ss}";
            }
            pollingTimer.Start();
        }

        private WeatherData GetLatestWeather()
        {
            try
            {
                var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
                using var conn = new SqliteConnection(cs);
                conn.Open();
                // SELECT Temperature (index 0) và UpdateTime (index 1)
                using var cmd = new SqliteCommand("SELECT Temperature, UpdateTime FROM CurrentWeather WHERE Id = 1", conn);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    double temp = reader.GetDouble(0);

                    DateTime updateTime = DateTime.MinValue;
                    if (!reader.IsDBNull(1) && DateTime.TryParse(reader.GetString(1), out updateTime))
                    {
                        return new WeatherData { Temperature = temp, UpdateTime = updateTime };
                    }

                    return new WeatherData { Temperature = temp, UpdateTime = DateTime.MinValue };
                }
                return new WeatherData { Temperature = double.NaN, UpdateTime = DateTime.MinValue };
            }
            catch
            {
                // Lỗi DB 
                return new WeatherData { Temperature = double.NaN, UpdateTime = DateTime.MinValue };
            }
        }
    }
}