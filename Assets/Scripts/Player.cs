using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int health = 10;
    public int currentHealth;
    
    public HealthBar healthBar;
    
    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
