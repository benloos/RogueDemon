using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private PlayerController pc;

    public LayerMask whatisGround, whatisPlayer;

    [SerializeField] Animator anim;


    private Vector3 oldPos;


    //Attack
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private int attackDamageHeavy = 20;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float timeBetweenAttacksHeavy;
    private bool hasAttacked;
    private bool canCast = true;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private AudioSource lightAttScream;
    [SerializeField] private AudioSource lightAttSound;
    [SerializeField] private AudioSource heavyAttScream;
    [SerializeField] private AudioSource heavyAttSound;
    [SerializeField] private AudioSource screamSound;
    [SerializeField] private AudioSource damagesound;

    //HP
    public int health = 100;
    [SerializeField] private float deathTime;
    [SerializeField] private float staggerTime;
    [SerializeField] private float staggerDmg;
    [SerializeField] private AudioSource deathSound;

    //States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInRange, playerInSight;
    public bool isActive;


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.current.player.transform;
        pc = GameManager.current.player.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        anim.SetInteger("HP", health);
    }

    void Update()
    {
        anim.SetInteger("HP", health);
        if (health <= 0) isActive = false;
        if (isActive == true)
        {
            anim.SetBool("IsActive", true);
            //checkRanges
            playerInSight = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
            playerInRange = Physics.CheckSphere(transform.position, attackRange, whatisPlayer);

            if (!playerInRange)
            {
                anim.SetBool("PlayerInRange", false);
                if (!hasAttacked)
                    ChasePlayer();
                else
                {
                    agent.SetDestination(transform.position);
                    transform.LookAt(player);
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
                }
            }
            if (playerInRange)
            {

                if (pc.HP > 0)
                {
                    anim.SetBool("PlayerInRange", true);
                    AttackPlayer();
                }
            }else if (canCast && health<=1000)
            {
                EnrageAttack();
            }
        }
    }

    void ChasePlayer()
    {
        if (player.position != oldPos)
        {
            if (agent.SetDestination(player.position))
            {
                oldPos = player.position;
            }
            else
            {
                agent.SetDestination(oldPos);
            }
        }
    }

    void EnrageAttack()
    {
        canCast = false;
        anim.SetBool("Scream", true);
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        screamSound.Play();
        attackDamage = attackDamage*2;
        attackDamageHeavy = attackDamageHeavy * 2;
        anim.SetBool("Scream", false);
    }

    void AttackPlayer()
    {
        float ran = Random.value;
        if (ran < 0.7)
        {
            anim.SetBool("Attacking", true);
            lightAttScream.Play();
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
            if (!hasAttacked)
            {
                //AttackCode
                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, whatisPlayer);
                //DmgPlayer
                if (hitEnemies.Length > 0)
                {
                    if (pc.HP > 0)
                    {
                        lightAttSound.Play();
                        pc.Damage(attackDamage);
                    }
                }

                hasAttacked = true;
                anim.SetBool("HasAttacked", true);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else
        {
            anim.SetBool("AttackHeavy", true);
            heavyAttScream.Play();
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
            if (!hasAttacked)
            {
                //AttackCode
                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, whatisPlayer);
                //DmgPlayer
                if (hitEnemies.Length > 0)
                {
                    if (pc.HP > 0)
                    {
                        heavyAttSound.Play();
                        pc.Damage(attackDamageHeavy);
                    }
                }

                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacksHeavy);
            }
        }
    }

    void ResetAttack()
    {
        hasAttacked = false;
        anim.SetBool("HasAttacked", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("AttackHeavy", false);
    }

    public void TakeDamage(int dmg)
    {
        health = health - dmg;
        anim.SetInteger("HP", health);
        agent.SetDestination(transform.position);

        if (damagesound.isPlaying == false)
        {
            damagesound.Play();
        }


        if (health <= 0)
        {
            //DeathCode
            isActive = false;
            anim.SetBool("IsActive", false);
            deathSound.Play();
            Destroy(GetComponent<CapsuleCollider>());
            //Invoke(nameof(destroyEnemy), deathTime);
        }
        else if(dmg>staggerDmg)
        {
            anim.SetBool("GotHit", true);
            isActive = false;
            Invoke(nameof(stagger), staggerTime);
        }
    }

    void destroyEnemy()
    {
        Destroy(gameObject);
    }

    void stagger()
    {
        anim.SetBool("GotHit", false);
        isActive = true;
    }
}
