using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipper_from_Blank
{
    class Formmater
    {
        public static string DoubleToCurrencyString(double amount) {
            return "$" + amount.ToString("0.00");
        }

        public static double PercentageStringToDouble(string pct) {
            string nStr = pct.Substring(0, pct.Length - 2);
            double result;
            if (double.TryParse(nStr, out result)) {
                return result;
            } else {
                return -1.0;
            }

        }
    }
}
