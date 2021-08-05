using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleHitScript : MonoBehaviour
{
    [SerializeField] private PlayerController pc;

    private void OnParticleCollision(GameObject other)
    {
        //pc.Damage(20);
    }

    
}
