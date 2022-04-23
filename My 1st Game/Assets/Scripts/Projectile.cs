using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const int amtDamage = 1;
    PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();   
    }

    void Update()
    {
        
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

