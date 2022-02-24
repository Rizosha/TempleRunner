using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load : MonoBehaviour
{
    // Start is called before the first frame update
    
void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
            SceneManager.LoadScene(sceneBuildIndex: 2);
    }
}

