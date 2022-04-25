using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    PlayerController player;
    public bool isGameOver;
    private float immuneTime;
    private float immune;
    [SerializeField] private Image[] hearts;

    void Start()
    {
        immune = 2f;
        isGameOver = false;
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerController>();
        UpdateHealth();
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

        if(player.fellDownHole)
        {
            isGameOver = true;
        }
    }

    public void DamagePlayer(int amtDamage)
    {
        if (immuneTime <= 0)
        {
            currentHealth -= amtDamage;

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

        UpdateHealth();
    }

    public IEnumerator resetGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainGame");
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
    }
    public void addHealth()
    {
        currentHealth++;
        UpdateHealth();
    }
}
