using UnityEngine;
using UnityEngine.SceneManagement;
public class KillJohnLennon : MonoBehaviour
{
    public int Respawn;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject mainUI;

    ScoreKeeper scoreKeeper;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathUI.SetActive(true);
            scoreKeeper.StopTimer();
            mainUI.SetActive(false);
        }
    }

}
