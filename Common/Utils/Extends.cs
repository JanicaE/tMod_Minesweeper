using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Common.Utils
{
    internal static class Extends
    {
        public static string DefaultIfEmpty(this string value, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }
    }
}
