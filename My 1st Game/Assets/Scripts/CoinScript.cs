using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : Collectables
{
    public GameObject gem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.coins++;
            Destroy(gameObject);

            if (player.coins == 80)
            {
                Instantiate(gem, new Vector3(2, 11, 175), Quaternion.identity);
            }
        }
    }
}
