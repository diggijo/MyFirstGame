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

    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.05f); //https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
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

        transform.position += characterSpeed * moveDirection * Time.deltaTime;
    }

    private void Idle()
    {
        characterAnimator.SetBool("isWalking", false);
        characterAnimator.SetBool("isRunning", false);
    }

    private void Run()
    {
        characterSpeed = runSpeed;
        characterAnimator.SetBool("isRunning", true);
        characterAnimator.SetBool("isWalking", false);
    }

    private void Walk()
    {
        characterSpeed = walkSpeed;
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isWalking", true);
    }
}
