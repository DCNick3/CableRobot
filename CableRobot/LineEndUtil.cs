using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public static class LineEndUtil
    {
        public static string ToUnix(string text) => text.Replace("\r", "");
        public static string ToWindows(string text) => text.Replace("\r", "").Replace("\n", "\r\n");
    }
}
