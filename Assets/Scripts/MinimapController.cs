using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Particle = Assets.Scripts.Models.Particle;


public class MinimapController : MonoBehaviour 
{
    public GameObject particleObj;

    private Dictionary<Particle, GameObject> particlesObjs = new Dictionary<Particle, GameObject>();
    private List<Particle> particles;
    void Update()
    {
        // if shift is not running and particles count is changed, delete old particle markers
        if (particles.Count != particlesObjs.Count && !shiftIsRunning)
        {
            var dc = new Dictionary<Particle, GameObject>();
            foreach (var p in particlesObjs.Keys)
            {
                if (particles.Contains(p))
                {
                    dc.Add(p, particlesObjs[p]);
                }
                else
                {
                    Destroy(particlesObjs[p]);
                }
            }
            particlesObjs = dc;
        }
    }
    public void ShowParticles(List<Particle> _particles, float best)
    {
        particles = _particles;
        // remove old
        foreach (var particle in particlesObjs)
        {
            Destroy(particle.Value);
        }
        particlesObjs.Clear();
        // create new
        foreach (var p in particles)
        {
            if (p.probablity > 0)
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
                var color = p.probablity / best;
                ren.material.color = new Color(1 - color, color, 0f);
                // add to the list
                particlesObjs.Add(p,o);
            }
                    
        }
    }

    private bool shiftIsRunning = true;
    public IEnumerator Shift(List<Particle> _particles)
    {
        var moved = 0;
        particles = _particles;
        shiftIsRunning = true;
        foreach (var p in particles)
        {
            if (particlesObjs.ContainsKey(p))
            {
                var o = particlesObjs[p];
                o.transform.position = p.position;
                moved++;
                if (moved == 100)
                {
                    moved = 0;
                    yield return null;
                }
            }
        }
        shiftIsRunning = false;
    }
}
