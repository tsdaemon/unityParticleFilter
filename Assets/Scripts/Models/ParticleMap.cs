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

        public void Shift(MoveModel move)
        {
            foreach (var p in Particles)
            {
                // move particle using moving probability model
                var randomMove = Random.insideUnitCircle*move.dopusk;
                var shift = new Vector3(move.shift.x + randomMove.x, 0f, move.shift.z + randomMove.y);
                p.position += shift;
            }
        }

        internal void Resample(Particle bestParticle)
        {
            var list = new List<Particle>();
            foreach (var p in Particles)
            {
                var probability = p.probablity/(bestParticle.probablity*1.2);
                if (probability >= Random.Range(0f, 1f))
                {
                    list.Add(p);
                }
            }
            Particles = list;
        }
    }
}
