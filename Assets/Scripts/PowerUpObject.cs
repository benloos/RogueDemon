using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    PlayerController pc;
    ParticleSystem ps;
    AudioSource pickup_sound;

    public enum PowerUpType
    {
        HpUp,
        Heal,
        DamageUp,
        FirerateUp
    };

    [Header("Typ des Powerups.")]
    public PowerUpType type;
    [Header("Current Value += Amount.")]
    public int amount = 10;

    [SerializeField] private AudioClip[] pickupSound;
    [SerializeField] private int selectedPickupSound = 0;

    private void Start()
    {
        pc = GameManager.current.player.GetComponent<PlayerController>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        pickup_sound = ps.GetComponent<AudioSource>();
        pickup_sound.clip = pickupSound[selectedPickupSound];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == PowerUpType.Heal && pc.HP < pc.maxHP)
            {
                pickup_sound.Play();
                ps.Play();
                Destroy(gameObject);
                pc.Heal(amount);
                StartCoroutine(destroyParticleAfter(1));
            }
            else if (type == PowerUpType.HpUp)
            {
                pickup_sound.Play();
                ps.Play();
                Destroy(gameObject);
                pc.maxHP += amount;
                pc.Heal(amount);
                StartCoroutine(destroyParticleAfter(1));
            }
            else if (type == PowerUpType.DamageUp)
            {
                pickup_sound.Play();
                ps.Play();
                Destroy(gameObject);
                pc.DMG += amount;
                StartCoroutine(destroyParticleAfter(1));
            }
            else if (type == PowerUpType.FirerateUp)
            {
                pickup_sound.Play();
                ps.Play();
                Destroy(gameObject);
                pc.Firerate += amount;
                StartCoroutine(destroyParticleAfter(1));
            }
        }
    }

    IEnumerator destroyParticleAfter(float s)
    {
        yield return new WaitForSeconds(s);
        Destroy(ps);
    }
}