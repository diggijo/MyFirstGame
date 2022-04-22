using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const int amtDamage = 50;
    PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();   
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.take_damage(amtDamage);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
