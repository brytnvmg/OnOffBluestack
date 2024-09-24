using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace OnOffBluestack
{
    public class Bluestacks
    {
        private static string _bluestack_Path = "";

        public static string Bluestack_Path
        {
            get { return _bluestack_Path; }
            set
            {
                _bluestack_Path = value;
                bluestack_Config_Path = Path.Combine(_bluestack_Path, "bluestacks.conf");
            }
        }

        public static string bluestack_Config_Path { get; private set; }

        public static List<Device> Get_All_Devices()
        {
            var devices = new List<Device>();
            if (!File.Exists(bluestack_Config_Path))
            {
                return devices;
            }
            var config = File.ReadAllText(bluestack_Config_Path);
            var instancePattern = new Regex(@"bst\.instance\.(.*?)\.");
            var instanceMatches = instancePattern.Matches(config);

            // Sử dụng HashSet để lưu trữ và kiểm tra trùng lặp các ID instance
            var processedInstances = new HashSet<string>();

            int count = 0;
            foreach (Match instanceMatch in instanceMatches)
            {
                var instance_Id = instanceMatch.Groups[1].Value;

                // Kiểm tra xem instance này đã được xử lý hay chưa
                if (!processedInstances.Contains(instance_Id))
                {
                    processedInstances.Add(instance_Id); // Đánh dấu instance này đã được xử lý

                    var name = GetValueFromConfig(config, $"bst.instance.{instance_Id}.display_name");
                    var portString = GetValueFromConfig(config, $"bst.instance.{instance_Id}.adb_port");
                    var widthString = GetValueFromConfig(config, $"bst.instance.{instance_Id}.fb_width");
                    var heightString = GetValueFromConfig(config, $"bst.instance.{instance_Id}.fb_height");
                    var dpiString = GetValueFromConfig(config, $"bst.instance.{instance_Id}.dpi");

                    Device device = new Device(DeviceType.BLUESTACK, instance_Id, name);
                    // Gán các thuộc tính còn lại sau khi đã khởi tạo
                    device.Width = int.TryParse(widthString, out var width) ? width : 0;
                    device.Height = int.TryParse(heightString, out var height) ? height : 0;
                    device.Dpi = int.TryParse(dpiString, out var dpi) ? dpi : 0;
                    device.Index = count;
                    count++;    

                    devices.Add(device);
                }
            }

            return devices;
        }


        private static string GetValueFromConfig(string config, string key)
        {
            var pattern = new Regex($"{key}=\"(.*?)\"");
            var match = pattern.Match(config);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public static string Get_Bluestack_Path()
        {
            string configPath = Path.Combine(MyConstant.PROJECT_DIR, "config.txt");

            if (File.Exists(configPath))
            {
                // Đọc dòng đầu tiên của file config.txt
                string firstLine = File.ReadLines(configPath).First();
                return firstLine;
            }
            else
            {
                string path = @"C:\ProgramData\BlueStacks_nxt";
                using (StreamWriter sw = File.CreateText(configPath))
                {
                    sw.WriteLine(path);
                }
                return path;
            }
        }

    }
}
