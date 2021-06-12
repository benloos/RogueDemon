using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatisGround, whatisPlayer;

    [SerializeField] Animator anim;


    //Attack
    [SerializeField] private float timeBetweenAttacks;
    private bool hasAttacked;
    [SerializeField] public Transform attackPoint;

    //HP
    [SerializeField] private int health=100;
    [SerializeField] private float deathTime;
    [SerializeField] private float staggerTime;

    //States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInRange, playerInSight;
    public bool isActive;
    

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim.SetFloat("HP", health);
    }

    void Update()
    {
        if (isActive == true)
        {
            anim.SetBool("isActive", true);
            //checkRanges
            playerInSight = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
            playerInRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);

            if (!playerInRange)
            {
                ChasePlayer();
                anim.SetBool("PlayerInRange", false);
            }
            if (playerInRange)
            {
                anim.SetBool("PlayerInRange", true);
                AttackPlayer();
            }
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
        if(!hasAttacked)
        {
            //AttackCode
            Collider[] hitEnemies=Physics.OverlapSphere(attackPoint.position, attackRange, whatisPlayer);
            //DmgPlayer
            foreach(Collider hieEnemy in hitEnemies)
            {
                Debug.Log("Zombie hit Player");
            }

            hasAttacked = true;
            anim.SetBool("HasAttacked", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        hasAttacked = false;
        anim.SetBool("HasAttacked", false);
    }

    public void TakeDamage(int dmg)
    {
        health = health - dmg;
        anim.SetFloat("HP", health);
        anim.SetBool("GotHit", true);
        Debug.Log(health);

        if (health<=0)
        {
            //DeathCode

            Invoke(nameof(destroyEnemy), deathTime);
        }
        Invoke(nameof(WaitForSeconds), staggerTime);
    }

    void destroyEnemy()
    {
        Destroy(gameObject);
    }
}
