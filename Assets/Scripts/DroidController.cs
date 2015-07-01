using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Models;

public class DroidController : MonoBehaviour
{
    // subsystems
    private DroidLaser laser;
    private LaserData lastScan;

    private DroidMap map;
    private MatrixPosition bestPosition;
    private Path currentPath;
    private int step = 0;

    private DroidMoving move;

    void Start()
    {
        laser = GetComponent<DroidLaser>();
        map = GetComponent<DroidMap>();
        move = GetComponent<DroidMoving>();
    }

    void Update()
    {
        if (!move.isMoving)
        {
            if (Input.GetButton("Scan") && !laser.isScanning)
            {
                lastScan = laser.Scan();
                Debug.Log(lastScan.ToString());
            }
            if (Input.GetButton("Go") && currentPath != null)
            {
                if (step < currentPath.actions.Count)
                {
                    var action = currentPath.actions[step];
                    var from = action.From.position;
                    var to = action.To.position;
                    if (from.x == to.x && from.z == to.z)
                    {
                        // rotate
                        var direction = from.k - to.k;
                        if (direction == 3) direction = -1;
                        if (direction == -3) direction = 1;
                        if (direction > 0)
                        {
                            move.RotateCCW();
                        }
                        else if (direction < 0)
                        {
                            move.RotateCW();
                        }
                        else
                        {
                            throw new Exception("Direction difference incorrect");
                        }
                    }
                    else
                    {
                        move.MoveForward();
                    }
                    map.ShiftPaticles(to - from);
                    step++;
                }
            }
        }
        if (Input.GetButton("Analyze") && lastScan != null)
        {
            bestPosition = map.AnalyzeData(lastScan);
        }
        if (Input.GetButton("Path") && bestPosition != null)
        {
            currentPath = map.GetPath(bestPosition);
            step = 0;
        }
    }
}
