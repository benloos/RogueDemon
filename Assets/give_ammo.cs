using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class give_ammo : MonoBehaviour
{
    [SerializeField] GameObject sniper;
    private shoot_wumms sniper_script;
    
    // Update is called once per frame
    void Start(){
        sniper_script = sniper.GetComponent<shoot_wumms>();
    }

    private void OnTriggerEnter(Collider collider){
        Debug.Log(collider.gameObject.tag.ToString());
        if(collider.gameObject.tag.Equals("Player")){
            sniper_script.add_ammo();
            Debug.Log(sniper_script.ammo.ToString());
            Destroy(this.gameObject);
        }
    }
}
