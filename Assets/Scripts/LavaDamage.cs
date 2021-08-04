using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public int DamagePerFrame;
    private PlayerController player;
    void Start()
    {
        player = GameManager.current.player.GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Damage(DamagePerFrame);
        }
    }
}
