using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float alertRadius = 5f;
    public float attackDistance = 1f;
    float distance;
    Transform target;
    NavMeshAgent agent;
    Collider collider;
    internal enum enemyState { patrol, moveToTarget, attack, dying , flippingOver, upsideDown, flippingBack};
    internal enemyState isCurrently = enemyState.patrol;
    private float flipTimer;
    public int amtDamage = 50;

    internal void Start()
    {
        target = Manager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
    }

    internal void Update()
    {
        switch (isCurrently)
        {
            case enemyState.patrol:
                
                distance = Vector3.Distance(target.position, transform.position);

                if(!FindObjectOfType<PlayerHealth>().isGameOver && distance <= alertRadius)
                {
                    agent.SetDestination(target.position);
                    FaceTarget();
                    isCurrently = enemyState.moveToTarget;
                }
                break;

            case enemyState.moveToTarget:

                if (!FindObjectOfType<PlayerHealth>().isGameOver && distance <= alertRadius)
                {
                    agent.SetDestination(target.position);
                    FaceTarget();
                    isCurrently = enemyState.moveToTarget;
                    if (distance <= attackDistance)
                    {
                        isCurrently = enemyState.attack;
                    }
                        
                }
                break;

            case enemyState.attack:

                if (!FindObjectOfType<PlayerHealth>().isGameOver && distance <= attackDistance)
                {
                    //damagePlayer(requires collider parameter);

                    agent.SetDestination(target.position);
                    FaceTarget();
                    isCurrently = enemyState.moveToTarget;
                }
                break;

            case enemyState.flippingOver:

                transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 180, flipTimer)));
                flipTimer += Time.deltaTime;
                //isCurrently = enemyState.upsideDown;
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
        if(isCurrently != enemyState.flippingOver && Input.GetMouseButton(0))
        {
            isCurrently = enemyState.flippingOver;
            flipTimer = 0f;
            print("sword hit");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            swordHit();
        }

        if (other.gameObject.tag == "Player")
        {
            damagePlayer(other);
        }
    }

    private void damagePlayer(Collider other)
    {
        if(isCurrently == enemyState.attack)
        {
            Vector3 damageDirection = other.transform.position - transform.position;
            damageDirection = damageDirection.normalized;

            FindObjectOfType<PlayerHealth>().DamagePlayer(amtDamage, damageDirection);
        }
    }
}
