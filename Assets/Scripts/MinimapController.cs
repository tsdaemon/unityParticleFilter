using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Particle = Assets.Scripts.Models.Particle;

public class MinimapController : MonoBehaviour 
{
    public GameObject particleObj;

    private Dictionary<Particle, GameObject> particlesObjs = new Dictionary<Particle, GameObject>();
    private LabyrinthController labyrinth;

    void Awake()
    {
        labyrinth = GameObject.FindGameObjectWithTag("Labyrinth").GetComponent<LabyrinthController>();
    }

    public IEnumerator ShowParticles(IEnumerable<Particle> particles)
    {
        var count = 0;
        // remove old
        foreach (var particle in particlesObjs)
        {
            Destroy(particle.Value);
            if (count == 10)
            {
                count = 0;
                yield return null;
            }
        }
        particlesObjs.Clear();
        // create new
        foreach (var p in particles)
        {
            if (p.probablity > 0 && IsOnMap(p.position))
            {
                // set to position
                var o = Instantiate(particleObj);
                o.transform.position = p.position;
                // set rotation
                var euler = transform.rotation.eulerAngles;
                euler.y = p.direction;
                o.transform.rotation = Quaternion.Euler(euler);
                // color is gradient dependent on probability from red for 0 to green for 1
                var ren = o.GetComponent<Renderer>();
                var color = p.probablity;
                ren.material.color = new Color(1 - color, color, 0f);
                // add to the list
                particlesObjs.Add(p,o);
                count++;
                if (count == 10)
                {
                    count = 0;
                    yield return null;
                }
            }
        }
    }

    private bool IsOnMap(Vector3 v)
    {
        return v.x > 0 && v.z > 0 && v.z < labyrinth.width && v.x < labyrinth.width;
    }
}
