using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    class ParticlesMap
    {
        public float[,,] Probabilities;
        private int length;
        private int width;

        public ParticlesMap(int _length, int _width)
        {
            length = _length;
            width = _width;
            Probabilities = new float[length,width,4];
        }

        public void Normalize()
        {
            var all = 0f;
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        all += Probabilities[i, j, k];
                    }
                }
            }
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        Probabilities[i, j, k] /= all;
                    }
                }
            }
        }

        public void Shift(MatrixPosition shift)
        {
            var np = new float[length, width, 4];
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var newI = i + shift.x;
                    var newJ = j + shift.z;
                    if (!(newI < 0 || newI >= length || newJ < 0 || newJ >= width))
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            var newK = DiscreeteAngleOperations.Summ(k, shift.k);
                            np[newI, newJ, newK] = Probabilities[i, j, k];
                        }
                    }
                }
            }
            Probabilities = np;
        }

        internal void Threshold(float f)
        {
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        if (Probabilities[i, j, k] < f)
                        {
                            Probabilities[i, j, k] = 0f;
                        }
                    }
                }
            }
        }
    }
}
