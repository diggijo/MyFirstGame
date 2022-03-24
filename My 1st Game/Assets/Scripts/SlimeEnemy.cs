using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyController
{
    CharacterController player;
    new
    void Start()
    {
        base.Start();
        player = FindObjectOfType<CharacterController>();
    }

    new
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && Input.GetMouseButton(0))
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
