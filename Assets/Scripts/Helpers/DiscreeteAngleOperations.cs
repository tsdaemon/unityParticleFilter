using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Helpers
{
    public static class DiscreeteAngleOperations
    {
        public static int Summ(int x, int y)
        {
            var k = x + y;
            if (k > 3)
            {
                k -= 4;
            }
            return k;
        }

        public static int Diff(int x, int y)
        {
            var k = x - y;
            if (k < 0)
            {
                k += 4;
            }
            return k;
        }
    }
}
