using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Image healthBar;
    private PlayerController pc;

    private void Start()
    {
        pc = GameManager.current.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)pc.HP / (float)pc.maxHP;
    }
}
