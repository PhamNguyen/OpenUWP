using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace OpenUWP.Extentions
{
    public static class RandomExtentions
    {
        public static int Next(this Random rand, int minValue, int maxValue, params int[] ignoreValues)
        {
            int value = rand.Next(minValue, maxValue);
            while (ignoreValues.Contains(value))
            {
                value = rand.Next(minValue, maxValue);
            }
            return value;
        }
    }
}
