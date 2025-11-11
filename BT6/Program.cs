using BT6;

namespace BT6
{
    internal static class Program
    {
        // Sử dụng ManualResetEvent để Admin báo hiệu cho User biết đã cập nhật lần đầu xong.
        public static ManualResetEvent AdminReadySignal = new ManualResetEvent(false);

        [STAThread]
        //static void Main()
        //{

        //https://aka.ms/applicationconfiguration.
        //    ApplicationConfiguration.Initialize();
        //    Application.Run(new ThoiTiet());
        //}
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Lấy kích thước màn hình làm việc
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;


            // CẬP NHẬT KÍCH THƯỚC FORM MỚI

            int adminWidth = 741;
            int adminHeight = 301;

            int userWidth = 872;
            int userHeight = 619;

            int totalWidth = adminWidth + 40 + userWidth;
            int formHeight = Math.Max(adminHeight, userHeight);


            // Tính toán vị trí X và Y để căn giữa TỔNG THỂ hai Form
            int xOffset = (screen.Width - totalWidth) / 2;
            int yOffset = (screen.Height - formHeight) / 2;

            // Thiết lập vị trí AdminForm (Bên trái)
            AdminForm adminForm = new AdminForm();
            adminForm.StartPosition = FormStartPosition.Manual;
            adminForm.Location = new Point(xOffset, yOffset + (formHeight - adminHeight) / 2); // Căn giữa Admin theo chiều dọc

            // Thiết lập vị trí UserForm (Bên phải)
            UserForm userForm = new UserForm();
            userForm.StartPosition = FormStartPosition.Manual;
            userForm.Location = new Point(xOffset + adminWidth + 40, yOffset + (formHeight - userHeight) / 2); // Đặt bên phải AdminForm 40px và căn giữa User theo chiều dọc

            // Khởi chạy AdminForm trên luồng riêng
            Thread adminThread = new Thread(() =>
            {
                Application.Run(adminForm);
            });
            adminThread.SetApartmentState(ApartmentState.STA);
            adminThread.Start();

            // Khởi chạy UserForm trên luồng riêng
            Thread userThread = new Thread(() =>
            {
                Application.Run(userForm);
            });
            userThread.SetApartmentState(ApartmentState.STA);
            userThread.Start();

            // THOÁT Main Thread
        }
    }

}