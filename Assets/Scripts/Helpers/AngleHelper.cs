using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class AngleHelper
    {
        private static float grToRad = Mathf.PI/180f;
        public static float GradToRad(float grad)
        {
            return grad*grToRad;
        }

        public static float NormalizeGrad(float grad)
        {
            var c = Mathf.FloorToInt(grad/360);
            grad = grad - c*360;
            return grad;
        }

        public static int NormalizeGrad(int grad)
        {
            var c = Mathf.FloorToInt(grad / 360);
            grad = grad - c * 360;
            return grad;
        }
    }
}
