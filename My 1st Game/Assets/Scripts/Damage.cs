using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    Rigidbody rb;
    EnemyController owner;

    public int amtDamage = 50;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        owner = GetComponentInParent<EnemyController>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {
            Vector3 damageDirection = other.transform.position - transform.position;
            damageDirection = damageDirection.normalized;

            FindObjectOfType<PlayerHealth>().DamagePlayer(amtDamage, damageDirection);
        }
    }
}
