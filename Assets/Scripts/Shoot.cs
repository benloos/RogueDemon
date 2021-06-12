using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject weapon;
    private ParticleSystem particle;


    void Start(){
        particle = GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            //spawn Raycast
            //illuminate Raycast
            particle.Play();
            
        }
    }

    private void OnDrawGizmos(){
        RaycastHit hit;
        Gizmos.color = Color.cyan;
        if(Physics.Raycast(weapon.transform.position,transform.right, out hit)){
            //Debug.Log(hit.transform.name);
            Debug.DrawRay(weapon.transform.position, transform.right, Color.cyan);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
