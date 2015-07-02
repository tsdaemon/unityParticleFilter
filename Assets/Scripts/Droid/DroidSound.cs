
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DroidSound : MonoBehaviour
{
    public AudioClip success;
    public AudioClip scream;
    public AudioClip[] randomSounds;

    private AudioSource audio;
    private float playedAgo;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        playedAgo += Time.deltaTime;
    }

    public void PlayRandom()
    {
        if (playedAgo < 3)
            return;
        playedAgo = 0;
        var clip = randomSounds[Random.Range(0, randomSounds.Length - 1)];
        audio.clip = clip;
        audio.Play();
    }

    public void PlaySuccess()
    {
        audio.clip = success;
        audio.Play();
        playedAgo = 0;
    }

    public void PlayScream()
    {
        audio.clip = scream;
        audio.Play();
        playedAgo = 0;
    }
}

