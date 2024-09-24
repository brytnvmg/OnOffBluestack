using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffBluestack
{
    public static class MyConstant
    {

        // Lấy đường dẫn đến thư mục chứa file thực thi
        //public static string EXE_PATH = AppDomain.CurrentDomain.BaseDirectory;

        // Lấy đường dẫn đến thư mục project (hai cấp trên thư mục chứa file thực thi)
        //public static string PROJECT_DIR = Directory.GetParent(Directory.GetParent(Directory.GetParent(EXE_PATH).FullName).FullName).FullName;

#if DEBUG // file exe nằm ở thư mục debug
        public static string PROJECT_DIR = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName;
#else // release thì file exe nằm ở thư mục gốc
        public static string PROJECT_DIR = AppDomain.CurrentDomain.BaseDirectory;
#endif

        // Các đường dẫn thư mục
        public static string IMAGE_PATH = Path.Combine(PROJECT_DIR, "images");
        public static string ICON_PATH = Path.Combine(IMAGE_PATH, "icons");
        public static string IMAGE_TEMP_PATH = Path.Combine(IMAGE_PATH, "temp");
        public static string TESSDATA_PATH = "D:\\10_program\\tessdata";
        public static string SAVE_IMAGE = "C:\\Users\\TranNam13500\\Desktop\\save_images";

        // Các đường dẫn icon
       
        public static string ICON_START_BLUESTACK_2K = Path.Combine(ICON_PATH, "ICON_START_BLUESTACK_2K.png");
        public static string ICON_STOP_BLUESTACK_2K = Path.Combine(ICON_PATH, "ICON_STOP_BLUESTACK_2K.png");
        public static string ICON_CLOSE_BLUESTACK_2K = Path.Combine(ICON_PATH, "ICON_CLOSE_BLUESTACK_2K.png");

        public static string ICON_START_BLUESTACK_FULL_HD = Path.Combine(ICON_PATH, "ICON_START_BLUESTACK_FULL_HD.png");
        public static string ICON_STOP_BLUESTACK_FULL_HD = Path.Combine(ICON_PATH, "ICON_STOP_BLUESTACK_FULL_HD.png");
        public static string ICON_CLOSE_BLUESTACK_FULL_HD = Path.Combine(ICON_PATH, "ICON_CLOSE_BLUESTACK_FULL_HD.png");


    }

    public class PercentPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        private static double SCREEN_WIDTH = 1600.0;
        private static double SCREEN_HEIGHT = 900.0;
      
        public PercentPoint(double x, double y)
        {
            X = (x / SCREEN_WIDTH) * 100;
            Y = (y / SCREEN_HEIGHT) * 100;
        }
    }
}
