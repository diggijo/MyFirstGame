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
        if(isCurrently == enemyState.flippingOver)
        {
            isCurrently = enemyState.dying;
        }
    }
}
