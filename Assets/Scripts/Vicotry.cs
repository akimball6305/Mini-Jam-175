using UnityEngine;
using UnityEngine.SceneManagement;

public class Vicotry : MonoBehaviour 
{
    
    public int Respawn;
    // [SerializeField] GameObject deathUI;
    //[SerializeField] GameObject mainUI;

    public ScoreKeeper scoreKeeper;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // deathUI.SetActive(true);
            // scoreKeeper.StopTimer();
            // mainUI.SetActive(false);
            SceneManager.LoadScene(Respawn);
            Debug.Log("hit");
        }
    }

}


