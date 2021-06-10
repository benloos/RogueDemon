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
    }

    // Update is called once per frame
    void Update()
    {
        health.text = pc.HP.ToString();
    }
}
