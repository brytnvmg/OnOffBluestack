using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace OnOffBluestack
{
    public class OpenCV
    {
        // Trả về rectangle của ảnh, sort theo độ chính xác từ cao tới thấp
        public static List<Rectangle> Find_Image_Rectangle(Bitmap mainImage, Bitmap subImage, double threshold = 0.5)
        {
            var matchInfoList = new List<(Rectangle Rect, double Accuracy)>();
            Image<Bgr, byte> source = new Image<Bgr, byte>(mainImage);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subImage);
            // Kiểm tra kích thước của subImage so với mainImage
            if (template.Width > source.Width || template.Height > source.Height)
            {
                return new List<Rectangle>();
            }
            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                result.ThresholdToZero(new Gray(threshold));
                for (int y = 0; y < result.Rows; y++)
                {
                    for (int x = 0; x < result.Cols; x++)
                    {
                        double matchAccuracy = result.Data[y, x, 0];

                        if (matchAccuracy >= threshold)
                        {

                            Rectangle match = new Rectangle(x, y, template.Width, template.Height);
                            int expansion = 5;
                            Rectangle expandedMatch = new Rectangle(
                                Math.Max(match.Left - expansion, 0),
                                Math.Max(match.Top - expansion, 0),
                                Math.Min(match.Width + 2 * expansion, source.Width),
                                Math.Min(match.Height + 2 * expansion, source.Height));
                            //Console.WriteLine($"Point: {CenterRectangle(match).X} {CenterRectangle(match).Y} - Accuracy: {matchAccuracy}");


                            if (!matchInfoList.Any(m => m.Rect.IntersectsWith(expandedMatch)))
                            {
                                matchInfoList.Add((match, matchAccuracy));
                                CvInvoke.Rectangle(result, expandedMatch, new MCvScalar(0), -1);
                            }
                        }
                    }
                }
            }

            // Sắp xếp danh sách theo độ chính xác giảm dần và trả về chỉ List<Rectangle>
            return matchInfoList.OrderByDescending(m => m.Accuracy).Select(m => m.Rect).ToList();
        }

        public static List<Rectangle> Find_Image_Rectangle(Bitmap mainImage, string subImagePath, double threshold = 0.5)
        {
            using (Bitmap subImage = new Bitmap(subImagePath))
            {
                return Find_Image_Rectangle(mainImage, subImage, threshold);
            }
        }


        // Trả về point có giá trị min_xy hoặc max_xy, hoặc độ chính xác cao nhất
        public static Point Find_First_Image(Bitmap mainImage, Bitmap icon, PointCondition condition, double threshold = 0.5)
        {
            List<Rectangle> rectangles = Find_Image_Rectangle(mainImage, icon, threshold);
            if (rectangles == null || rectangles.Count == 0)
                return Point.Empty; // Hoặc cách xử lý khác nếu danh sách rỗng

            switch (condition)
            {
                case PointCondition.Accuracy:
                    return CenterRectangle(rectangles[0]);
                case PointCondition.MinXY:
                    return CenterRectangle(rectangles.OrderBy(r => r.Left + r.Top).FirstOrDefault());
                case PointCondition.MaxXY:
                    return CenterRectangle(rectangles.OrderByDescending(r => r.Right + r.Bottom).FirstOrDefault());
                default:
                    Console.WriteLine("MyOpenCV.Find_Image: Invalid condition.");
                    return CenterRectangle(rectangles[0]);
            }
        }

        public static Point Find_First_Image(Bitmap mainImage, string iconPath, PointCondition condition, double threshold = 0.5)
        {
            using (Bitmap icon = new Bitmap(iconPath))
            {
                return Find_First_Image(mainImage, icon, condition, threshold);
            }
        }

        //public static bool Find_First_Image_And_Click(string emulator_Id, string iconPath, PointCondition condition, double threshold = 0.5)
        //{
        //    Bitmap screen = ADB.Screen_Shoot(emulator_Id);
        //    Point point = Find_First_Image_With_Scale(screen, iconPath, condition);
        //    if (point != Point.Empty)
        //    {
        //        ADB.Tap(emulator_Id, point);
        //        return true;
        //    }
        //    return false;
        //}

        public static Point Find_First_Image_With_Scale(Bitmap mainImage, Bitmap icon, PointCondition condition, double threshold = 0.5)
        {
          
            List<double> scales = new List<double> { 0.8, 1.0, 0.9, 1.1, 1.2 };

            foreach (double scale in scales)
            {
                // Thay đổi kích thước icon
                using (Bitmap resizedIcon = new Bitmap(icon, new Size((int)(icon.Width * scale), (int)(icon.Height * scale))))
                {
                    // Tìm kiếm hình ảnh
                    Point point = Find_First_Image(mainImage, resizedIcon, condition, threshold);

                    if (point != Point.Empty)
                    {
                        //Console.WriteLine($"OpenCV.Find_First_Image_Width_Scale Scale: {scale}");
                        return point;
                    }
                }
            }
            return Point.Empty;
        }

        public static Point Find_First_Image_With_Scale(Bitmap mainImage, string iconPath, PointCondition condition, double threshold = 0.5)
        {
            using (Bitmap icon = new Bitmap(iconPath))
            {
                return Find_First_Image_With_Scale(mainImage, icon, condition, threshold);
            }
        }

        //public static bool Find_First_Image_With_Scale_And_Click(string emulator_Id, string iconPath, PointCondition condition, double threshold = 0.5)
        //{
        //    Bitmap screen = ADB.Screen_Shoot(emulator_Id);
        //    Point point = Find_First_Image_With_Scale(screen, iconPath, condition, threshold);
        //    if (point != Point.Empty)
        //    {
        //        ADB.Tap(emulator_Id, point);
        //        return true;
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Hàm nhập vào 1 mảng icon
        ///// </summary>
       
        //public static bool Find_First_Image_By_Arr_Icon(string emulator_Id, string[] iconPaths, PointCondition condition, double threshold = 0.5)
        //{
        //    Bitmap screen = ADB.Screen_Shoot(emulator_Id);
        //    screen = ConvertToThresholded(screen, 200);
        //    foreach (var iconPath in iconPaths)
        //    {
        //        Point point = Find_First_Image(screen, iconPath, condition, threshold);
        //        if (point != Point.Empty)
        //        {
        //            ADB.Tap(emulator_Id, point);
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public static bool Find_First_Image_With_Scale_And_Click(string emulator_Id, Bitmap mainImage, string iconPath, PointCondition condition, double threshold = 0.5)
        //{
        //    Point point = Find_First_Image_With_Scale(mainImage, iconPath, condition, threshold);
        //    if (point != Point.Empty)
        //    {
        //        ADB.Tap(emulator_Id, point);
        //        return true;
        //    }
        //    return false;
        //}


        // Trả về point giữa retangle
        public static Point CenterRectangle(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        }

        public static bool Have_Image(Bitmap mainImage, string iconPath, double threshold = 0.5)
        {
            // Tạo icon từ đường dẫn file, dùng using để giải phóng sau khi sử dụng
            using (Bitmap icon = new Bitmap(iconPath))
            {
                return Have_Image(mainImage, icon, threshold);
            }
        }

        public static bool Have_Image(Bitmap mainImage, Bitmap icon, double threshold = 0.5)
        {
            Point point = Find_First_Image_With_Scale(mainImage, icon, PointCondition.Accuracy, threshold);
            if (point != Point.Empty)
            {
                return true;
            }
            return false;
        }


       

    }

    public enum PointCondition
    {
        Accuracy,
        MinXY,
        MaxXY
    }
}