using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using UnityEngine;
using Particle = Assets.Scripts.Models.Particle;

public class DroidMap : MonoBehaviour
{
    private LabyrinthController labyrinth;
    private MinimapController minimap;
    private int[,] walls;
    private ParticlesMap particlesMap;
    // subsystems
    private DroidLaser laser;

    void Awake()
    {
        // connect to the outer data
        labyrinth = GameObject.FindGameObjectWithTag("Labyrinth").GetComponent<LabyrinthController>();
        walls = labyrinth.GetMap();
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapController>();

        // connect to subsystems
        laser = GetComponent<DroidLaser>();
        
        // create first particles, for 3*3 for each unit
        var length = labyrinth.length;
        var width = labyrinth.width;
        var particlesOnUnit = 1;
        var particlesShift = CreateParticlesShift(particlesOnUnit);
        var ls = new List<Particle>();
        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (walls[i, j] == 0)
                {
                    // nine particles for every unit
                    for (var k = 0; k < particlesOnUnit*particlesOnUnit; k++)
                    {
                        var particle = new Particle
                        {
                            probablity = 1,
                            position = new Vector3(i, 1f, j) + particlesShift[k]
                        };
                        ls.Add(particle);
                    }
                }
            }
        }

        // create particles map
        particlesMap = new ParticlesMap(ls);
        particlesMap.Normalize();
        StartCoroutine(minimap.ShowParticles(particlesMap.Particles));
    }

    private Vector3[] CreateParticlesShift(int particlesOnUnit)
    {
        var d = 1.0f/(particlesOnUnit);
        var vectorValues = new Vector3[particlesOnUnit*particlesOnUnit];
        var vectorShift = new Vector3(d/2, 0, d/2);

        for (var i = 0; i < particlesOnUnit; i++)
        {
            for (var j = 0; j < particlesOnUnit; j++)
            {
                vectorValues[i + j * particlesOnUnit] = new Vector3(i * d, 0f, j * d) + vectorShift;
            }
        }
        return vectorValues;
    }

    // on move 
    public void OnMove(MoveModel model)
    {
        //float time = Time.realtimeSinceStartup;
        // on move shift all particles in move direction
        ShiftParticles(model.shift);
        //time = Time.realtimeSinceStartup - time;
        //Debug.Log("ShiftParticles execution time: " + time);
        //time = Time.realtimeSinceStartup;
        // next weight all particles according to new scan data
        WeightParticles();
        //time = Time.realtimeSinceStartup - time;
        //Debug.Log("WeightParticles execution time: " + time);
        //time = Time.realtimeSinceStartup;
        // next recreate particles with a probabilities
        Resample(model.tolerance);
        //time = Time.realtimeSinceStartup - time;
        //Debug.Log("Resample execution time: " + time);
        // in the end start particles rerender
        StartCoroutine(minimap.ShowParticles(particlesMap.Particles));
    }

    private void Resample(float tolerance)
    {
        particlesMap.Resample(tolerance);
        particlesMap.Normalize();
    }

    private void WeightParticles()
    {
        var data = laser.Scan();
        var pBest = new Particle();
        foreach (var p in particlesMap.Particles)
        {
            var convolution = LaserHelper.ScanPoint(p.position)*data;
            //p.direction = (int)convolution.topPosition;

            p.probablity = CalculateProbabilityOnProductResult(convolution);
            if (p.probablity > pBest.probablity)
            {
                pBest = p;
            }
        }
    }

    private float CalculateProbabilityOnProductResult(float r)
    {
        var p = 0f;
        var threshold = 0.4f;
        if (r > threshold)
        {
            p = 1.028f * Mathf.Pow(r, 3) - 0.028f;
        }
        return p;
    }

    private void ShiftParticles(Vector3 shift)
    {
        particlesMap.Shift(shift);
        // set zero probability for particles, which shifts in a wall
        foreach (var p in particlesMap.Particles)
        {
            var x = Mathf.FloorToInt(p.position.x);
            var z = Mathf.FloorToInt(p.position.z);
            
            if (x < 0 || x >= labyrinth.length || z < 0 || z >= labyrinth.width || walls[x, z] == 1)
            {
                p.probablity = 0;
            }
        }
    }
}

