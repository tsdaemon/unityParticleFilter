using UnityEngine;
using System.Collections;
using Assets.Scripts.Models;

public class GameController : MonoBehaviour {

    public GameObject targetPrefab;
    public GameObject droidPrefab;
    public SceneFadeInOut fadeInOut;
    public int nextLevel;

    private GameObject droid;
    private GameObject target;
    private LabyrinthController labyrinth;

	void Start ()
	{
        labyrinth = GameObject.FindGameObjectWithTag("Labyrinth").GetComponent<LabyrinthController>();
        // create droid
        var free = labyrinth.GetRandomFreePosition();
        droid = Instantiate(droidPrefab);
        droid.transform.parent = transform;
        droid.transform.position = new Vector3(free.x + 0.5f, 1, free.z + 0.5f);
        droid.layer = 8; // Player
        droid.GetComponent<DroidSound>().PlayScream();
        // create target
	    do
	    {
	        free = labyrinth.GetRandomFreePosition();
	    } while (free == droid.transform.position);
        target = Instantiate(targetPrefab);
        target.transform.parent = transform;
        target.transform.position = new Vector3(free.x, 0f, free.z);
	}

    void Update()
    {
        // check game complete
        if ((droid.transform.position - target.transform.position).sqrMagnitude < 4)
        {
            droid.GetComponent<DroidSound>().PlaySuccess();
            fadeInOut.EndScene(nextLevel);
        }
    }
}
