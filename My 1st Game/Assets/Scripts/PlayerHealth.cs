using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    CharacterController player;
    public bool isGameOver;
    private float immuneTime;
    private float immune;

    void Start()
    {
        immune = 1.2f;
        isGameOver = false;
        maxHealth = 500;
        currentHealth = maxHealth;
        player = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        if(isGameOver)
        {
            StartCoroutine(resetGame());
        }
        if (immuneTime > 0)
        {
            immuneTime -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int amtDamage)
    {
        if (immuneTime <= 0)
        {

            currentHealth -= amtDamage;
            player.getHit();

            if (currentHealth <= 0)
            {
                player.dead();
                isGameOver = true;
                immuneTime = immune;
            }
            else
            {
                immuneTime = immune;
            }
        }
    }

    public IEnumerator resetGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("SampleScene");
    }
}
