using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System.Net;

namespace BT6
{
    public partial class YTB : Form
    {
       
        private const string API_KEY = "AIzaSyA-kwo7iID4gp0xySBLclO5URrBzoYbxHQ";
        private const string REGION_CODE = "VN";
        private const int MAX_RESULTS = 30;
        private const string DB_FILE = "trending.db";
        private HttpClient httpClient = new HttpClient();
        private List<YTVideo> videos = new();

        public YTB()
        {
            InitializeComponent();
            InitializeDataGridColumns();
            this.Load += Form1_Load;
            EnsureDatabase();
            this.webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        private void InitializeDataGridColumns()
        {
            dgvVideos.Columns.Clear();
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "Tiêu đề", FillWeight = 40 });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Channel", HeaderText = "Kênh", FillWeight = 25 });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Published", HeaderText = "Ngày đăng", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Views", HeaderText = "Lượt xem", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Likes", HeaderText = "Lượt thích", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Mô tả ngắn", Visible = false });
            dgvVideos.Columns.Add(new DataGridViewTextBoxColumn { Name = "VideoId", HeaderText = "VideoId", Visible = false });
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Đang khởi tạo...";
            try
            {
                await webView.EnsureCoreWebView2Async();
                lblStatus.Text = "Sẵn sàng";
            }
            catch
            {
                lblStatus.Text = "Lỗi WebView2";
            }

            await LoadTrendingAsync();
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
            }
        }


        // ====== Model ======
        private class YTVideo
        {
            public string VideoId { get; set; }
            public string Title { get; set; }
            public string ChannelTitle { get; set; }
            public string ThumbnailUrl { get; set; }
            public DateTime PublishedAt { get; set; }
            public string Description { get; set; }
            public long ViewCount { get; set; }
            public long LikeCount { get; set; }
        }

        // ===== Database Logic (Caching) =====
        private void EnsureDatabase()
        {
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            string sql = @"
CREATE TABLE IF NOT EXISTS TrendingVideos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    VideoId TEXT,
    Title TEXT,
    ChannelTitle TEXT,
    ThumbnailUrl TEXT,
    PublishedAt TEXT,
    Description TEXT,
    ViewCount INTEGER,
    LikeCount INTEGER,
    SavedDate TEXT
);";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private bool HasSavedDate(DateTime date)
        {
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            using var cmd = new SqliteCommand("SELECT COUNT(1) FROM TrendingVideos WHERE SavedDate=@d", conn);
            cmd.Parameters.AddWithValue("@d", date.ToString("yyyy-MM-dd"));
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private List<YTVideo> LoadVideosFromDb(DateTime date)
        {
            var list = new List<YTVideo>();
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            string sql = "SELECT VideoId, Title, ChannelTitle, ThumbnailUrl, PublishedAt, Description, ViewCount, LikeCount FROM TrendingVideos WHERE SavedDate=@d";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@d", date.ToString("yyyy-MM-dd"));
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new YTVideo
                {
                    VideoId = rdr.GetString(0),
                    Title = rdr.GetString(1),
                    ChannelTitle = rdr.GetString(2),
                    ThumbnailUrl = rdr.GetString(3),
                    PublishedAt = DateTime.Parse(rdr.GetString(4)),
                    Description = rdr.GetString(5),
                    ViewCount = rdr.GetInt64(6),
                    LikeCount = rdr.GetInt64(7)
                });
            }
            return list;
        }

        private void SaveVideosToDb(List<YTVideo> vids, DateTime savedDate)
        {
            var cs = new SqliteConnectionStringBuilder { DataSource = DB_FILE }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();

            // Xóa dữ liệu cũ của ngày hôm nay trước khi lưu mới để đảm bảo tính duy nhất
            using (var delCmd = new SqliteCommand("DELETE FROM TrendingVideos WHERE SavedDate=@d", conn))
            {
                delCmd.Parameters.AddWithValue("@d", savedDate.ToString("yyyy-MM-dd"));
                delCmd.ExecuteNonQuery();
            }

            foreach (var v in vids)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO TrendingVideos
(VideoId, Title, ChannelTitle, ThumbnailUrl, PublishedAt, Description, ViewCount, LikeCount, SavedDate)
VALUES (@id, @title, @ch, @thumb, @pub, @desc, @views, @likes, @date)";
                cmd.Parameters.AddWithValue("@id", v.VideoId);
                cmd.Parameters.AddWithValue("@title", v.Title);
                cmd.Parameters.AddWithValue("@ch", v.ChannelTitle);
                cmd.Parameters.AddWithValue("@thumb", v.ThumbnailUrl);
                cmd.Parameters.AddWithValue("@pub", v.PublishedAt.ToString("o"));
                cmd.Parameters.AddWithValue("@desc", v.Description);
                cmd.Parameters.AddWithValue("@views", v.ViewCount);
                cmd.Parameters.AddWithValue("@likes", v.LikeCount);
                cmd.Parameters.AddWithValue("@date", savedDate.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
        }

        // ===== Load Trending (Hoàn thiện Logic Caching và API Safety) =====
        private async Task LoadTrendingAsync()
        {
            // Bắt đầu quá trình tải, hiển thị trạng thái bận
            SetUiBusy("Đang kiểm tra CSDL...");
            dgvVideos.Rows.Clear();
            videos.Clear();
            DateTime today = DateTime.Now.Date;
            string finalStatusMessage = "";

            try
            {
                // 1. Kiểm tra CSDL trước
                if (HasSavedDate(today))
                {
                    videos = LoadVideosFromDb(today);
                    // Lấy thông báo cache
                    finalStatusMessage = $"✅ Đã tải {videos.Count} video từ CSDL (ngày {today.ToShortDateString()}).";
                }
                else
                {
                    // 2. Tải từ API và kiểm tra API Key
                    SetUiBusy("Đang tải dữ liệu mới từ Youtube...");
                    string url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&chart=mostPopular&regionCode={REGION_CODE}&maxResults={MAX_RESULTS}&key={API_KEY}";
                    var resp = await httpClient.GetAsync(url);

                    // KHẮC PHỤC LỖI API: Ném ngoại lệ nếu status code không thành công (403, 400,...)
                    resp.EnsureSuccessStatusCode();

                    var json = await resp.Content.ReadAsStringAsync();
                    var j = JObject.Parse(json);

                    var items = (JArray)j["items"];
                    if (items == null)
                    {
                        finalStatusMessage = "LỖI PARSING: Cấu trúc JSON không hợp lệ.";
                        return;
                    }

                    foreach (var it in items)
                    {
                        var stats = it["statistics"];
                        videos.Add(new YTVideo
                        {
                            VideoId = (string)it["id"],
                            Title = (string)it["snippet"]["title"],
                            ChannelTitle = (string)it["snippet"]["channelTitle"],
                            ThumbnailUrl = (string)it["snippet"]["thumbnails"]["medium"]["url"],
                            PublishedAt = (DateTime)it["snippet"]["publishedAt"],
                            Description = (string)it["snippet"]["description"],
                            ViewCount = stats.Value<long?>("viewCount") ?? 0,
                            LikeCount = stats.Value<long?>("likeCount") ?? 0
                        });
                    }
                    SaveVideosToDb(videos, today);
                    // Lấy thông báo API và đã lưu
                    finalStatusMessage = $"💾 Đã tải {videos.Count} video mới từ Youtube API và lưu vào CSDL.";
                }

                // 3. Hiển thị dữ liệu
                foreach (var v in videos)
                {
                    string shortDesc = v.Description?.Substring(0, Math.Min(120, v.Description.Length)) + (v.Description.Length > 120 ? "..." : "");

                    dgvVideos.Rows.Add(v.Title, v.ChannelTitle, v.PublishedAt.ToShortDateString(),
                        v.ViewCount, v.LikeCount, shortDesc, v.VideoId);
                }
                if (dgvVideos.Rows.Count > 0) dgvVideos.Rows[0].Selected = true;
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi HTTP/API Key
                finalStatusMessage = "❌ LỖI API: " + ex.Message;
                MessageBox.Show($"Lỗi kết nối API:\nKiểm tra API Key và giới hạn sử dụng.\nChi tiết: {ex.Message}", "Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Lỗi CSDL hoặc lỗi tổng quát
                finalStatusMessage = "❗ LỖI: " + ex.Message;
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Gán thông báo cuối cùng và đặt lại UI thành Sẵn sàng
                SetUiReady(finalStatusMessage);
            }
        }

        #region API logic (SearchAsync)
        private async Task SearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) { SetUiReady("Vui lòng nhập từ khóa tìm kiếm."); return; }

            try
            {
                SetUiBusy($"Đang tìm kiếm \"{query}\"...");
                videos.Clear();
                dgvVideos.Rows.Clear();

                // 1. Tìm kiếm (chỉ lấy ID)
                string urlSearch = $"https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&maxResults={MAX_RESULTS}&q={Uri.EscapeDataString(query)}&regionCode={REGION_CODE}&key={API_KEY}";
                var resp = await httpClient.GetAsync(urlSearch);

                // Kiểm tra lỗi API
                resp.EnsureSuccessStatusCode();

                string jsonSearch = await resp.Content.ReadAsStringAsync();
                var jSearch = JObject.Parse(jsonSearch);
                var items = (JArray)jSearch["items"];
                var ids = items.Select(it => (string)it["id"]?["videoId"]).Where(id => !string.IsNullOrEmpty(id)).ToList();
                if (ids.Count == 0) { SetUiReady("Không có kết quả"); return; }

                // 2. Lấy chi tiết Video 
                string idsParam = string.Join(",", ids);
                string urlVideos = $"https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&id={idsParam}&key={API_KEY}";
                var resp2 = await httpClient.GetAsync(urlVideos);
                resp2.EnsureSuccessStatusCode();

                string jsonVideos = await resp2.Content.ReadAsStringAsync();
                var jVideos = JObject.Parse(jsonVideos);
                var items2 = (JArray)jVideos["items"];

                foreach (var it in items2)
                {
                    var stats = it["statistics"];
                    var v = new YTVideo
                    {
                        VideoId = (string)it["id"],
                        Title = (string)it["snippet"]?["title"] ?? "",
                        ChannelTitle = (string)it["snippet"]?["channelTitle"] ?? "",
                        ThumbnailUrl = (string)it["snippet"]?["thumbnails"]?["medium"]?["url"] ?? (string)it["snippet"]?["thumbnails"]?["default"]?["url"],
                        PublishedAt = (DateTime)it["snippet"]?["publishedAt"],
                        Description = (string)it["snippet"]?["description"] ?? "",
                        ViewCount = stats != null && stats["viewCount"] != null ? (long)(stats.Value<long?>("viewCount") ?? 0) : 0,
                        LikeCount = stats != null && stats["likeCount"] != null ? (long)(stats.Value<long?>("likeCount") ?? 0) : 0
                    };
                    videos.Add(v);

                    // Thêm dữ liệu mới vào DataGrid
                    string shortDesc = v.Description.Substring(0, Math.Min(120, v.Description.Length)) + (v.Description.Length > 120 ? "..." : "");
                    dgvVideos.Rows.Add(
                        v.Title,
                        v.ChannelTitle,
                        v.PublishedAt.ToShortDateString(),
                        v.ViewCount,
                        v.LikeCount,
                        shortDesc,
                        v.VideoId
                    );
                }

                SetUiReady($"Kết quả tìm kiếm: {videos.Count}");
                if (dgvVideos.Rows.Count > 0) dgvVideos.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetUiReady("Lỗi");
            }
            finally
            {
                SetUiReady(lblStatus.Text);
            }
        }
        #endregion

        #region UI Helpers & Handlers
        private void SetUiBusy(string text)
        {
            lblStatus.Text = text;
            btnTrending.Enabled = btnSearch.Enabled = btnPlay.Enabled = false;
            Cursor = Cursors.WaitCursor;
        }

        private void SetUiReady(string text)
        {
            lblStatus.Text = text;
            btnTrending.Enabled = btnSearch.Enabled = btnPlay.Enabled = true;
            Cursor = Cursors.Default;
        }

        private async void BtnTrending_Click(object sender, EventArgs e)
        {
            await LoadTrendingAsync();
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            await SearchAsync(txtSearch.Text.Trim());
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            PlaySelected();
        }

        private void DgvVideos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) PlaySelected();
        }

        private async void DgvVideos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVideos.SelectedRows.Count == 0) return;
            var videoId = dgvVideos.SelectedRows[0].Cells["VideoId"].Value?.ToString();
            if (string.IsNullOrEmpty(videoId)) return;

            var v = videos.FirstOrDefault(x => x.VideoId == videoId);
            if (v == null) return;

            // Hiển thị mô tả ngắn gọn
            string shortDesc = v.Description.Substring(0, Math.Min(v.Description.Length, 150)) + (v.Description.Length > 150 ? "..." : "");
            

            // load thumbnail
            if (!string.IsNullOrEmpty(v.ThumbnailUrl))
            {
                try
                {
                    using var s = await httpClient.GetStreamAsync(v.ThumbnailUrl);
                    picThumbnail.Image?.Dispose();
                    picThumbnail.Image = System.Drawing.Image.FromStream(s);
                }
                catch
                {
                    picThumbnail.Image = null;
                }
            }
            else picThumbnail.Image = null;
        }

        private void PlaySelected()
        {
            if (dgvVideos.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn video."); return; }
            string id = dgvVideos.SelectedRows[0].Cells["VideoId"].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;
            string url = $"https://www.youtube.com/watch?v={id}";

            if (webView?.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(url);
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = url, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở trình duyệt: " + ex.Message);
                }
            }
        }
        #endregion    
    }
}