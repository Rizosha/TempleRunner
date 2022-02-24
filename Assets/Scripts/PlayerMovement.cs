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
    public float jumpHeight = 2.5f;

    private Vector3 velocity;
   
    private Vector3 momentum;

    public Transform groundCheck;
    public float groundDistance = 3f;
    public LayerMask groundMask;
    
    private bool isGrounded;
/
    public Camera playerCamera;

    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private bool isGrapple;
    public float grappleDistance = 4f;
    public LayerMask grappleMask;
    
    
    /// <summary>
    /// Really big script to get the character hook shot working
    /// </summary>
    private enum State
    {
        // has 3 states which the player can be in for hookshot
      Normal,
      HookshotThrown,
      HookshotFlyingPlayer,
    }

    private void Awake()
    {
        //starts in normal state
        state = State.Normal;
        
        hookshotTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        //switch that handles the player hookshot
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
        //grounded check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // takes input from unity controller
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //moves player based on transform
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(Vector3.ClampMagnitude(move, 1) * speed * Time.deltaTime);
       
        //jumps
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }
        
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.deltaTime);
       
        //momentum drag, i dont understand this fully
        move += momentum;
        
        if (momentum.magnitude >= 0f)
        {
            float momentumDrag = 4f;
            momentum -= momentum * momentumDrag * Time.deltaTime;
            if (momentum.magnitude < .0f)
            {
                momentum = Vector3.zero;
            }
        }
        
        controller.Move(move * Time.deltaTime);
    }
    
    private void ResetGravity()
    {
        velocity.y = -2f;
    }

    private void HandleHookshotstart()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        { //shoots a raycast where you click
            if(Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out RaycastHit raycastHit,50f ))
            {
                //moves the mouse location cubeto where you clicked
               debugHitPointTransform.position = raycastHit.point;
               hookshotPosition = raycastHit.point;
               hookshotSize = 0f;
               hookshotTransform.gameObject.SetActive(true);
               hookshotTransform.localScale = Vector3.zero;

               //check to see if it hits grapple layer
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
        //looks where we threw the hook shot
        hookshotTransform.LookAt(hookshotPosition);
        //throws it
        float hookshotThrowSpeed = 180f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        
        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
        }
    }
    
    private void HandleHookshotMovement()
    {
        //gets the direction that we clicked in
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;
        float hsSpeedMax = 90f;
        float hsSpeedMin = 40f;
        
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position,hookshotPosition), hsSpeedMin, hsSpeedMax);
        float hookshotSpeedMultiplier = 1.5f;
        
        //moves the player
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);
        
        float jumpSpeed = 4.5f;
        float momentumExtra = 3.5f;
        float reachedHookshotPositionDistance = 5f;
        
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            hookshotTransform.gameObject.SetActive(false);
          //  momentum = hookshotDir * hookshotSpeed * momentumExtra;
          if (Input.GetMouseButton(1))
          { 
              ResetGravity();
              state = State.Normal;
          }
        }
        //fires hookshot
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            momentum += hookshotDir * hookshotSpeed * momentumExtra;
            StopHookshot();
        }
        
        if (Input.GetButton("Jump"))
        {
            momentum += hookshotDir * hookshotSpeed * jumpSpeed;
            //momentum += Vector3.up * jumpSpeed;
            StopHookshot();
        }
    }
    private void StopHookshot()
    {
        state = State.Normal;
           ResetGravity();
           hookshotTransform.gameObject.SetActive(false);
    }
}
