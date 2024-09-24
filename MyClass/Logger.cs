using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffBluestack
{
    public static class Logger
    {
        public static event Action<string> OnLog;

        public static void Log(string message)
        {
            OnLog?.Invoke(message);
        }
    }
}
