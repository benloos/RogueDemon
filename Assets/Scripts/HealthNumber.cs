using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthNumber : MonoBehaviour
{
    public TMP_Text health;
    private PlayerController pc;

    private void Start()
    {
        pc = GameManager.current.player.GetComponent<PlayerController>();
        health.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.text != pc.HP.ToString())
        {
            if (LeanTween.isTweening(gameObject)) 
            { 
                LeanTween.cancel(gameObject);
                gameObject.transform.localScale = Vector3.one;
            }
            LeanTween.scale(gameObject, Vector3.one * 2, 1f).setEasePunch();
            health.text = pc.HP.ToString();
            health.color = new Vector4(1, ((float)pc.HP / (float)pc.maxHP), ((float)pc.HP / (float)pc.maxHP), 1);
        }
    }
}
