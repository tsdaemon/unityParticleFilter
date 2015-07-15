using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Generators
{
    public abstract class LabyrinthGenerator : MonoBehaviour
    {
        public GameObject prefab;
        public abstract void Generate(int[,] map, int length, int width);
    }
}
