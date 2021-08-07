using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sniper_ammo_UI : MonoBehaviour
{
    private shoot_wumms sniper_script;
    [SerializeField] private GameObject sniper;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMP_Text>();
        sniper_script = sniper.GetComponent<shoot_wumms>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sniper.activeSelf){
            text.text = sniper_script.ammo.ToString();
        } else {
            text.text = "";
        }
    }
}
