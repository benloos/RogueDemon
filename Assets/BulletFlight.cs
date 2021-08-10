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
        if (collision.gameObject.transform.tag == "ZombieP")
            {
                EnemyAI script = collision.gameObject.transform.GetComponent<EnemyAI>();
                script.TakeDamage(450);
            }
            else if (collision.gameObject.transform.tag == "Imp")
            {
                EnemyAIRanged script = collision.gameObject.transform.GetComponent<EnemyAIRanged>();
                script.TakeDamage(450);
            }
            else if (collision.gameObject.transform.tag == "Boss")
            {
                BossAI script = collision.gameObject.transform.GetComponent<BossAI>();
                script.TakeDamage(450);
            }
        }
}
