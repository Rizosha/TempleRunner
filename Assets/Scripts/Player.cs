using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int health = 10;
    public int currentHealth;
    
    public HealthBar healthBar;
    public int setIdol = 0;
    public int currentIdol;
    public bool hasIdol;
    
    /// <summary>
    /// attempted to make a last ditch effort for making a door unlock when you collect the idol. Doesn't do anything 
    /// </summary>
    
    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //TakeDamage(1);
        }
        if (currentIdol == 1)
        {
            hasIdol = true;
        }
        Debug.Log(currentIdol);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    
    public void IdolPickup(int key)
    {
        currentIdol += key;
    }
}
