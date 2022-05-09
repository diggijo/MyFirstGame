using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : Collectables
{
    public GameObject gem;
    private int totalCoins = 80;
    Vector3 gemSpawn = new Vector3(2, 11, 175);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.coins++;
            Destroy(gameObject);

            if (player.coins == totalCoins)
            {
                Instantiate(gem, gemSpawn, Quaternion.identity);
            }
        }
    }
}
