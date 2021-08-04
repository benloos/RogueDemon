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
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float shootForce;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("AttackPosition: " + attackPoint.localPosition);
        attackPoint.localPosition.Set(0.015f, 0, -0.788f);
        Debug.Log("AttackPosition: " + attackPoint.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            MuzzleFlash_Front.Play();
            MuzzleFlash_Left.Play();
            MuzzleFlash_Right.Play();
            shoot();
        }
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

        Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        Debug.Log("AttackPosition: " + attackPoint.localPosition);
        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

                //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
    }
}
