using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public LabyrinthController labyrinth;
    public GameObject droidPrefab;

    private GameObject droid;

	void Start () 
    {
        var free = labyrinth.GetRandomFreePosition();
        droid = Instantiate(droidPrefab);
        droid.transform.parent = labyrinth.transform;
        droid.transform.position = new Vector3(free.x + 0.5f, 1, free.z + 0.5f);
        droid.layer = 8; // Player
	}
}
