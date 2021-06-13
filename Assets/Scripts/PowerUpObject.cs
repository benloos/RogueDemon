using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    PlayerController pc;

    public enum PowerUpType{ 
        HpUp,
        Heal,
        DamageUp,
        FirerateUp
    };

    [Header("Typ des Powerups.")]
    public PowerUpType type;
    [Header("Stärke wird drauf addiert, Firerate wird damit multipliziert.")]
    public int amount = 10;

    private void Start()
    {
        pc = GameManager.current.player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (type == PowerUpType.HpUp)
            {
                pc.maxHP += amount;
            } 
            else if (type == PowerUpType.Heal)
            {
                pc.Heal(amount);
            }
            else if (type == PowerUpType.DamageUp)
            {
                pc.DMG += amount;
            }
            else if (type == PowerUpType.FirerateUp)
            {
                pc.Firerate *= amount;
            }
        }
    }
}
