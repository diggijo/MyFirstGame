using System;
using UnityEngine;
using System.Collections.Generic;

public class TreasureChest : MonoBehaviour
{
    public GameObject lootBox;
    PlayerController player;
    public bool bouncingBox = true;
    private bool isPlayerClose;
    public bool isOpen;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        BounceBox(bouncingBox);
    }

    private void Update()
    {
        if (isPlayerClose)
        {
            if (Input.GetKey(KeyCode.E) || player.Attacking)
            {
                Open();
            }      
        }
    }

    public void BounceBox (bool bounceIt)
    {
        if (animator)
        {
            animator.SetBool("bounce", bounceIt);
        }
    }

    public void Open ()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;

        if (animator)
        {
            animator.Play("Open");
        }

        player.coins += 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerClose = false;
    }
}
