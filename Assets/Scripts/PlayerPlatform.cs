using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    public GameObject player;

    /// <summary>
    /// used to move a player on a platform  
    /// </summary>
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        player.transform.parent = null;
    }
}
