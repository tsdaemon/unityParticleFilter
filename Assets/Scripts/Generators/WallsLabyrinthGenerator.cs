using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Generators
{
    class WallsLabyrinthGenerator : LabyrinthGenerator
    {
        public override void Generate(int[,] map, int length, int width)
        {
            var y = prefab.transform.localScale.y / 2;
            // create labyrinth walls on x
            for (var i = 0; i < length; i++)
            {
                var x = i + 0.5f;
                var z1 = 0.5f;
                var z2 = 0.5f + width - 1;
                // set on map
                map[i, 0] = 1;
                map[i, width - 1] = 1;
                // create objects

                var o1 = Instantiate(prefab);
                o1.transform.SetParent(transform);
                o1.transform.localPosition = new Vector3(x, y, z1);

                var o2 = Instantiate(prefab);
                o2.transform.SetParent(transform);
                o2.transform.localPosition = new Vector3(x, y, z2);
            }

            // create labyrinth walls on z
            for (var i = 1; i < width - 1; i++)
            {
                var z = i + 0.5f;
                var x1 = 0.5f;
                var x2 = 0.5f + length - 1;

                // set on map
                map[0, i] = 1;
                map[length - 1, i] = 1;

                // create objects
                var o1 = Instantiate(prefab);
                o1.transform.SetParent(transform);
                o1.transform.localPosition = new Vector3(x1, y, z);

                var o2 = Instantiate(prefab);
                o2.transform.SetParent(transform);
                o2.transform.localPosition = new Vector3(x2, y, z);
            }
        }
    }
}
