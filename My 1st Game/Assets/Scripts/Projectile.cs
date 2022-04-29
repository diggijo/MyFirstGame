using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const int amtDamage = 1;
    PlayerController player;
    EnemyController enemy;
    Wizard wiz;
    private float speed = 6f;
    private float destroyTime = 2.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<EnemyController>();
        wiz = FindObjectOfType<Wizard>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Destroy(gameObject, destroyTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" && !player.defending) || (other.gameObject.tag == "Player" && player.defending && !enemy.playerNotFacing(wiz.target)))
        {
            player.take_damage(amtDamage);
            Destroy(gameObject);
        }
    }
}

