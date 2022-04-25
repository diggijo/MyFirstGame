using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    internal PlayerController player;
    internal PlayerHealth ph;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        ph = FindObjectOfType<PlayerHealth>();
    }
    void Update()
    {
        if(gameObject.tag == "Coin")
        {
            transform.Rotate(0, 0, -90 * Time.deltaTime);

        }

        if(gameObject.tag =="Heart")
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
        }
    }
}
