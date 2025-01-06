using UnityEngine;
using TMPro;

public class WinSceneManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    private void Start()
    {
        if (ScoreKeeper.instance != null)
        {
            int finalScore = ScoreKeeper.instance.CalculateFinalScore();
            finalScoreText.text = "Final Score: " + finalScore.ToString();
        }
        else
        {
            Debug.LogError("ScoreKeeper instance not found!");
        }
    }
}
