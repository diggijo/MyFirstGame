using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : EnemyController
{
    Transform wand;
    GameObject magicBall;
    public GameObject magic_Ball_template;
    private float animation_timer = 0;
    private float magic_start = 0.75f;
    private float magic_end = 2.5f; 
    private float total_animation = 3f;
    new void Start()
    {
        base.Start();
        alertRadius = 10f;
        wand = find_Wand();
        magicBall = Instantiate(magic_Ball_template, wand);
        magicBall.SetActive(false);
    }

    new void Update()
    {
        switch (isCurrently)
        {
            case enemyState.idle:

                magicBall.SetActive(false);
                enemyAnimator.SetBool("isAttacking", false);
                enemyAnimator.Play("Idle");

                if (!ph.isGameOver && distance <= alertRadius)
                {
                    isCurrently = enemyState.attack;
                }

                break;

            case enemyState.attack:

                enemyAnimator.SetBool("isAttacking", true);

                if (!ph.isGameOver && distance > alertRadius)
                {
                    isCurrently = enemyState.idle;
                }

                animation_timer += Time.deltaTime;

                if (animation_timer > total_animation)
                {
                    animation_timer = 0f;
                }

                magicBall.SetActive((animation_timer > magic_start) && (animation_timer < magic_end));

                print("attacking");

                break;

            case enemyState.dying:

                Destroy(gameObject);

                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && targetScript.Attacking)
        {
            swordHit();
        }
    }

    private Transform find_Wand()
    {
        foreach (Transform bone in GetComponentsInChildren<Transform>())
            if (bone.name == "Wand")
            {
                return bone;
            }

        return null;
    }
}
