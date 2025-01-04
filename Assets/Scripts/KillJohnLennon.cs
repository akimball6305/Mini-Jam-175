using UnityEngine;
using UnityEngine.SceneManagement;
public class KillJohnLennon : MonoBehaviour
{
    public int Respawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Respawn);
        }
    }

}
