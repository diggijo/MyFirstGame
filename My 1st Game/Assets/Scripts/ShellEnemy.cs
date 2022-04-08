using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEnemy : EnemyController
{
    PlayerController player;
    new void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }

    new void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && player.Attacking)
        {
            swordHit();
        }

        else if (isCurrently == enemyState.upsideDown && other.gameObject.tag == "Player" && !player.Grounded)
        {
            take_damage(amtDamage);
        }

        else
        {
            player.take_damage(amtDamage);
        }
    }

    internal new void swordHit()
    {
        if (isCurrently == enemyState.upsideDown)
        {
            take_damage(amtDamage);
        }
        else if (isCurrently != enemyState.flippingOver && isCurrently != enemyState.dying)
        {
            isCurrently = enemyState.flippingOver;
        }
    }
}
