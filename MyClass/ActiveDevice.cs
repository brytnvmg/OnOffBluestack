using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffBluestack
{
    public class ActiveDevice
    {
        public string Type { get; set; }
        public string Instance_Id { get; set; }

        public ActiveDevice(string type, string instance_Id)
        {
            Type = type;
            Instance_Id = instance_Id;
        }
    }
}
