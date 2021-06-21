using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private PlayerController pc;

    public LayerMask whatisGround, whatisPlayer;

    [SerializeField] Animator anim;


    private Vector3 oldPos;


    //Attack
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float timeBetweenAttacks;
    private bool hasAttacked;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private AudioSource attScream;
    [SerializeField] private AudioSource attSound;
    [SerializeField] private AudioSource damagesound;

    //HP
    public int health=100;
    [SerializeField] private float deathTime;
    [SerializeField] private float staggerTime;
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
                if (pc.HP > 0)
                {
                    anim.SetBool("PlayerInRange", true);
                    AttackPlayer();
                }
            }
        }
    }

    void ChasePlayer()
    {
        if(agent.SetDestination(player.position))
        {
            oldPos = player.position;
        }
        else
        {
            agent.SetDestination(oldPos);
        }
    }

    void AttackPlayer()
    {
        attScream.Play();
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
        if(!hasAttacked)
        {
            //AttackCode
            Collider[] hitEnemies=Physics.OverlapSphere(attackPoint.position, attackRange, whatisPlayer);
            //DmgPlayer
            if(hitEnemies.Length>0)
            {
                attSound.Play();
                pc.Damage(attackDamage);
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
        Debug.Log(health);
        agent.SetDestination(transform.position);
        
        Debug.Log("false heißt der bums ist nicht am playen" + damagesound.isPlaying);
        if(damagesound.isPlaying == false){
            Debug.Log("hello?");
            damagesound.Play();
        }
            

        if (health <= 0)
        {
            //DeathCode
            isActive = false;
            anim.SetBool("isActive", false);
            deathSound.Play();
            Destroy(GetComponent<CapsuleCollider>());
            //Invoke(nameof(destroyEnemy), deathTime);
        }
        else
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
