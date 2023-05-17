using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Animator anim;
    NavMeshAgent navMeshAgent;
    PlayerController playerController;
    [SerializeField] float walkSpeed, runSpeed, attackDistance, attackRange;
    private AudioSource onAttackSound;
    private AudioSource attackSound;

    [SerializeField] AudioClip[] zombieSounds;
    public float health;
    public int killReward;

    public bool canMove = true;
    public bool isAttacking = false;

    private bool isDead;

    public enum ZombieState
    {
        idle,
        walk,
        run,
        attack
    }

    public ZombieState zombieState;
    private void Awake()
    {
        onAttackSound = gameObject.AddComponent<AudioSource>();
        attackSound = gameObject.AddComponent<AudioSource>();
        onAttackSound.spatialBlend = 1f;
        attackSound.spatialBlend = 1f;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    private void Update()
    {

        if (isDead)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        if (playerController.isDead)
        {
            zombieState = ZombieState.idle;
            return;
        }
        if (canMove && isInAttackDistance(attackDistance) && !isAttacking)
        {
            Attack();

        }
        else if (canMove && isInAttackDistance(attackRange) && !isAttacking)
        {
            FollowPlayer(ZombieState.walk);
        }
        else if (canMove && !isInAttackDistance(attackRange))
        {
            FollowPlayer(ZombieState.idle);
        }


        switch (zombieState)
        {
            case ZombieState.idle:
                navMeshAgent.speed = 0;
                anim.Play("Z_Idle");
                break;
            case ZombieState.walk:
                navMeshAgent.speed = walkSpeed;
                anim.Play("Z_Walk_InPlace");
                break;
            case ZombieState.run:
                navMeshAgent.speed = runSpeed;
                anim.Play("Z_Run_InPlace");
                break;
            case ZombieState.attack:
                anim.Play("Z_Attack");
                navMeshAgent.speed = 0;
                break;
        }


    }

    private void DecideMovementType(ZombieState zombieState)
    {
        this.zombieState = zombieState;
    }
    private void FollowPlayer(ZombieState zombieState)
    {
        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }
        DecideMovementType(zombieState);
        if (DistanceBetweenZombiePlayer() < (attackRange * 65) / 100)
        {
            DecideMovementType(ZombieState.run);
        }
        navMeshAgent.destination = playerController.gameObject.transform.position;

    }

    private bool isInAttackDistance(float attackType)
    {
        //Debug.Log(Vector3.Distance(transform.position, playerController.gameObject.transform.position));
        if (DistanceBetweenZombiePlayer() <= attackType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float DistanceBetweenZombiePlayer()
    {
        return Vector3.Distance(transform.position, playerController.gameObject.transform.position);
    }

    private void Attack()
    {
        zombieState = ZombieState.attack;
        isAttacking = true;
        navMeshAgent.isStopped = true;
        float randomValue = Random.value;
        if (randomValue < 0.33f)
        {

        }
        else if (randomValue >= 0.33f && randomValue < 0.66f)
        {

        }
        else if (randomValue >= 0.66f)
        {

        }
        StartCoroutine(AttackCoolDown());


    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0 && !isDead)
        {
            isDead = true;
            anim.Play("Z_FallingForward");
            FindObjectOfType<InputController>().GetComponent<HandleEnemyDead>().enemyKilled++;
            FindObjectOfType<InputController>().GetComponent<HandleEnemyDead>().score += GetComponent<Zombie>().killReward;
            FindObjectOfType<InputController>().GetComponent<HandleEnemyDead>().CheckIfallMobsDead();
            Destroy(gameObject, 5f);
            //give point
        }
    }
    private void PlayAttackStartSound()
    {
        onAttackSound.clip = zombieSounds[1];
        onAttackSound.Play();
    }
    private void PlayAttackSound()
    {
        attackSound.clip = zombieSounds[0];
        attackSound.Play();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<PlayerController>().TakeDamage(20f);
        }

    }
    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1);
        if (isInAttackDistance(attackDistance))
        {
            Attack();
        }
        else
        {
            isAttacking = false;
            DecideMovementType(ZombieState.walk);
        }
    }
}
