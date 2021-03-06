using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    private float characterSpeed;
    private const float walkSpeed = 4f;
    private const float runSpeed = 6f;
    Animator characterAnimator;
    Rigidbody rb;
    private bool characterOnFloor = true;
    public int coins;
    public int gem;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;
    private float swordAttack;
    private const float swordAttackTimer = 0.5f;
    internal bool fellDownHole = false;
    PlayerHealth ph;
    private const float rotateSpeed = 5f;
    private const float jumpHeight = 5.5f;
    private const int fallDamage = 3;
    internal bool Grounded { get { return characterOnFloor; } set { characterOnFloor = value; characterAnimator.SetBool("isGrounded", value); } }
    internal bool Attacking { get; set; }
    internal bool defending;

    void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        ph = FindObjectOfType<PlayerHealth>();
        rb = GetComponent<Rigidbody>();
        Grounded = true;
    }

    void Update()
    {
        if (knockBackCounter <= 0 && !ph.isGameOver)
        {
            Move();
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

        swordAttack += Time.deltaTime;

        if (swordAttack > swordAttackTimer)
        {
            Attacking = false;
        }
    }

    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0, zDirection);
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed); //https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
        }

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) && characterOnFloor)
        {
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && characterOnFloor && !defending)
        {
            Run();
        }

        else if((moveDirection == Vector3.zero) && Grounded)
        {
            Idle();
        }

        if (Input.GetMouseButton(0))
        {
            Attack();
        }

        if (Input.GetMouseButton(1))
        {
            Defend();
        }
        else
        {
            defending = false;
        }
            

        transform.position += characterSpeed * moveDirection * Time.deltaTime;

        if(Input.GetKey(KeyCode.Space) && characterOnFloor)
        {
            Jump();
        }
}

    private void Jump()
    {
        rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        characterAnimator.SetBool("isJumping", true);
        Grounded = false;
    }

    private void Defend()
    {
        characterAnimator.SetBool("isDefending", true);
        defending = true;
    }

    private void Attack()
    {
        swordAttack = 0f;
        characterAnimator.SetBool("isAttacking", true);
        Attacking = true;
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

    public void dead()
    {
        characterAnimator.SetBool("isDead", true);
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
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.fontSize = 48;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.UpperRight;
        float square = Screen.width * 0.08f;
        float xPosition = Screen.width * .91f;
        float yPosition = Screen.height * .01f;

        GUI.Label(new Rect(xPosition, yPosition, square, square), "" + coins, guiStyle);
    }

    public void KnockBack(Vector3 enemyPos)
    {
        knockBackCounter = knockBackTime;
        Vector3 damageDirection = enemyPos - transform.position;
        damageDirection = damageDirection.normalized;
        rb.AddForce(damageDirection * knockBackForce);
    }

    public void take_damage(int amtDamage)
    {
        ph.DamagePlayer(amtDamage);
        characterAnimator.Play("GetHit");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallToDeath")
        {
            fellDownHole = true;
            dead();
        }
    }
}
