using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour, IDamagable
{
    internal enum enemyState { idle, attack, dying};
    internal enemyState isCurrently = enemyState.idle;
    Animator enemyAnimator;
    Transform wand;
    GameObject magicBall;
    Transform target;
    PlayerController player;
    PlayerHealth ph;
    public GameObject magic_Ball_template;
    /*private float animation_timer = 0;
    private float magic_start = 0.75f;
    private float magic_end = 2.5f; 
    private float total_animation = 3f;*/
    private const float alertRadius = 10f;
    public const int amtDamage = 1;
    private int currentHealth;
    private const int maxHealth = 1;
    private float distance;
    private const float faceTargetSpeed = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        target = Manager.instance.player.transform;
        player = target.GetComponent<PlayerController>();
        ph = FindObjectOfType<PlayerHealth>();
        wand = find_Wand();
        magicBall = Instantiate(magic_Ball_template, wand);
        magicBall.SetActive(false);
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        FaceTarget();

        distance = Vector3.Distance(target.position, transform.position);

        if (FindObjectOfType<PlayerHealth>().isGameOver)
        {
            enemyAnimator.Play("Victory");
        }

        switch (isCurrently)
        {
            case enemyState.idle:

                magicBall.SetActive(false);

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
                    enemyAnimator.SetBool("isAttacking", false);
                }

                magicBall.SetActive(true);

                break;

            case enemyState.dying:

                Destroy(gameObject);

                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && player.Attacking)
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

    internal void swordHit()
    {

        take_damage(amtDamage);
    }

    public void take_damage(int amtDamage)
    {
        currentHealth = maxHealth - amtDamage;

        if (currentHealth <= 0)
        {
            isCurrently = enemyState.dying;
        }
    }
    internal void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * faceTargetSpeed);
        }
    }
}
