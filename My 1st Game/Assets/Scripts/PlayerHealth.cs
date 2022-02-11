using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public CharacterController player;
    public bool isGameOver;
    private float immuneTime;
    private float immune;

    void Start()
    {
        immune = 1.2f;
        isGameOver = false;
        maxHealth = 50;
        currentHealth = maxHealth;
        player = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        if(isGameOver)
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (immuneTime > 0)
        {
            immuneTime -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int amtDamage, Vector3 direction)
    {
        if (immuneTime <= 0)
        {

            currentHealth -= amtDamage;
            player.getHit();

            if (currentHealth <= 0)
            {
                player.dead();
                isGameOver = true;
            }
            else
            {
                immuneTime = immune;
            }
        }
        player.KnockBack(direction);
    }
}
