using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    PlayerController pc;
    ParticleSystem ps;

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
    [SerializeField] private int selectedPickupSound = 3;

    private void Start()
    {
        pc = GameManager.current.player.GetComponent<PlayerController>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        //selectedPickupSound = Random.Range(0, pickupSound.Length);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == PowerUpType.Heal && pc.HP < pc.maxHP)
            {
                AudioSource.PlayClipAtPoint(pickupSound[selectedPickupSound], transform.position);
                ps.Play();
                Destroy(gameObject);
                pc.Heal(amount);
            }
            else if (type == PowerUpType.HpUp)
            {
                AudioSource.PlayClipAtPoint(pickupSound[selectedPickupSound], transform.position);
                ps.Play();
                Destroy(gameObject);
                pc.maxHP += amount;
                pc.Heal(amount / 2);
            }
            else if (type == PowerUpType.DamageUp)
            {
                AudioSource.PlayClipAtPoint(pickupSound[selectedPickupSound], transform.position);
                ps.Play();
                Destroy(gameObject);
                pc.DMG += amount;
            }
            else if (type == PowerUpType.FirerateUp)
            {
                AudioSource.PlayClipAtPoint(pickupSound[selectedPickupSound], transform.position);
                ps.Play();
                Destroy(gameObject);
                pc.Firerate += amount;
            }
        }
        StartCoroutine(destroyParticleAfter(1));
    }

    IEnumerator destroyParticleAfter(float s)
    {
        yield return new WaitForSeconds(s);
        Destroy(ps);
    }
}