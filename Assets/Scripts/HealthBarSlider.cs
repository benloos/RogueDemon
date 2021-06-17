using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    private Image healthBar;
    private PlayerController pc;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        pc = GameManager.current.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = Mathf.Round(((float)pc.HP / (float)pc.maxHP) *100f) / 100f;
        if (healthBar.fillAmount < ratio + 0.1f && healthBar.fillAmount > ratio - 0.1f)
        {
            healthBar.fillAmount = ratio;
        } else if (healthBar.fillAmount < ratio)
        {
            healthBar.fillAmount += Time.deltaTime * 2f;
        } else if (healthBar.fillAmount > ratio)
        {
            healthBar.fillAmount -= Time.deltaTime * 2f;
        }
    }
}
