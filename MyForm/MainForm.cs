using Emgu.CV.Ocl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnOffBluestack
{
    public partial class mainForm : Form
    {

        private int previousSelectedIndex = -1;

        // Màn FULL HD
        double thresholdByResolution = 0.7; 
        string ICON_CLOSE = MyConstant.ICON_CLOSE_BLUESTACK_FULL_HD;
        string ICON_START = MyConstant.ICON_START_BLUESTACK_FULL_HD;
        string ICON_STOP = MyConstant.ICON_STOP_BLUESTACK_FULL_HD;
        string ICON_SORT = MyConstant.ICON_SORT_BLUESTACK_FULL_HD;
        string ICON_TEXT_SORT_ALL = MyConstant.ICON_TEXT_SORT_ALL_FULL_HD;

        // Màn 2K
        //double thresholdByResolution = 0.9; 
        //string ICON_CLOSE = MyConstant.ICON_CLOSE_BLUESTACK_2K;
        //string ICON_START = MyConstant.ICON_START_BLUESTACK_2K;
        //string ICON_STOP = MyConstant.ICON_STOP_BLUESTACK_2K;
        //string ICON_SORT = MyConstant.ICON_SORT_BLUESTACK_2K;
        //string ICON_TEXT_SORT_ALL = MyConstant.ICON_TEXT_SORT_ALL_2K;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;

        private void ClickAt(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        private void ClickCenterRectangle(Rectangle rect)
        {
            Point point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            ClickAt(point.X, point.Y);  
        }

        #region LOAD FORM
        public mainForm()
        {
            InitializeComponent();
            Get_Version_App();
            Config_Device_List_View();
            countdownLabel.Text = "";
            Logger.OnLog += Log;
        }
        /// <summary>
        /// Khởi tạo các giá trị sau khi load xong giao diện form
        /// </summary>
        private void mainForm_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Pre_Load_Form_Async));
            thread.Start();
            //Test_Image();
        }

        private void Get_Version_App()
        {
            // Lấy phiên bản từ Assembly Info
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Hoặc nếu bạn muốn định dạng như "1.0.2" thay vì "1.0.2.0"
            version = version.Substring(0, version.LastIndexOf('.'));

            // Cập nhật tiêu đề của form
            this.Text = $"On Off Bluestack by roktop.net {version}";
        }

        /// <summary>
        /// Chạy các hàm bất đồng bộ sau khi hiện chương trình
        /// </summary>
        private void Pre_Load_Form_Async()
        {
            try
            {
                Bluestacks.Bluestack_Path = Bluestacks.Get_Bluestack_Path();
                Get_Active_Devices();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi trong MessageBox
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        /// ===================================================================================

        #region START STOP AUTO - CÁC HÀM XỬ LÝ LOGIC

        /// <summary>
        /// Start Stop auto button
        /// </summary>
        private async void startAuto_Button_Click(object sender, EventArgs e)
        {
            Device device = Get_Device_Selected();
            if (device == null) { return; }

            var cancellationTokenSource = new CancellationTokenSource();

            // Khởi động đồng thời đếm ngược và xử lý logic
            Task countdownTask = CountdownAndDisableButton(10, cancellationTokenSource.Token);
            Task logicTask = Task.Run(() =>
            {
                List<ButtonInfo> allButtons = getListRectangleButton();
                if (device.Index >= 0 && device.Index < allButtons.Count)
                {
                    ButtonInfo buttonByIndex = allButtons[device.Index];

                    if (buttonByIndex.Status == "Start")
                    {
                        ClickCenterRectangle(buttonByIndex.ButtonRect);
                        Logger.Log(device.Name + " đã khởi động Bluestack, đợi một chút");
                    }
                    else
                    {
                        Logger.Log(device.Name + " đang chạy");
                        cancellationTokenSource.Cancel();  // Hủy đếm ngược
                    }
                }
                else
                {
                    Logger.Log($"{device.Name} index {device.Index} vượt quá số lượng {allButtons.Count} button có sẵn");
                    cancellationTokenSource.Cancel();
                }
            });

            // Chạy song song đếm ngược và logic
            await Task.WhenAll(countdownTask, logicTask);
        }

        private async void stopAuto_Button_Click(object sender, EventArgs e)
        {
            Device device = Get_Device_Selected();
            if (device == null) { return; }

            var cancellationTokenSource = new CancellationTokenSource();

            // Khởi động đồng thời đếm ngược và xử lý logic
            Task countdownTask = CountdownAndDisableButton(15, cancellationTokenSource.Token);
            Task logicTask = Task.Run(() =>
            {
                List<ButtonInfo> allButtons = getListRectangleButton();
                if (device.Index >= 0 && device.Index < allButtons.Count)
                {
                    ButtonInfo buttonByIndex = allButtons[device.Index];

                    if (buttonByIndex.Status == "Stop")
                    {
                        ClickCenterRectangle(buttonByIndex.ButtonRect);
                        Thread.Sleep(1000);
                        Bitmap screen = CaptureScreen();
                        Point buttonClosePoint = OpenCV.Find_First_Image(screen, ICON_CLOSE, PointCondition.MinXY, thresholdByResolution); // Màn 2k để 0.85
                        if (buttonClosePoint != Point.Empty)
                        {
                            ClickAt(buttonClosePoint.X, buttonClosePoint.Y);
                            Logger.Log(device.Name + " đã đóng Bluestack, đợi cho tắt hẳn");
                        }
                        else
                        {
                            Logger.Log(device.Name + " không tìm thấy nút đóng bluestack");
                        }
                    }
                    else
                    {
                        Logger.Log(device.Name + " đang tắt");
                        cancellationTokenSource.Cancel();  // Hủy đếm ngược
                    }
                }
                else
                {
                    Logger.Log($"{device.Name} index {device.Index} vượt quá số lượng {allButtons.Count} button");
                    cancellationTokenSource.Cancel();
                }
            });

            // Chạy song song đếm ngược và logic
            await Task.WhenAll(countdownTask, logicTask);
        }

        /// SẮP XẾP BLUESTACK
        private async void sortBluestack_Button_Click(object sender, EventArgs e)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            // Khởi động đồng thời đếm ngược và xử lý logic
            Task countdownTask = CountdownAndDisableButton(5, cancellationTokenSource.Token);
            Task logicTask = Task.Run(() =>
            {

                Bitmap screen = CaptureScreen();
                Point iconSortBluestack = OpenCV.Find_First_Image(screen, ICON_SORT, PointCondition.MinXY, thresholdByResolution); 
                if (iconSortBluestack != Point.Empty)
                {
                    ClickAt(iconSortBluestack.X, iconSortBluestack.Y);
                    Thread.Sleep(1000);
                    screen = CaptureScreen();
                    Point textSortAll = OpenCV.Find_First_Image(screen, ICON_TEXT_SORT_ALL, PointCondition.MinXY, thresholdByResolution);
                    if (textSortAll != Point.Empty)
                    {
                        ClickAt(textSortAll.X, textSortAll.Y);
                        Logger.Log("Đã sắp xếp Bluestack");
                    }
                }
                else
                {
                    Logger.Log("Không tìm thấy nút sắp xếp Bluestack");
                }
                
            });

            // Chạy song song đếm ngược và logic
            await Task.WhenAll(countdownTask, logicTask);
        }

        /// <summary>
        /// Hàm đếm ngược dùng chung:
        /// </summary>

        private async Task CountdownAndDisableButton(int seconds, CancellationToken cancellationToken)
        {
            // Vô hiệu hóa cả hai nút
            panel1.Enabled = false;

            for (int i = seconds; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break; // Ngừng đếm ngược nếu có yêu cầu hủy
                }

                // Cập nhật text cho startButton
                countdownLabel.Invoke(new Action(() =>
                {
                    countdownLabel.Text = $"Waiting... ({i}s)";
                }));

                await Task.Delay(1000);  // Đợi 1 giây
            }

            // Kích hoạt lại cả hai nút và đặt lại text riêng biệt
            panel1.Invoke(new Action(() =>
            {
                panel1.Enabled = true;
            }));

            countdownLabel.Invoke(new Action(() =>
            {
                countdownLabel.Text = "";
            }));
        }




        /// <summary>
        /// Hàm xử lý logic chính (Start và Stop):
        /// </summary>

        private async Task ProcessAutoTask(Device device, string expectedStatus, Action<Rectangle> clickAction)
        {
            await Task.Run(() =>
            {
                try
                {
                    List<ButtonInfo> allButtons = getListRectangleButton();
                    if (device.Index >= 0 && device.Index < allButtons.Count)
                    {
                        ButtonInfo buttonByIndex = allButtons[device.Index];

                        if (buttonByIndex.Status == expectedStatus)
                        {
                            clickAction(buttonByIndex.ButtonRect);  // Thực hiện hành động click với tọa độ
                        }
                        else
                        {
                            Logger.Log(device.Name + (expectedStatus == "Start" ? " đang chạy" : " đang tắt"));
                        }
                    }
                    else
                    {
                        Logger.Log($"Index {device.Index} vượt quá số lượng button có sẵn: {allButtons.Count}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }


        /// <summary>
        /// Lấy danh sách tọa độ index các button và trạng thái
        /// </summary>
        private List<ButtonInfo> getListRectangleButton()
        {
            Bitmap screen = CaptureScreen();

            List<ButtonInfo> allButton = new List<ButtonInfo>();

            // Tìm các button Start và thêm chúng vào danh sách với trạng thái "Start"
            List<Rectangle> listButtonStart = OpenCV.Find_Image_Rectangle(screen, ICON_START, thresholdByResolution); // màn 2k phải để 0.9 không là nhận nhầm button Phiên Bản
            foreach (var rect in listButtonStart)
            {
                allButton.Add(new ButtonInfo(rect, "Start"));
            }

            // Tìm các button Stop và thêm chúng vào danh sách với trạng thái "Stop"
            List<Rectangle> listButtonStop = OpenCV.Find_Image_Rectangle(screen, ICON_STOP, thresholdByResolution); 
            foreach (var rect in listButtonStop)
            {
                allButton.Add(new ButtonInfo(rect, "Stop"));
            }

            // Sắp xếp danh sách allButton theo tọa độ Y
            allButton = allButton.OrderBy(btn => btn.ButtonRect.Y).ToList();


            // In danh sách các button Start
            //Logger.Log("List Button: " + ListToString(allButton));

            return allButton;
        }

        // Chuyển danh sách Rectangle thành chuỗi mô tả
        private string ListToString(List<ButtonInfo> list)
        {
            return string.Join(", ", list.Select(btn => btn.ToString()));
        }

        /// <summary>
        /// Chụp ảnh màn hình chính
        /// </summary>
        private Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return screenshot;
        }


        #endregion

        /// ===================================================================================

        #region HỖ TRỢ XỬ LÝ GIAO DIỆN FORM

        /// <summary>
        /// Config danh sách device list view
        /// </summary>
        private void Config_Device_List_View()
        {
            string[] columnNames = { "Instance", "Name", "Width", "Height", "Dpi" };
            int[] columnWidths = { 75, 150, 45, 45, 40 };
            device_ListView.Config_Column(columnNames, columnWidths);
            device_ListView.ItemSelectionChanged += deviceListView_ItemSelectionChanged;
        }

        /// <summary>
        /// Hàm Log
        /// </summary>
        private void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            this.Invoke((MethodInvoker)delegate
            {
                // Lấy tất cả các dòng log hiện tại
                var lines = log_TextBox.Lines.ToList();

                // Nếu số dòng vượt quá 200, xóa dòng đầu tiên
                if (lines.Count >= 200)
                {
                    lines.RemoveAt(0);
                }

                // Thêm dòng log mới
                lines.Add("[" + timestamp + "]" + " " + message);

                // Gán lại các dòng log vào TextBox
                log_TextBox.Lines = lines.ToArray();

                // Cuộn xuống dòng cuối cùng
                log_TextBox.SelectionStart = log_TextBox.Text.Length;
                log_TextBox.ScrollToCaret();
            });
        }




        /// <summary>
        /// Lấy danh sách các Device đã thêm có sẵn trong database
        /// </summary>

        private void Get_Active_Devices()
        {
            //Hiển thị PictureBox spinner trước khi bắt đầu
            this.Invoke((MethodInvoker)delegate
            {
                device_ListView.Items.Clear();
            });

            try
            {
              
                List<Device> all_Devices = Bluestacks.Get_All_Devices();

                foreach (Device device in all_Devices)
                {
                    // Sử dụng Invoke để cập nhật UI từ main thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        device_ListView.Items.Add(device.Convert_To_Item());
                    });
                }
            }
            catch
            {
                // Không làm gì cả khi chương trình đã thoát
            }
            finally
            {
             
            }
        }

        /// <summary>
        /// Lấy ra Device đang được chọn click vào trong list view
        /// </summary>
        private Device Get_Device_Selected()
        {
            if (device_ListView.SelectedItems.Count > 0)
            {
                // Lấy dòng được chọn
                ListViewItem item = device_ListView.SelectedItems[0];
                return item.Convert_To_Device(device_ListView);
            }
            else
            {
                //Logger.Log("Please select a device.");
                MessageBox.Show("Please select a device.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }


        /// <summary>
        /// Refresh Device list
        /// </summary>
        private void refreshDevice_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Get_Active_Devices();
        }

        /// <summary>
        /// Hàm xử lý sự kiện row selected change list view
        /// previousSelectedIndex để xác định row vừa click có phải là row khác ko
        /// nếu click row cũ đã select rồi thì vẫn giữ nguyên form mà ko cần thay đổi gì
        /// ở vế else khi 1 hàng bị bỏ chọn vẫn phải kiểm tra e.ItemIndex != previousSelectedIndex
        /// nếu không sẽ bị lỗi khi click vào 1 row, bỏ focus listview rồi click lại row đó sẽ bị xóa trắng form
        /// </summary>
        private void deviceListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            //Hành động khi một hàng được chọn
            if (e.IsSelected)
            {
                if (e.ItemIndex != previousSelectedIndex)
                {
                    Device device = Get_Device_Selected();
                    if (device == null) { return; }
                    deviceInfoLabel.Text = device.Instance_Id;
                    deviceInfoLabel.Text += " - " + device.Name;
                    deviceInfoLabel.Text += " - " + device.Width + " x " + device.Height;
                }
                // Cập nhật chỉ mục của dòng đã chọn trước đó
                previousSelectedIndex = e.ItemIndex;
            }
            // Hành động khi một hàng bị bỏ chọn
            else
            {
                if (e.ItemIndex != previousSelectedIndex)
                {
                    if (device_ListView.SelectedItems.Count == 0)
                    {
                        deviceInfoLabel.Text = "";
                    }
                }
            }
        }


        private void alwaysOnTop_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Bật/tắt chế độ luôn ở trên cùng
            this.TopMost = !this.TopMost;
            Logger.Log("Always on top = " + this.TopMost.ToString());
        }

        #endregion

    }

}
