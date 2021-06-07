using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chasePlayer : MonoBehaviour
{
    private Transform player;
    private float dist;
    [SerializeField] float speed;
    public float range;
    Rigidbody rigid;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GameObject.FindGameObjectWithTag("ZombieP").GetComponent<Rigidbody>();

    }

    private void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= range)
        {
            transform.LookAt(player);
            //rigid.velocity += (player.position - transform.position).normalized * speed * Time.deltaTime;
        }

    }
}
