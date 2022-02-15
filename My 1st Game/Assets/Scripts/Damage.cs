using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    Rigidbody rb;

    public int amtDamage = 50;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
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

        if (other.gameObject.tag == "Sword")
        {
            //rb.transform.Rotate(0, 180 * Time.deltaTime, 0);
            //Destroy(transform.parent.gameObject);
            transform.Rotate(0, 180 * Time.deltaTime, 0);
        }
    }
}
