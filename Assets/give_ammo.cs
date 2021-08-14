using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class give_ammo : MonoBehaviour
{
    [SerializeField] GameObject sniper;
    private shoot_wumms sniper_script;
    ParticleSystem ps;
    AudioSource pickup_sound;

    // Update is called once per frame
    void Start(){
        sniper_script = sniper.GetComponent<shoot_wumms>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        pickup_sound = ps.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider){
        Debug.Log(collider.gameObject.tag.ToString());
        if(collider.gameObject.tag.Equals("Player")){
            pickup_sound.Play();
            sniper_script.add_ammo();
            Debug.Log(sniper_script.ammo.ToString());
            Destroy(this.gameObject);
            StartCoroutine(destroyParticleAfter(1));
        }
    }

    IEnumerator destroyParticleAfter(float s)
    {
        yield return new WaitForSeconds(s);
        Destroy(ps);
    }
}
