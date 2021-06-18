using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    private DontDestroyAudio current;

    public AudioClip[] menumusic;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        } else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        AudioSource music = GetComponent<AudioSource>();
        music.clip = menumusic[Random.Range(0,menumusic.Length)];
        music.pitch = Random.Range(0.7f, 1.1f);
        music.Play();
    }
}
