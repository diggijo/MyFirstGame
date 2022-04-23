using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    public float alertRadius = 5f;
    public float attackDistance = 1f;
    internal float distance;
    Transform target;
    internal PlayerController targetScript;
    internal PlayerHealth ph;
    NavMeshAgent agent;
    internal Animator enemyAnimator;

    internal enum enemyState { idle, moveToTarget, attack, dying , flippingOver, upsideDown, flippingBack};
    internal enemyState isCurrently = enemyState.idle;
    private const float flipHeight = 1.7f;
    internal float flipTimer = 0f;
    private const float flipTimerMax = 1.1f;
    private float flipCooldown;
    private const float flipCooldownTimer = 4f;
    private float yPos;
    private const float moveYPos = 0.45f;
    private const float flippingBack = .61f;
    private float attackTimer = 0f;
    private const float attack_Cooldown = 1.2f;
    public const int amtDamage = 1;
    private int currentHealth;
    private const int maxHealth = 1;
    private const float faceTargetSpeed = 5f;
    internal bool isAttacking;

    internal void Start()
    {
        target = Manager.instance.player.transform;
        targetScript = target.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        Collider collider = GetComponent<CapsuleCollider>();
        currentHealth = maxHealth;
        yPos = transform.position.y;
        ph = FindObjectOfType<PlayerHealth>();
    }

    internal void Update()
    {
        attackTimer -= Time.deltaTime;
        distance = Vector3.Distance(target.position, transform.position);

        if (isCurrently != enemyState.attack)
        {
            enemyAnimator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        if (FindObjectOfType<PlayerHealth>().isGameOver)
        {
            enemyAnimator.Play("Victory");
            agent.enabled = false;
        }
            
        switch (isCurrently)
        {
            case enemyState.idle:

                if(!ph.isGameOver && distance <= alertRadius)
                {
                    isCurrently = enemyState.moveToTarget;
                }

                break;

            case enemyState.moveToTarget:

                agent.enabled = true;
                agent.SetDestination(target.position);
                FaceTarget();

                if (!ph.isGameOver && distance <= attackDistance)
                {
                    isCurrently = enemyState.attack;
                    isAttacking = true;
                }
                
                if (distance > alertRadius)
                {
                    isCurrently = enemyState.idle;
                }

                break;

            case enemyState.attack:

                if(!ph.isGameOver && distance <= attackDistance)
                {
                    if (attackTimer<=0)
                    {
                        attackTimer = attack_Cooldown;
                        enemyAnimator.SetBool("isAttacking", true);

                        if (targetScript is IDamagable && (!targetScript.defending && targetScript.Grounded) || (targetScript.defending && playerNotFacing(target)))
                        {
                            targetScript.take_damage(amtDamage);
                            targetScript.KnockBack(transform.position);     
                        }
                    }
                }

                if (distance > attackDistance)
                {
                    isCurrently = enemyState.moveToTarget;
                }
                         
                break;

            case enemyState.flippingOver:

                agent.enabled = false;

                if(yPos >= flipHeight)
                {
                    yPos = flipHeight;
                }

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 180, flipTimer)));
                flipTimer += Time.deltaTime;

                if(flipTimer >= moveYPos)
                {
                    yPos += Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
                }

                if (flipTimer >= flipTimerMax)
                {
                    isCurrently = enemyState.upsideDown;
                    flipTimer = 0;
                    flipCooldown = 0;
                }   
                
                break;

            case enemyState.upsideDown:

                flipCooldown += Time.deltaTime;

                if (flipCooldown >= flipCooldownTimer)
                {
                    isCurrently = enemyState.flippingBack;
                }
                   
                break;

            case enemyState.flippingBack:

                if (yPos <= 0)
                {
                    yPos = 0;
                }

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(180, 0, flipTimer)));
                flipTimer += Time.deltaTime;

                if (flipTimer >= flippingBack)
                {
                    yPos -=Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
                }

                if (flipTimer >= flipTimerMax)
                {
                    isCurrently = enemyState.idle;
                    flipTimer = 0;
                }
                break;

            case enemyState.dying:

                Destroy(gameObject);

                break;
        }
    }

    internal void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if(direction != Vector3.zero)
        {
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * faceTargetSpeed);  
        }
    }

    internal void swordHit()
    {
    
        take_damage(amtDamage);
    }

    public void take_damage(int amtDamage)
    {
        currentHealth = maxHealth - amtDamage;

        if(currentHealth <= 0)
        {
            isCurrently = enemyState.dying;
        }
    }

    public bool playerNotFacing(Transform target)
    {
        Vector3 toTarget = (transform.position - target.position).normalized;
        return (Vector3.Dot(toTarget, target.forward) < 0);
    }
}
