using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float attackRadius = 5f;
    float distance;
    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        target = Manager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
       distance = Vector3.Distance(target.position, transform.position);

        if(!FindObjectOfType<PlayerHealth>().isGameOver && distance <= attackRadius)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
