using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotatingIdol : MonoBehaviour
{
    public float gizmo;
    public float speed = 10f;
    [SerializeField] GameObject player;
     public LayerMask lPlayer;
     private bool range;

     public GameObject door;
     
     
/// <summary>
/// rotated the idle before deleted. was also going to activate a door
/// </summary>
    
    private void Start()
    {
        player = GameObject.Find("Player");
        door = GameObject.Find("EndDoor");
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
            SceneManager.LoadScene(sceneBuildIndex: 0);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gizmo);
   
    }
}
