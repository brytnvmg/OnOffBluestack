using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Threading;

namespace OnOffBluestack
{
    public class Utils
    {

        // Tạo 1 chuỗi 15 kí tự là token tổng cho auto kết nối websocket
        // Webserver sẽ gủi đến token này kèm emulator_token là chuỗi 10 kí tự mà chủ auto cung cấp cho user
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static string GetHardDriveSerialNumber()
        {
            string serialNumber = "";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    // Property "SerialNumber" chứa mã ổ cứng
                    serialNumber = wmi_HD["SerialNumber"]?.ToString()?.Trim();
                    //Console.WriteLine(serialNumber);
                    break; // Chỉ lấy mã ổ cứng đầu tiên
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Không thể lấy mã ổ cứng: " + ex.Message);
            }

            return serialNumber;
        }

     
    }


}
