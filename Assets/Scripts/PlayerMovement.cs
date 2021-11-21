using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    public CharacterController controller;
    
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    
    //private Vector3 velocity;
    private float characterVelocityY;
    private Vector3 momentum;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    public Camera playerCamera;

    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private bool isGrapple;
    public float grappleDistance = 4f;
    public LayerMask grappleMask;
    
    private enum State
    {
      Normal,
      HookshotThrown,
      HookshotFlyingPlayer,
      
    }

    private void Awake()
    {
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
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
            case State.HookshotThrown:
                HandleHookshotThrow();
                CharacterMovement();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
            
        }
    }

   

    public void CharacterMovement()
    {
        // takes input from unity controller
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 charVelocity = transform.right * x + transform.forward * z;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && characterVelocityY < 0)
        {
           characterVelocityY = -0f;
        }
        
        if (testInputJump() && isGrounded)
        {
            characterVelocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        charVelocity.y = characterVelocityY;
        characterVelocityY += gravity * Time.deltaTime;
       
        charVelocity += momentum;
        
        
        controller.Move(charVelocity * speed * Time.deltaTime);
        controller.Move(charVelocity * Time.deltaTime);
        if (momentum.magnitude >= 0f)
        {
            float momentumDrag = 3f;
            momentum -= momentum * momentumDrag * Time.deltaTime;
            if (momentum.magnitude < .0f)
            {
                momentum = Vector3.zero;
            }
        }
    }

    
    
   
    
    
    
    
    
    private void ResetGravity()
    {
        characterVelocityY = -0f;
    }

    private void HandleHookshotstart()
    {
        if (TestInputDownHookshot())
        {
         

           if(Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out RaycastHit raycastHit))
           {
               debugHitPointTransform.position = raycastHit.point;
               hookshotPosition = raycastHit.point;
               hookshotSize = 0f;
               hookshotTransform.gameObject.SetActive(true);
               hookshotTransform.localScale = Vector3.zero;

               isGrapple = Physics.CheckSphere(hookshotPosition, grappleDistance, grappleMask);
               if (isGrapple)
               {
                   state = State.HookshotThrown;
               }
               
           }
           
        }
        
    }

    private void HandleHookshotThrow()
    {
        
        hookshotTransform.LookAt(hookshotPosition);
        float hookshoThrowSpeed = 180f;
        hookshotSize += hookshoThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        
        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
        }
       
    }
    
    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;
        float hsSpeedMax = 70f;
        float hsSpeedMin = 30f;
        
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position,hookshotPosition), hsSpeedMin, hsSpeedMax);
        float hookshotSpeedMultiplier = 1.5f;
        
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);
        
        float jumpSpeed = .4f;
        float momentumExtra = .3f;
        float reachedHookshotPositionDistance = 2f;
        
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
          
            momentum = hookshotDir * hookshotSpeed * momentumExtra;
            momentum += Vector3.up * jumpSpeed;
            state = State.Normal;
            ResetGravity();
            hookshotTransform.gameObject.SetActive(false);
        }

        if (TestInputDownHookshot())
        {
            StopHookshot();
        }

        if (testInputJump())
        {
            momentum = hookshotDir * hookshotSpeed * momentumExtra;
            momentum += Vector3.up * jumpSpeed;
            StopHookshot();
        }
    }

    private void StopHookshot()
    {
        state = State.Normal;
            ResetGravity();
            hookshotTransform.gameObject.SetActive(false);
    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    private bool testInputJump()
    {
        return Input.GetButton("Jump");
    }

    
}
