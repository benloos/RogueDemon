using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBarSlider : MonoBehaviour
{
    private PlayerController pc;
    private Image dashBar;
    private bool startedDash;

    private void Start()
    {
        startedDash = false;
        dashBar = GetComponent<Image>();
        pc = GameManager.current.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.isDashing)
        {
            if (!startedDash)
            {
                Invoke(nameof(ResetDash), pc.dashCD);
                startedDash = true;
                dashBar.fillAmount = 0f;
            }
        }
        if (startedDash)
        {
            dashBar.fillAmount += Time.deltaTime/pc.dashCD;
        }
    }

    private void ResetDash()
    {
        startedDash = false;
    }
}
