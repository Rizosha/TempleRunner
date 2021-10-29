using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    [SerializeField] float health = 180f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        // playeer can shoot me, take damage here
        Debug.Log("enemy take damage");
        health -= 5f;
    }
    
}
