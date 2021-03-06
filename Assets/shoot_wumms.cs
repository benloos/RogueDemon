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
    private AudioSource audio;
    private PlayerController playerController;
    private Quaternion upRecoil = Quaternion.Euler(7f,-180f,90f);
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 RecoilPosition;
    public bool AKTIVIEREN;
    public int ammo = 0;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        attackPoint.localPosition.Set(0.015f, 0, -0.788f);
        originalRotation = transform.localRotation;
        playerController = player.GetComponent<PlayerController>(); 
        originalPosition = transform.localPosition;
        RecoilPosition.Set(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.04f);
        timer = 1.0f / (float)playerController.Firerate;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= (1.0f / (float)playerController.Firerate)){
            Debug.Log("times up");
        }
        if(ammo > 0){
            if(timer >= (1.0f / (float)playerController.Firerate)*2){
                if (Input.GetKeyDown(KeyCode.Mouse0)){
                timer = 0;
                    MuzzleFlash_Front.Play();
                    MuzzleFlash_Left.Play();
                    MuzzleFlash_Right.Play();
                    shoot();
                    ammo--;
                    RecoilUp();
                }else if(Input.GetKeyUp(KeyCode.Mouse0)){
                    RecoilDown();
                }
            }
        }
        timer += Time.deltaTime;
    }

    //spaghettiiiiiiii Code
    private void shoot(){
        //hit position finden
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        //Debug.DrawRay(cam.transform.position, new Vector3(0.5f,0.5f,0), Color.cyan , 5);
        if (Physics.Raycast(ray, out hit)){
            targetPoint = hit.point;
           
        }else{
            targetPoint = ray.GetPoint(75); //Just a point far away from the player
        }

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        //Debug.DrawRay(attackPoint.position, directionWithoutSpread, Color.blue);
        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

                //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        audio.Play();
        StartCoroutine(playerController.Shake(0.15f, 0.1f));
    }

    private void RecoilUp(){
        originalPosition = transform.localPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, RecoilPosition, 0.5f);
    }

    private void RecoilDown(){
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
    public void add_ammo(){
        Debug.Log("sniper_script triggered");
        ammo += 10;
    }
    
}
