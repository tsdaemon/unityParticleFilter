using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generators
{
    class DFGenerator : LabyrinthGenerator
    {
        public override void Generate(int[,] map, int length, int width)
        {
            int xstart;
            int zstart;
            do
            {
                xstart = Random.Range(0, length - 1);
                zstart = Random.Range(0, width - 1);
            } while (map[xstart, zstart] == 1);

            var path = new List<int[]> {new[] {xstart, zstart}};
            CreateMaze(path, map, length, width);
        }

        private void CreateMaze(List<int[]> path, int[,] map, int length, int width)
        {
            // create wall on last path position
            var last = GetLast(path);
            var x = last[0];
            var z = last[1];
            CreateWall(x, z);
            map[x, z] = 1;
            // find free points around or go back and find another free directions
            do
            {
                last = GetLast(path);
                var freePoints = GetFreePoints(map, last[0], last[1], length, width);
                if (freePoints.Count > 0)
                {
                    var i = Random.Range(0, freePoints.Count - 1);
                    var fr = freePoints[i];
                    path.Add(fr);
                    CreateMaze(path, map, length, width);
                    break;
                } 
                path.RemoveAt(path.Count - 1);

            } while (path.Count > 0);
        }

        private int[][] nextPoints = 
        {
            new []{-1, 0},
            new []{0,-1},
            new []{1,0},
            new []{0,1}
        };
        private List<int[]> GetFreePoints(int[,] map, int x, int z, int length, int width)
        {
            var ls = new List<int[]>();
            for (var i = 0; i < 4; i++)
            {
                var next = nextPoints[i];
                var newX = next[0] + x;
                var newZ = next[1] + z;

                if (newX < 0 || newZ < 0 || newX > length - 1 || newZ > width - 1 || map[newX,newZ] == 1)
                {
                    continue;
                }

                var x1 = newX > x ? newX : newX < x ? newX + next[0] : x - 1;
                x1 = x1 < 0 ? 0 : x1;
                var x2 = newX > x ? newX+ next[0] : newX < x ? newX : x + 1;
                x2 = x2 > length-1 ? length-1 : x2;

                var z1 = newZ > z ? newZ : newZ < z ? newZ + next[1] : z - 1;
                z1 = z1 < 0 ? 0 : z1;
                var z2 = newZ > z ? newZ + next[1] : newZ < z ? newZ : z + 1;
                z2 = z2 > width-1 ? width-1 : z2;

                if (CheckFree(map, x1, x2, z1, z2))
                {
                    ls.Add(new[] {newX, newZ});
                }
            }
            return ls;
        }
        private bool CheckFree(int[,] map, int x1, int x2, int z1, int z2)
        {
            for (var i = x1; i <= x2; i++)
            {
                for (var j = z1; j <= z2; j++)
                {
                    if (map[i, j] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreateWall(int x, int z)
        {
            var y = prefab.transform.localScale.y / 2;
            var o = Instantiate(prefab);
            o.transform.SetParent(transform);
            o.transform.localPosition = new Vector3(x+0.5f, y, z+0.5f);
        }

        private int[] GetLast(List<int[]> list)
        {
            return list[list.Count - 1];
        }
    }
}
