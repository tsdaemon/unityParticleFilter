using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    class ParticlesMap
    {
        public Particle[] Particles;

        public ParticlesMap(List<Particle> ls)
        {
            Particles = ls.ToArray();
        }

        public void Normalize()
        {
            var all = 0f;
            foreach(var p in Particles)
            {
                all += p.probablity;
            }
            foreach (var p in Particles)
            {
                p.probablity /= all;
            }
        }

        public void Shift(Vector3 shift)
        {
            foreach (var p in Particles)
            {
                p.position += shift;
            }
        }

        internal void Resample(float randomRadius)
        {
            var list = new List<Particle>();
            var i = 0;
            while (i < Particles.Length)
            {
                foreach (var p in Particles)
                {
                    if (p.probablity > 0f && Random.Range(0f, 1f) <= p.probablity)
                    {
                        var randomShift = randomRadius*Random.insideUnitCircle;
                        var newPosition = p.position + new Vector3(randomShift.x, 0f, randomShift.y);
                        var particle = new Particle {probablity = 1, direction = p.direction, position = newPosition};
                        list.Add(particle);
                        i++;
                        if (i == Particles.Length) break;
                    }
                }
            }
            Particles = list.ToArray();
        }

        public void Threshold(float t)
        {
            foreach (var p in Particles)
            {
                if (p.probablity < t)
                {
                    p.probablity = 0f;
                }
            }
        }
    }
}
