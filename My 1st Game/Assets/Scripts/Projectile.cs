using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const int amtDamage = 1;
    PlayerController player;
    private float speed = 6f;
    private float destroyTime = 2.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Destroy(gameObject, destroyTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.take_damage(amtDamage);
            Destroy(gameObject);
        }
    }
}

