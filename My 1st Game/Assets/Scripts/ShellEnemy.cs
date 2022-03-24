using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEnemy : EnemyController
{
    CharacterController player;
    new void Start()
    {
        base.Start();
        player = FindObjectOfType<CharacterController>();
    }

    new void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && Input.GetMouseButton(0))
        {
            swordHit();
        }

        if (isCurrently == enemyState.upsideDown && other.gameObject.tag == "Player" && !player.Grounded)
        {
            take_damage(50);
        }    
    }

    internal new void swordHit()
    {
        if (isCurrently != enemyState.flippingOver && Input.GetMouseButton(0))
        {
            isCurrently = enemyState.flippingOver;
        }

        if (isCurrently == enemyState.upsideDown && Input.GetMouseButton(0))
        {
            take_damage(50);
        }
    }
}
