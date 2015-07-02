using UnityEngine;
using System.Collections;
using Assets.Scripts.Models;

public class GameController : MonoBehaviour {
    public LabyrinthController labyrinth;
    public GameObject droidPrefab;
    public SceneFadeInOut fadeInOut;

    private GameObject droid;
    private Vector3 targetPosition;

	void Start () 
    {
        var free = labyrinth.GetRandomFreePosition();
        droid = Instantiate(droidPrefab);
        droid.transform.parent = transform;
        droid.transform.position = new Vector3(free.x + 0.5f, 1, free.z + 0.5f);
        droid.layer = 8; // Player
        droid.GetComponent<DroidSound>().PlayScream();

	    targetPosition = labyrinth.GetTargetPosition();
    }

    void Update()
    {
        // check game complete
        if ((droid.transform.position - targetPosition).sqrMagnitude < 4)
        {
            droid.GetComponent<DroidSound>().PlaySuccess();
            fadeInOut.EndScene();
        }
    }
}
