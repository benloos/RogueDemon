using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot_wumms : MonoBehaviour
{
    [SerializeField] private ParticleSystem MuzzleFlash_Front;
    [SerializeField] private ParticleSystem MuzzleFlash_Left;
    [SerializeField] private ParticleSystem MuzzleFlash_Right;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){

            MuzzleFlash_Front.Play();
            MuzzleFlash_Left.Play();
            MuzzleFlash_Right.Play();
            spawn_projectile();
        }
    }

    private void spawn_projectile(){
        //Vector3 local_position = new Vector3(0.0171073f, -0.00849f, -0.836298f);
        Vector3 worldposition = transform.TransformPoint(Vector3.forward * 1.5f);
        Instantiate(projectile, worldposition, Quaternion.identity);
        //temp.transform.localPosition = local_position;
    }

    private void shoot(){
        //hit position finden
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
    }
}
