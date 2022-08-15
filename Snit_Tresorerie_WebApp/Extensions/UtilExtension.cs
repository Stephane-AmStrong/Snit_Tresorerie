using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snit_Tresorerie_WebApp.Extensions
{
    public static class UtilExtension
    {
        public static string Truncate(this string value, int maxChars)
        {
            if (value==null) return "";
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
