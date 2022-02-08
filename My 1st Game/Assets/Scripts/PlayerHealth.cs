using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public CharacterController player;
    void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<CharacterController>();
    }

    void Update()
    {

    }

    public void DamagePlayer(int amtDamage, Vector3 direction)
    {
        currentHealth -= amtDamage;
        player.KnockBack(direction);
    }
}
