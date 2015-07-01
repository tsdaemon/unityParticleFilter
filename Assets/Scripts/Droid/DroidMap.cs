using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using UnityEngine;
using Particle = Assets.Scripts.Models.Particle;

public class DroidMap : MonoBehaviour
{
    private GameObject labyrinth;
    private MinimapController minimap;
    private int[,] walls;
    private ParticlesMap particlesMap;
    // subsystems
    private DroidLaser laser;

    void Awake()
    {
        // connect to the outer data
        labyrinth = GameObject.FindGameObjectWithTag("Labyrinth");
        var c = labyrinth.GetComponent<LabyrinthController>();
        walls = c.GetMap();

        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<MinimapController>();
        // connect to subsystems
        laser = GetComponent<DroidLaser>();
        
        // create first particles, for 3*3 for each unit
        var length = walls.GetUpperBound(0) + 1;
        var width = walls.GetUpperBound(1) + 1;
        var particlesOnUnit = 3;
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
        minimap.ShowParticles(particlesMap.Particles, 1f);
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

    void Update()
    {
        if (Input.GetButton("Scan") && !laser.isScanning)
        {
            var scan = laser.Scan();
            //Debug.Log(lastScan.ToString());
            RecreateParticles(scan);
        }
    }

    // on this step every particle creates again with probability of it weight
    public void RecreateParticles(LaserData data)
    {
        var bestParticle = new Particle();
        // reweight particles
        foreach (var p in particlesMap.Particles)
        {
            var convolution = LaserHelper.ScanPoint(p.position, 0).Convolve(data);
            p.direction = convolution.direction;
            p.probablity = convolution.probablity;
            if (bestParticle.probablity < p.probablity)
            {
                bestParticle = p;
            }
        }
        particlesMap.Normalize();
        // next recreate particles with a probabilities
        particlesMap.Resample(bestParticle);
        // update minimap
        minimap.ShowParticles(particlesMap.Particles, bestParticle.probablity*2);
    }

    public void ShiftPaticles(MoveModel model)
    {
        // shift particles
        particlesMap.Shift(model);
        // set zero probability for particles, which shifts in a wall
        foreach (var p in particlesMap.Particles)
        {
            var x = Mathf.FloorToInt(p.position.x);
            var z = Mathf.FloorToInt(p.position.z);

            if (walls[x, z] == 1)
            {
                p.probablity = 0;
            }
        }
        // shift gameobjects
        StartCoroutine(minimap.ShowParticles(particlesMap.Particles));
    }
}

