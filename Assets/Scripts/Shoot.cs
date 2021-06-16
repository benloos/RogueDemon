using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject weapon;
    [SerializeField] Camera camera;
    private PlayerController pc;
    private ParticleSystem particle;
    private Light pointlight;
    private AudioSource schuss_sound;
    float timer;

    void Start(){
        pc = GameManager.current.player.GetComponent<PlayerController>();
        timer = 1.0f / (float)pc.Firerate;
        particle = GetComponentInChildren<ParticleSystem>();
        pointlight = particle.GetComponentInChildren<Light>();
        schuss_sound = GetComponent<AudioSource>();
        //pointlight.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && timer >= (1.0f / (float)pc.Firerate))
        {
            timer = 0;
            //spawn Raycast
            //illuminate Raycast
            particle.Play();
            schuss_sound.Play();
            //pointlight.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, transform.right, out hit))
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform.tag == "ZombieP")
                {
                    EnemyAI script = hit.transform.GetComponent<EnemyAI>();
                    script.TakeDamage(pc.DMG);
                }

            }
            //pointlight.enabled = false;
        }
        timer += Time.deltaTime;
    }

    private void OnDrawGizmos(){
        RaycastHit hit;
        Gizmos.color = Color.magenta;
        if(Physics.Raycast(camera.transform.position,transform.right, out hit)){
            //Debug.Log(hit.transform.name);
            Debug.DrawRay(camera.transform.position, transform.right, Color.magenta);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

}
