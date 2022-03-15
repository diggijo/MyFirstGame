using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour, IDamagable
{
    private float characterSpeed;
    private float walkSpeed;
    private float runSpeed;
    Animator characterAnimator;
    Rigidbody rb;
    private bool characterOnFloor = true;
    public int coins;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    private bool Grounded { get { return characterOnFloor; } set { characterOnFloor = value; characterAnimator.SetBool("isGrounded", value); } }

    void Start()
    {
        walkSpeed = 3f;
        runSpeed = 6f;
        characterAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        Grounded = true;
    }

    void Update()
    {
        if(knockBackCounter <= 0)
        {
            Move();
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0, zDirection);
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 5f); //https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
        }

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        else if(moveDirection == Vector3.zero)
        {
            Idle();
        }

        if(Input.GetMouseButton(0))
        {
            Attack();
        }

        if(Input.GetMouseButton(1))
        {
            Defend();
        }

        transform.position += characterSpeed * moveDirection * Time.deltaTime;

        if(Input.GetButtonDown("Jump") && characterOnFloor)
        {
            Jump();
        }
}

    private void Jump()
    {
        rb.AddForce(new Vector3(0, 5.5f, 0), ForceMode.Impulse);
        characterAnimator.SetBool("isJumping", true);
        Grounded = false;
    }

    private void Defend()
    {
        characterAnimator.SetBool("isDefending", true);
    }

    private void Attack()
    {
        characterAnimator.SetBool("isAttacking", true);

    }

    private void Idle()
    {
        characterAnimator.SetBool("isWalking", false);
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isDefending", false);
        characterAnimator.SetBool("isAttacking", false);
        characterAnimator.SetBool("isJumping", false);
        characterAnimator.SetBool("takingDamage", false);
    }

    private void Run()
    {
        characterSpeed = runSpeed;
        characterAnimator.SetBool("isRunning", true);
        characterAnimator.SetBool("isWalking", false);
        characterAnimator.SetBool("isDefending", false);
        characterAnimator.SetBool("isJumping", false);
        characterAnimator.SetBool("isAttacking", false);
        characterAnimator.SetBool("takingDamage", false);
    }

    private void Walk()
    {
        characterSpeed = walkSpeed;
        characterAnimator.SetBool("isWalking", true);
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isDefending", false);
        characterAnimator.SetBool("isAttacking", false);
        characterAnimator.SetBool("isJumping", false);
        characterAnimator.SetBool("takingDamage", false);
    }

    public void getHit()
    {
        characterAnimator.SetBool("takingDamage", true);
    }

    public void dead()
    {
        characterAnimator.Play("Die");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Grounded = true;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Coins: " + coins);
    }

    public void KnockBack(Vector3 enemyPos)
    {
        knockBackCounter = knockBackTime;

        Vector3 damageDirection = enemyPos - transform.position;
        damageDirection = damageDirection.normalized;
        rb.AddForce(damageDirection * knockBackForce * 100);
    }

    public void take_damage(int amtDamage)
    {
        FindObjectOfType<PlayerHealth>().DamagePlayer(amtDamage); 
    }
}
