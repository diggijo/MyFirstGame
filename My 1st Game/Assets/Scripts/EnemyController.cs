using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    public float alertRadius = 5f;
    public float attackDistance = 1f;
    float distance;
    Transform target;
    PlayerController targetScript;
    NavMeshAgent agent;
    Animator enemyAnimator;

    internal enum enemyState { patrol, moveToTarget, attack, dying , flippingOver, upsideDown, flippingBack};
    internal enemyState isCurrently = enemyState.patrol;
    private const float flipHeight = 0.6f;
    internal float flipTimer = 0f;
    private const float flipTimerMax = 1.1f;
    private float flipCooldown;
    private const float flipCooldownTimer = 4f;
    private float yPos;
    private const float moveYPos = 0.45f;
    private float attackTimer = 0f;
    private const float attack_Cooldown = 1.2f;
    public const int amtDamage = 50;
    private int currentHealth;
    private int maxHealth;
    private const float faceTargetSpeed = 5f;

    internal void Start()
    {
        target = Manager.instance.player.transform;
        targetScript = target.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        Collider collider = GetComponent<CapsuleCollider>();
        maxHealth = 50;
        currentHealth = maxHealth;
    }

    internal void Update()
    {
        attackTimer -= Time.deltaTime;
        distance = Vector3.Distance(target.position, transform.position);

        if (isCurrently != enemyState.attack)
        {
            enemyAnimator.SetBool("isAttacking", false);
        }

        if (FindObjectOfType<PlayerHealth>().isGameOver)
        {
            enemyAnimator.Play("Victory");
            agent.enabled = false;
        }
            
        switch (isCurrently)
        {
            case enemyState.patrol:

                if(!FindObjectOfType<PlayerHealth>().isGameOver && distance <= alertRadius)
                {
                    isCurrently = enemyState.moveToTarget;
                }

                break;

            case enemyState.moveToTarget:

                agent.enabled = true;
                agent.SetDestination(target.position);
                FaceTarget();

                if (!FindObjectOfType<PlayerHealth>().isGameOver && distance <= attackDistance)
                {
                    isCurrently = enemyState.attack;
                }
                
                if (distance > alertRadius)
                {
                    isCurrently = enemyState.patrol;
                }

                break;

            case enemyState.attack:

                if(!FindObjectOfType<PlayerHealth>().isGameOver && distance <= attackDistance)
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

                if (flipTimer >= moveYPos)
                {
                    yPos -=Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
                }

                if (flipTimer >= flipTimerMax)
                {
                    isCurrently = enemyState.patrol;
                    flipTimer = 0;
                }

                break;

            case enemyState.dying:

                Destroy(gameObject);

                break;
        }
    }

    private void FaceTarget()
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
    
        take_damage(50);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
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
