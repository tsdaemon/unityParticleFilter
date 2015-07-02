using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;

public class LabyrinthController : MonoBehaviour
{
    public GameObject wallGameObject;
    public GameObject targetGameObject;
    public int length;
    public int width;
    [Range(0,1)]
    public float complexity;

    private int[,] collisionMap;
    private Vector3 targetPosition;
    private int xtarget;
    private int ztarget;

    void Awake ()
    {
        var yadd = wallGameObject.transform.localScale.y/2;
        var y = yadd;

        // create map
        collisionMap = new int[width, length];
        CreateWalls(y);
        CreateTarget();
        CreateCollisions(y);
    }

    private void CreateTarget()
    {
        // create target position
        xtarget = Random.Range(2, length - 2);
        ztarget = Random.Range(2, width - 2);

        targetPosition = new Vector3(xtarget, 0, ztarget);
        var o = Instantiate(targetGameObject);
        o.transform.parent = transform;
        o.transform.position = targetPosition;
    }
    private void CreateCollisions(float y)
    {
        // create other
        for (var i = 1; i < length - 1; i++)
        {
            for (var j = 1; j < width - 1; j++)
            {
                // check it is not target position
                if (i == xtarget && j == ztarget)
                {
                    continue;
                }
                // get random to decide whether build wall or not
                var random = Random.Range(0f, 1f);
                if (random < complexity)
                {
                    collisionMap[i, j] = 1;
                    var x = i + 0.5f;
                    var z = j + 0.5f;

                    var o = Instantiate(wallGameObject);
                    o.transform.SetParent(transform);
                    o.transform.localPosition = new Vector3(x, y, z);
                }
            }
        }
    }
    private void CreateWalls(float y)
    {
        // create labyrinth walls on x
        for (var i = 0; i < length; i++)
        {
            var x = i + 0.5f;
            var z1 = 0.5f;
            var z2 = 0.5f + width - 1;
            // set on map
            collisionMap[i, 0] = 1;
            collisionMap[i, width - 1] = 1;
            // create objects

            var o1 = Instantiate(wallGameObject);
            o1.transform.SetParent(transform);
            o1.transform.localPosition = new Vector3(x, y, z1);

            var o2 = Instantiate(wallGameObject);
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
            collisionMap[0, i] = 1;
            collisionMap[length - 1, i] = 1;

            // create objects
            var o1 = Instantiate(wallGameObject);
            o1.transform.SetParent(transform);
            o1.transform.localPosition = new Vector3(x1, y, z);

            var o2 = Instantiate(wallGameObject);
            o2.transform.SetParent(transform);
            o2.transform.localPosition = new Vector3(x2, y, z);
        }
    }

    public MatrixPosition GetRandomFreePosition()
    {
        var x = 0;
        var z = 0;
        do
        {
            x = Random.Range(1, length - 2);
            z = Random.Range(1, width - 2);
        } while (collisionMap[x, z] == 1 || (targetPosition.x == x && targetPosition.z == z));
        return new MatrixPosition{x = x, z = z};
    }
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    internal int[,] GetMap()
    {
        return collisionMap;
    }
}
