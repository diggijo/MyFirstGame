using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyController
{
    PlayerController player;
    new
    void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }

    new
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && player.Attacking)
        {
            swordHit();
        }

        if (other.gameObject.tag == "Player" && !player.Grounded)
        {
                take_damage(50);  
        }
    }

    internal new void swordHit()
    {
        take_damage(50);
    }
}
