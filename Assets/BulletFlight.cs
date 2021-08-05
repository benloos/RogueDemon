using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlight : MonoBehaviour
{

    private float time = 5.0f;
    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 3.0f);
    }

    void OnTriggerEnter(Collider collision){
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        if(collision.gameObject.transform.name.Contains("Zombie")){
            /**
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.localScale += new Vector3(1,1,1);
            */
            Debug.Log("Hit Zombie");
            EnemyAI script = collision.gameObject.transform.GetComponent<EnemyAI>();
            script.TakeDamage(100);
        }
        
    }
}
