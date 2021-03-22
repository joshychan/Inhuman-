using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public  CharacterController controller;
    public float speed = 4f;

    public float walkSpeed = 4f;

    public float runSpeed = 12f;

    public float gravity = -9.91f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groudMask;

    public float jumpHeight = 3f;

    private bool walking = true;
    Vector3 velocity;
    bool isGrounded;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (walking)
            {
                speed = runSpeed;
                walking = false;
            }
            else
            {
                speed = walkSpeed;
                walking = true;
            }
        }  






        isGrounded = Physics.CheckSphere( groundCheck.position, groundDistance, groudMask);
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
