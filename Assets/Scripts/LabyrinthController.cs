using UnityEngine;
using System.Collections;
using Assets.Scripts.Generators;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;

public class LabyrinthController : MonoBehaviour
{
    public int length;
    public int width;

    private int[,] collisionMap;

    void Awake ()
    {
        // create map
        collisionMap = new int[width, length];
        var generators = GetComponents<LabyrinthGenerator>();
        foreach (var generator in generators)
        {
            generator.Generate(collisionMap, length, width);
        }
    }

    public Vector3 GetRandomFreePosition()
    {
        var x = 0;
        var z = 0;
        do
        {
            x = Random.Range(1, length - 2);
            z = Random.Range(1, width - 2);
        } while (collisionMap[x, z] == 1);
        return new Vector3(x,0,z);
    }

    internal int[,] GetMap()
    {
        return collisionMap;
    }
}
