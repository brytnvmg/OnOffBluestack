using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffBluestack
{
    public static class BitmapExtensions
    {
        public static void Save_Image(this Bitmap bitmap, string fileName = "")
        {
            if (bitmap == null)
            {
                Logger.Log("Save_Image: Bitmap is null");
                return;
            }
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = Utils.GenerateRandomString(5);
            }
            fileName = EnsureValidExtension(fileName);
            string fullPath = Path.Combine(MyConstant.SAVE_IMAGE, fileName);

            // Tạo thư mục nếu nó không tồn tại
            Directory.CreateDirectory(MyConstant.SAVE_IMAGE);

            // Lưu file ở định dạng PNG
            bitmap.Save(fullPath);
        }

        private static string EnsureValidExtension(string fileName)
        {
            // Danh sách các phần mở rộng hợp lệ
            var validExtensions = new[] { ".png", ".jpg", ".jpeg" };

            // Kiểm tra xem fileName đã có phần mở rộng hợp lệ chưa
            if (!validExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                // Nếu không, thêm mặc định là .png
                fileName += ".png";
            }

            return fileName;
        }

        /// <summary>
        /// Hàm cắt ảnh cần trong vùng rectangle, xóa trắng vùng  ngoài
        /// </summary>
        public static Bitmap Crop_And_White_Out(this Bitmap originalBitmap, double xPercent, double yPercent, double widthPercent, double heightPercent)
        {
            // Crop phần ảnh cần giữ lại
            Bitmap croppedBitmap = originalBitmap.Crop_Image(xPercent, yPercent, widthPercent, heightPercent);

            // Tạo một bản sao của ảnh gốc
            Bitmap resultBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            // Tô trắng toàn bộ ảnh
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.Clear(Color.White);
            }

            // Vẽ lại phần ảnh đã crop lên ảnh kết quả tại đúng vị trí
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                int x = (int)(xPercent * originalBitmap.Width / 100);
                int y = (int)(yPercent * originalBitmap.Height / 100);
                Rectangle destRect = new Rectangle(x, y, croppedBitmap.Width, croppedBitmap.Height);

                g.DrawImage(croppedBitmap, destRect);
            }

            return resultBitmap;
        }

        public static Bitmap Crop_Image(this Bitmap originalBitmap, double xPercent, double yPercent, double widthPercent, double heightPercent)
        {
            // Tính toán giá trị pixel từ tỉ lệ %
            int x = (int)(xPercent * originalBitmap.Width / 100);
            int y = (int)(yPercent * originalBitmap.Height / 100);
            int width = (int)(widthPercent * originalBitmap.Width / 100);
            int height = (int)(heightPercent * originalBitmap.Height / 100);

            // Tạo bitmap mới chỉ với vùng ảnh cần cắt
            Bitmap croppedBitmap = new Bitmap(width, height);

            // Vẽ lại phần ảnh cần giữ từ ảnh gốc lên ảnh đã cắt
            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                Rectangle cropRect = new Rectangle(0, 0, width, height);
                g.DrawImage(originalBitmap, cropRect, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }


    }
}
