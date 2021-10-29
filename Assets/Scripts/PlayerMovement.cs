using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public CharacterController controller;
    
    public float speed = 12f;

    
    void Update()
    {
        // takes input from unity controller 
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // creates a transform based off input
        
        Vector3 move = transform.right * x + transform.forward * z;
        
        // moves the character using speed and delta time 
        
        controller.Move(move * speed * Time.deltaTime);
    }
}
