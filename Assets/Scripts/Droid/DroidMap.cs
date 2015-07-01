using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

public class DroidMap : MonoBehaviour
{
    private GameObject labyrinth;
    private MatrixPosition targetPosition;
    private LaserData[,,] map;

    private ParticlesMap particles;
    private float bestpr = 1f;
    private MinimapController minimap;
    private AStarGraph graph;



    void Awake()
    {
        // get data from labyrinth
        labyrinth = GameObject.FindGameObjectWithTag("Labyrinth");
        var c = labyrinth.GetComponent<LabyrinthController>();
        targetPosition = c.GetTargetPosition();
        map = c.GetScan();
        var intMap = c.GetMap();
        // init particles
        var length = map.GetUpperBound(0) + 1;
        var width = map.GetUpperBound(1) + 1;
        particles = new ParticlesMap(length,width);
        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (intMap[i,j] == 0)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        particles.Probabilities[i, j, k] = 1;
                    }
                }
            }
        }
        particles.Normalize();
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapController>();
        minimap.ShowParticles(particles.Probabilities, 1f);
        // generate A* graph model
        graph = new AStarGraph(intMap);
    }

    public MatrixPosition AnalyzeData(LaserData data)
    {
        // next reweight particles
        var bestPosition = new MatrixPosition();
        bestpr = 0f;
        for (var i = 0; i < map.GetUpperBound(0) + 1; i++)
        {
            for (var j = 0; j < map.GetUpperBound(1) + 1; j++)
            {
                for (var k = 0; k < 4; k++)
                {
                    if (map[i, j, 0] != null)
                    {
                        // set particle weight to the scalar product
                        particles.Probabilities[i, j, k] *= map[i, j, k]*data;

                        if (particles.Probabilities[i, j, k] > bestpr)
                        {
                            bestpr = particles.Probabilities[i, j, k];
                            bestPosition.x = i;
                            bestPosition.z = j;
                            bestPosition.k = k; // direction
                        }
                    }
                    else
                    {
                        particles.Probabilities[i, j, k] = 0;
                    }
                }
            }
        }
        particles.Threshold(bestpr / 10);
        particles.Normalize();
        bestpr = particles.Probabilities[bestPosition.x, bestPosition.z, bestPosition.k];
        // update minimap
        minimap.ShowParticles(particles.Probabilities, bestpr);
        // return position with the most probability
        return bestPosition;
    }

    internal Path GetPath(MatrixPosition start)
    {
        var path = graph.GeneratePath(start, targetPosition);

        minimap.RenderPath(path);

        return path;
    }

    public void ShiftPaticles(MatrixPosition matrixPosition)
    {
        // shift particles
        particles.Shift(matrixPosition);
        // rerender particles
        minimap.ShowParticles(particles.Probabilities, bestpr);
    }
}

