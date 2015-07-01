using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class LaserData : Dictionary<int, float>
    {
        public static float operator *(LaserData x, LaserData y)
        {
            var result = 0f;
            foreach (var x1 in x)
            {
                if (y.ContainsKey(x1.Key))
                {
                    result += x1.Value*y[x1.Key];
                }
            }
            return result;
        }

        public LaserData RotateOn90()
        {
            var ld = new LaserData();
            foreach (var d in this)
            {
                if (d.Key < 90)
                {
                    ld[d.Key + 270] = d.Value;
                }
                else
                {
                    ld[d.Key - 90] = d.Value;
                }

            }
            return ld;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Laser data: ");
            foreach (var k in this)
            {
                sb.Append(string.Format("ang. {0}: {1}, ", k.Key, k.Value));
            }
            return sb.ToString();
        }

        public LaserData Normalize()
        {
            // get vector power
            var f = 0f;
            foreach (var k in this)
            {
                f += Mathf.Pow(k.Value, 2);
            }
            f = Mathf.Sqrt(f);
            var newData = new LaserData();
            foreach (var k in this)
            {
                newData.Add(k.Key, k.Value/f);
            }
            return newData;
        }
    }
}
