using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Image healthBar;
    public GameObject player;
    private PlayerController pc;

    private void Start()
    {
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = pc.HP / pc.maxHP;
    }
}
