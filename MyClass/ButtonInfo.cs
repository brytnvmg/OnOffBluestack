using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffBluestack
{
    public class ButtonInfo
    {
        public Rectangle ButtonRect { get; set; }
        public string Status { get; set; }

        public ButtonInfo(Rectangle rect, string status)
        {
            ButtonRect = rect;
            Status = status;
        }

        public override string ToString()
        {
            return $"[{ButtonRect.X}, {ButtonRect.Y}, {ButtonRect.Width}, {ButtonRect.Height}] - Status: {Status}";
        }
    }
}
