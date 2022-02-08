using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private float characterSpeed;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    Animator characterAnimator;
    Rigidbody rb;
    private bool characterOnFloor = true;
    public int coins = 0;
    private bool Grounded { get { return characterOnFloor; } set { characterOnFloor = value; characterAnimator.SetBool("isGrounded", value); } }

    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        Grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
    }

    private void Run()
    {
        characterSpeed = runSpeed;
        characterAnimator.SetBool("isRunning", true);
        characterAnimator.SetBool("isWalking", false);
        characterAnimator.SetBool("isDefending", false);
        characterAnimator.SetBool("isJumping", false);
        characterAnimator.SetBool("isAttacking", false);
    }

    private void Walk()
    {
        characterSpeed = walkSpeed;
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isWalking", true);
        characterAnimator.SetBool("isDefending", false);
        characterAnimator.SetBool("isAttacking", false);
        characterAnimator.SetBool("isJumping", false);
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
}
