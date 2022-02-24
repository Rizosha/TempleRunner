using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadoutside : MonoBehaviour
{
    /// <summary>
    /// Loads end scene
    /// </summary>

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
            SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
