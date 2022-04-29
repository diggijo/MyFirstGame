using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : Collectables
{
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.gem++;
            Destroy(gameObject);
        }
    }
}
