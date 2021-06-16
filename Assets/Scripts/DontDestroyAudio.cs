using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    private DontDestroyAudio current;
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
    }
}
