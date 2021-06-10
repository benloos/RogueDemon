using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooT : MonoBehaviour
{

    [SerializeField] GameObject Projectile;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Debug.Log("Schuss");
        }
    }
}
