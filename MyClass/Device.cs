using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace OnOffBluestack
{
    public class Device
    {
        public string Auto_Status { get; set; }
        public DeviceType Type { get; private set; }
        public string Instance_Id { get; private set; }
        public string Name { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Dpi { get; set; }
        public int Index { get; set; }


        public Device(DeviceType type, string instance_Id, string name)
        {
            Type = type;
            Instance_Id = instance_Id;
            Name = name;
           
            Width = 0;
            Height = 0;
            Dpi = 0;
            Index = -1;
        }

        /// <summary>
        /// Chuyển từ list device sang item listview, có lấy trạng thái auto running từ database
        /// </summary>
        public ListViewItem Convert_To_Item()
        {
            ListViewItem listViewItem = new ListViewItem();

            listViewItem.SubItems.Add(Instance_Id.ToString());
            listViewItem.SubItems.Add(Name);
            listViewItem.SubItems.Add(Width.ToString());
            listViewItem.SubItems.Add(Height.ToString());
            listViewItem.SubItems.Add(Dpi.ToString());

            return listViewItem;
        }
    

    }


    /// <summary>
    /// Các hàm extension & enum
    /// </summary>

    public static class ListViewItemExtensions
    {
        public static Device Convert_To_Device(this ListViewItem item, ListView listView)
        {
            // Phân tích các giá trị từ SubItems cho 4 tham số chính
            string instanceId = item.Get_Sub_Item_Text(listView, "Instance");
            string name = item.Get_Sub_Item_Text(listView, "Name");

            // Khởi tạo đối tượng Device với 4 tham số
            Device device = new Device(DeviceType.BLUESTACK, instanceId, name);
            device.Index = item.Index;
            // Gán giá trị cho các thuộc tính còn lại từ subitems
            device.Width = int.TryParse(item.Get_Sub_Item_Text(listView, "Width"), out int width) ? width : 0;
            device.Height = int.TryParse(item.Get_Sub_Item_Text(listView, "Height"), out int height) ? height : 0;
            device.Dpi = int.TryParse(item.Get_Sub_Item_Text(listView, "Dpi"), out int dpi) ? dpi : 0;

            return device;
        }


        public static string Get_Sub_Item_Text(this ListViewItem item, ListView listView, string columnName)
        {
            int columnIndex = -1;
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                if (listView.Columns[i].Text == columnName)
                {
                    columnIndex = i;
                    break;
                }
            }

            if (columnIndex != -1)
            {
                return item.SubItems[columnIndex].Text;
            }
            else
            {
                return "";
            }
        }

        public static void Config_Column(this ListView listView, string[] columnNames, int[] columnWidths)
        {
            // Kiểm tra đảm bảo rằng cả hai mảng có cùng kích thước
            if (columnNames.Length != columnWidths.Length)
            {
                Logger.Log("Device.Config_Column: columnNames.Length != columnWidths.Length");
            }
            listView.Columns.Clear(); // Xóa các cột hiện tại trước khi thêm mới
            listView.CheckBoxes = false;
            listView.Columns.Add("", 0);
            for (int i = 0; i < columnNames.Length; i++)
            {
                listView.Columns.Add(columnNames[i], columnWidths[i]);
            }
        }
    }

    public enum DeviceType
    {
        ALL,
        LDPLAYER,
        BLUESTACK,
        MANUAL,
        OTHER
    }

  
}
