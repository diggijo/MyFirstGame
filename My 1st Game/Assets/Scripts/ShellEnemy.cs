using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEnemy : EnemyController
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && targetScript.Attacking)
        {
            swordHit();
        }

        if (isCurrently == enemyState.upsideDown && other.gameObject.tag == "Player" && !targetScript.Grounded)
        {
            take_damage(amtDamage);
        }

        if (isCurrently != enemyState.upsideDown && other.gameObject.tag == "Player" && isCurrently != enemyState.dying && !targetScript.defending)
        {
            targetScript.take_damage(amtDamage);
        }
    }

    internal new void swordHit()
    {
        if (isCurrently == enemyState.upsideDown)
        {
            take_damage(amtDamage);
        }

        if (isCurrently != enemyState.flippingOver && isCurrently != enemyState.dying)
        {
            isCurrently = enemyState.flippingOver;
        }
    }
}
