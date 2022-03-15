using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float alertRadius = 10f;
    public float attackDistance = 5f;
    float distance;
    Transform target;
    CharacterController targetScript;
    NavMeshAgent agent;

    internal enum enemyState { patrol, moveToTarget, attack, dying , flippingOver, upsideDown, flippingBack};
    internal enemyState isCurrently = enemyState.patrol;
    private float flipTimer;
    public int amtDamage = 50;

    internal void Start()
    {
        target = Manager.instance.player.transform;
        targetScript = target.GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        Collider collider = GetComponent<CapsuleCollider>();
    }

    internal void Update()
    {
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
                    if (targetScript is IDamagable)
                    {
                        targetScript.take_damage(50);
                        targetScript.KnockBack(transform.position);
                    }
                }
                if (distance > attackDistance)
                {
                    isCurrently = enemyState.moveToTarget;
                }
                         
                break;

            case enemyState.flippingOver:
                transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 180, flipTimer)));
                flipTimer += Time.deltaTime;
                //isCurrently = enemyState.upsideDown;

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
        if(isCurrently != enemyState.flippingOver && Input.GetMouseButton(0))
        {
            isCurrently = enemyState.flippingOver;
            flipTimer = 0f;
            print("sword hit");
        }

        if(isCurrently == enemyState.upsideDown && Input.GetMouseButton(0))
        {
            isCurrently = enemyState.dying;
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
    }
}
