using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;

public class MinimapController : MonoBehaviour 
{
    public GameObject particleObj;
    public GameObject pathObj;

    private IList<GameObject> particlesObjs = new List<GameObject>();
    private Vector2[] directionParticlesShift =
    {
        new Vector2(0f, 0.3f), 
        new Vector2(0.3f, 0f), 
        new Vector2(0f, -0.3f),
        new Vector2(-0.3f, 0)
    };

    public void ShowParticles(float[,,] particles, float best)
    {
        // remove old
        foreach (var particle in particlesObjs)
        {
            Destroy(particle);
        }
        particlesObjs.Clear();
        // create new
        for (var i = 0; i < particles.GetUpperBound(0) + 1; i++)
        {
            for (var j = 0; j < particles.GetUpperBound(1) + 1; j++)
            {
                for (var k = 0; k < 4; k++)
                {
                    var probability = particles[i, j, k];
                    if (probability > 0)
                    {
                        // set to position
                        var o = Instantiate(particleObj);
                        o.transform.parent = transform;
                        var shift = directionParticlesShift[k];
                        o.transform.position = new Vector3(i + 0.5f + shift.x, 0.5f, j + 0.5f + shift.y);
                        // color is gradient dependent on probability from red for 0 to green for 1
                        var ren = o.GetComponent<Renderer>();
                        probability = probability/best;
                        ren.material.color = new Color(1 - probability, probability, 0f);
                        // add to the list
                        particlesObjs.Add(o);
                    }
                }
            }
        }
    }

    private IList<GameObject> pathObjs = new List<GameObject>();
    internal void RenderPath(Path path)
    {
        foreach (var p in pathObjs)
        {
            Destroy(p);
        }
        pathObjs.Clear();
        // clear previous path
        foreach (var p in path.actions)
        {
            // set to position
            var o = Instantiate(pathObj);
            o.transform.parent = transform;
            var shift = directionParticlesShift[p.To.position.k];
            o.transform.position = new Vector3(p.To.position.x + 0.5f + shift.x, 0.8f, p.To.position.z + 0.5f + shift.y);
            // color is gradient dependent on probability from red for 0 to green for 1
            var ren = o.GetComponent<Renderer>();
  
            ren.material.color = Color.magenta;
            // add to the list
            pathObjs.Add(o);
        }
    }
}
