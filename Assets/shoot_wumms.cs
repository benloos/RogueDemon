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
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private Quaternion upRecoil = Quaternion.Euler(7f,-180f,90f);
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 RecoilPosition;
    public bool AKTIVIEREN;
    public int munition = 10;
    // Start is called before the first frame update
    void Start()
    {
        attackPoint.localPosition.Set(0.015f, 0, -0.788f);
        originalRotation = transform.localRotation;
        playerController = player.GetComponent<PlayerController>(); 
        originalPosition = transform.localPosition;
        RecoilPosition.Set(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.04f);
    }

    // Update is called once per frame
    void Update()
    {
        if(munition < 0){
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                MuzzleFlash_Front.Play();
                MuzzleFlash_Left.Play();
                MuzzleFlash_Right.Play();
                shoot();
                munition--;
                RecoilUp();
            }else if(Input.GetKeyUp(KeyCode.Mouse0)){
                RecoilDown();
            }
        }
        //TODO WENN SNIPER AUFGESAMMELT WURDE CALL add_ammo()
    }

    //spaghettiiiiiiii Code
    private void shoot(){
        //hit position finden
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        Debug.DrawRay(cam.transform.position, new Vector3(0.5f,0.5f,0), Color.cyan , 5);
        if (Physics.Raycast(ray, out hit)){
            targetPoint = hit.point;
           
        }else{
            targetPoint = ray.GetPoint(75); //Just a point far away from the player
        }

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        Debug.DrawRay(attackPoint.position, directionWithoutSpread, Color.blue);
        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

                //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        //StartCoroutine(playerController.Shake(0.15f, 0.1f));
    }

    private void RecoilUp(){
        /*
        while (transform.localRotation.eulerAngles.x != 7){
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, upRecoil, Time.deltaTime * 5);
            Debug.Log(transform.localRotation);
        }
        */
        originalPosition = transform.localPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, RecoilPosition, 0.5f);
    }

    private void RecoilDown(){
        /*
        while (transform.localRotation.eulerAngles.x != 0){
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, Time.deltaTime * 5);
        }
        */
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, 0.5f);
    }
    private void OnDrawGizmos(){
        RaycastHit hit;
        Gizmos.color = Color.green;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward , out hit)){
            //Debug.Log(hit.transform.name);
            Debug.DrawRay(cam.transform.position, transform.right, Color.green);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

    private void add_ammo(){
        munition += 10;
    }
}
