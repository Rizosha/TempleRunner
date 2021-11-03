using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointTransform;
    
    
    public CharacterController controller;
    
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    public Camera playerCamera;

    private State state;
    private Vector3 hookshotPosition;
    private enum State
    {
      Normal,  
      HookshotFlyingPlayer
    }

    private void Awake()
    {
        state = State.Normal;
    }

    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:

                CharacterMovement();
                HandleHookshotstart();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }
    }

    private void HandleHookshotMovement()
    {
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;
        float hookshotSpeed = 20f;
        controller.Move(hookshotDir * hookshotSpeed * Time.deltaTime);
        float reachedHookshotPositionDistance = 5f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
        }
    }

    public void CharacterMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // takes input from unity controller
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // creates a transform based off input
        
        Vector3 move = transform.right * x + transform.forward * z;
        
        // moves the character using speed and delta time 
        
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); 
    }

    private void HandleHookshotstart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
         

           if(Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out RaycastHit raycastHit))
           {
               debugHitPointTransform.position = raycastHit.point;
               hookshotPosition = raycastHit.point;
               state = State.HookshotFlyingPlayer;
           }
           
        }
    }
}
