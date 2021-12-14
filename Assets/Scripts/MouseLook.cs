
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;

    public Transform playerBody;

    public float xRotation = 0.1f;
    void Start()
    {
        // turns cursor off when played
        Cursor.lockState = CursorLockMode.Locked;
    }

   
    void Update()
    {
        // takes input from unity controller a multiplies it by defined sensitivity and delta time
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        // sets rotation to only be on the y axis and clamps view  
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // takes xRotation and applies it to the player model
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
