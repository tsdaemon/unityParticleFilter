using UnityEngine;

namespace Assets.Scripts.Generators
{
    class RandomLabyrinthGenerator : LabyrinthGenerator
    {
        [Range(0,1)]
        public float complexity;

        public override void Generate(int[,] map, int length, int width)
        {
            var y = prefab.transform.localScale.y / 2;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i, j] == 1) continue;
                    // get random to decide whether build wall or not
                    var random = Random.Range(0f, 1f);
                    if (random < complexity)
                    {
                        map[i, j] = 1;
                        var x = i + 0.5f;
                        var z = j + 0.5f;

                        var o = Instantiate(prefab);
                        o.transform.SetParent(transform);
                        o.transform.localPosition = new Vector3(x, y, z);
                    }
                }
            }
        }
    }
}
