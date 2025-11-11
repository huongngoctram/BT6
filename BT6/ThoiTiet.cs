using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace BT6
{
    public partial class ThoiTiet : Form
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly Dictionary<string, (double lat, double lon)> CityCoordinates = new()
        {
            {"Hồ Chí Minh", (10.823, 106.629)},
            {"Hà Nội", (21.028, 105.834)},
            {"Đà Nẵng", (16.054, 108.204)}
        };

        public ThoiTiet()
        {
            InitializeComponent();

            // Thiết lập giá trị ban đầu cho các textbox tọa độ
            txtLatitude.Text = "10.00";
            txtLongitude.Text = "105.00";

            // Khởi chạy mặc định sau khi UI đã sẵn sàng
            cmbCity.SelectedIndex = 1; // Chọn Hà Nội
            pnlWeatherDisplay.Visible = false;
        }

        private async void CmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOther = cmbCity.SelectedItem?.ToString().StartsWith("Khác") ?? false;

            // Toggle visibility cho ô nhập tọa độ
            lblLatitude.Visible = isOther;
            txtLatitude.Visible = isOther;
            lblLongitude.Visible = isOther;
            txtLongitude.Visible = isOther;

            // Tự động cập nhật khi thay đổi tỉnh thành
            if (cmbCity.SelectedIndex != -1 && !isOther)
            {
                await BtnUpdate_Logic();
            }
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            await BtnUpdate_Logic();
        }

        private async Task BtnUpdate_Logic()
        {
            btnUpdate.Enabled = false;
            btnUpdate.Text = "Đang tải...";
            pnlWeatherDisplay.Visible = false;

            try
            {
                await GetAndUpdateWeatherAsync();
            }
            catch (Exception ex)
            {
                string errorMessage;
                if (ex is HttpRequestException httpEx && httpEx.StatusCode.HasValue)
                {
                    errorMessage = $"Lỗi HTTP {httpEx.StatusCode.Value}";
                }
                else if (ex is InvalidOperationException)
                {
                    errorMessage = ex.Message;
                }
                else
                {
                    errorMessage = "Lỗi hệ thống: Vui lòng kiểm tra kết nối.";
                    Console.WriteLine(ex.ToString()); // Log lỗi chi tiết
                }

                // Cập nhật UI hiển thị lỗi
                lblTime.Text = errorMessage;
                lblIcon.Text = "❌";
                lblTemp.Text = "---";
                lblWind.Text = "---";
                lblIcon.ForeColor = Color.Red;
                pnlWeatherDisplay.Visible = true;
            }
            finally
            {
                updateTimer.Start(); 
            }
        }

        private async Task GetAndUpdateWeatherAsync()
        {
            double lat, lon;
            string selectedCity = cmbCity.SelectedItem?.ToString() ?? throw new InvalidOperationException("Vui lòng chọn một thành phố.");

            if (selectedCity.StartsWith("Khác"))
            {
                if (!double.TryParse(txtLatitude.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out lat) ||
                    !double.TryParse(txtLongitude.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out lon))
                {
                    throw new InvalidOperationException("Vui lòng nhập tọa độ Kinh độ/Vĩ độ hợp lệ (sử dụng dấu chấm thập phân).");
                }
            }
            else
            {
                (lat, lon) = CityCoordinates[selectedCity];
            }

            string latStr = lat.ToString(CultureInfo.InvariantCulture);
            string lonStr = lon.ToString(CultureInfo.InvariantCulture);

            // GỌI API:
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current_weather=true&timezone=Asia/Ho_Chi_Minh";

            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            string result = await res.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(result);

            double temp = data.current_weather.temperature;
            double wind = data.current_weather.windspeed;
            int code = data.current_weather.weathercode;
            string time = data.current_weather.time;

            ApplyWeatherLogic(temp, wind, code, time);
        }

        private void ApplyWeatherLogic(double temp, double wind, int code, string time)
        {
            string icon = "🌤️"; // Icon mặc định: Nắng nhẹ/Ít mây
            Color tempColor = Color.FromArgb(150, 255, 150); // Mặc định: Xanh lá

            // Lấy mã thời tiết để xác định Mưa/Tuyết/Dông
            bool isRainyOrSnowy = (code >= 51 && code <= 67) || (code >= 71 && code <= 86) || (code >= 95);

            // ************ LOGIC ĐƠN GIẢN HÓA ************

            if (temp > 35)
            {
                // Yêu cầu 1: Nhiệt độ > 35°C => Đỏ + Nắng/Lửa
                tempColor = Color.FromArgb(255, 100, 100); // Đỏ sáng
                icon = "☀️🔥"; // Nắng kèm Lửa (Icon Nắng sẽ được ưu tiên)
            }
            else if (isRainyOrSnowy)
            {
                // Yêu cầu 2: Nếu trời Mưa => Icon Mưa
                icon = "🌧️";
            }

            // Xử lý icon Gió mạnh (Không ưu tiên icon Gió lên trên Mưa/Nắng)
            if (wind >= 20)
            {
                // Yêu cầu 3: Gió mạnh (> 20 km/h) => Cảnh báo Gió
                lblWind.ForeColor = Color.FromArgb(255, 165, 0); // Cam (Cảnh báo gió mạnh)
                icon += " 🌬️"; // Thêm biểu tượng gió vào cạnh icon chính
            }
            else
            {
                lblWind.ForeColor = Color.White;
            }

            // Cập nhật UI
            lblIcon.Text = icon;
            lblTemp.Text = $"{temp:0.0}°C";
            lblTemp.ForeColor = tempColor; // Màu nhiệt độ theo logic >35
            lblWind.Text = $"{wind:0.0} km/h";

            // Thời gian đã được API trả về theo múi giờ Việt Nam, chỉ cần định dạng
            lblTime.Text = DateTime.Parse(time).ToString("HH:mm dd/MM/yyyy");

            // Hiệu ứng "Fade In" được xử lý bằng cách hiện Panel ở cuối Logic
            pnlWeatherDisplay.Visible = true;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Logic cho hiệu ứng "Fade In" (reset nút)
            updateTimer.Stop();
            btnUpdate.Text = "🔄 Cập nhật dữ liệu";
            btnUpdate.Enabled = true;
        }

        private void lblIcon_Click(object sender, EventArgs e)
        {

        }
    }
}