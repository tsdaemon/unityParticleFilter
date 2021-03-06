﻿using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class LaserHelper
    {
        private const int SCANNING_ANGLES = 12;

        // scanning goes over angles relative to y as axis of rotation, so x = 1, z = 0 it is 90 grads.
        public static LaserData ScanPoint(Vector3 point)
        {
            LaserData data = new LaserData();
            for (var i = 0; i < SCANNING_ANGLES; i++)
            {
                var angleGrad = 360/SCANNING_ANGLES*i;
                if (angleGrad >= 360) angleGrad -= 360;

                var angle = angleGrad*Mathf.PI/180;
                var directionX = Mathf.Sin(angle);
                var directionZ = Mathf.Cos(angle);
                var ray = new Ray()
                {
                    origin = point,
                    direction = new Vector3(directionX, 0f, directionZ)
                };
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                data[angleGrad] = hit.distance;
            }
            return data.Normalize();
        }
    }
}
