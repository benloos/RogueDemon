using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject weapon;
    [SerializeField] Camera camera;
    private PlayerController pc;
    [SerializeField] private ParticleSystem particleBullet;
    [SerializeField] private ParticleSystem particleBulletShells;
    [SerializeField] private ParticleSystem particleHit_brown;
    [SerializeField] private ParticleSystem particleHit_red;
    private Light pointlight;
    private AudioSource schuss_sound;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private int selectedShootSound = 0;
    float timer;

    void Start(){
        pc = GameManager.current.player.GetComponent<PlayerController>();
        timer = 1.0f / (float)pc.Firerate;
        pointlight = particleBullet.GetComponentInChildren<Light>();
        schuss_sound = GetComponent<AudioSource>();
        schuss_sound.clip = sounds[selectedShootSound];
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
            particleBullet.Play();
            particleBulletShells.Play();
            schuss_sound.pitch = Random.Range(0.9f, 1.2f);
            schuss_sound.Play();
            //pointlight.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, transform.right, out hit))
            {
                //Debug.Log(hit.transform.name);
                if ((hit.transform.tag == "ZombieP") || hit.transform.tag == "Imp")
                {
                    EnemyAI script = hit.transform.GetComponent<EnemyAI>();
                    script.TakeDamage(pc.DMG);

                    particleHit_brown.transform.position = hit.point;
                    particleHit_brown.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    particleHit_brown.Play();
                    particleHit_red.transform.position = hit.point;
                    particleHit_red.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    particleHit_red.Play();
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
            //Debug.DrawRay(camera.transform.position, transform.right, Color.magenta);
            //Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

}
