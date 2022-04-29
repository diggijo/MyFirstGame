using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Collectables
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(ph.currentHealth < ph.maxHealth)
            {
                ph.addHealth();
            }

            Destroy(gameObject);
        }
    }
}
