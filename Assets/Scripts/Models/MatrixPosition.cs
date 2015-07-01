using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Models
{
    public class MatrixPosition
    {
        public int x;
        // direction 0 - +z, 1 - +z, 2 - -z, 3 - -x
        public int k;
        public int z;

        public static MatrixPosition operator -(MatrixPosition x, MatrixPosition y)
        {
            var k = DiscreeteAngleOperations.Diff(x.k, y.k);
            return new MatrixPosition {k = k, x = x.x - y.x, z = x.z - y.z};
        }
        public static MatrixPosition operator +(MatrixPosition x, MatrixPosition y)
        {
            var k = DiscreeteAngleOperations.Summ(x.k, y.k);
            return new MatrixPosition { k = k, x = x.x + y.x, z = x.z + y.z };
        }

        public override string ToString()
        {
            return string.Format("x: {0}, z: {1}, k: {2}", x, z, k);
        }
    }
}
