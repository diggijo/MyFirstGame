using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyController
{
    new
    void Start()
    {
        base.Start();
    }

    new
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && targetScript.Attacking)
        {
            swordHit();
        }

        if (other.gameObject.tag == "Player" && !targetScript.Grounded)
        {
                take_damage(50);  
        }
    }
}
