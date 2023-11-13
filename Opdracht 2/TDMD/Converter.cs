using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMD
{
    public static class Converter
    {
        public static double ValueToPercentage(double value)
        {
            double percentage = (value / 254.0) * 100.0;
            return Math.Round(percentage);
        }
        public static double PercentageToValue(double percentage)
        {
            double convertedValue = (percentage / 100.0) * 253.0 + 1.0;
            return convertedValue;
        }
    }
}
