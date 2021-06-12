using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 1f;
    // Start is called before the first frame update
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f), Time.deltaTime * speed, Space.World);
    }
}
