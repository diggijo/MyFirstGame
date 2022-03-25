using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    public float alertRadius = 10f;
    public float attackDistance = 5f;
    float distance;
    Transform target;
    PlayerController targetScript;
    NavMeshAgent agent;

    internal enum enemyState { patrol, moveToTarget, attack, dying , flippingOver, upsideDown, flippingBack};
    internal enemyState isCurrently = enemyState.patrol;
    internal float flipTimer = 0f;
    private float attackTimer = 0f;
    private float attack_Cooldown = 1.2f;
    public int amtDamage = 50;
    private int currentHealth;
    private int maxHealth;  

    internal void Start()
    {
        target = Manager.instance.player.transform;
        targetScript = target.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        Collider collider = GetComponent<CapsuleCollider>();
        maxHealth = 50;
        currentHealth = maxHealth;
    }

    internal void Update()
    {
        attackTimer -= Time.deltaTime;
        Vector3 toTarget = (transform.position - target.position).normalized;
        distance = Vector3.Distance(target.position, transform.position);

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
                    if(attackTimer<=0)
                    {
                        attackTimer = attack_Cooldown;

                        if (targetScript is IDamagable && !targetScript.defending)
                        {
                            targetScript.take_damage(50);
                            targetScript.KnockBack(transform.position);     
                        }

                        if(targetScript.defending && Vector3.Dot(toTarget, transform.forward) < 0)
                        {
                            targetScript.take_damage(50);
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
                transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 179, flipTimer)));
                flipTimer += Time.deltaTime;

                if (flipTimer>2)
                {
                    isCurrently = enemyState.upsideDown;
                }   
                
                break;

            case enemyState.upsideDown:

                break;

            case enemyState.flippingBack:

                break;

            case enemyState.dying:

                Destroy(gameObject);

                break;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * 5f);
    }

    internal void swordHit()
    {
        print("Sword Hit");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    public void take_damage(int amtDamage)
    {
        currentHealth =  maxHealth - amtDamage;

        if(currentHealth <= 0 )
        {
            isCurrently = enemyState.dying;
        }
    }
}
