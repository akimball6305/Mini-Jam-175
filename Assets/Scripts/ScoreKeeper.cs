using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance; // Singleton instance for easy access

    public int score = 0; // Player's base score
    public int finalScore = 0; // Final calculated score for the win scene
    public TextMeshProUGUI scoreText; // UI text element to display the score

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of ScoreKeeper exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public int CalculateFinalScore()
    {
        // Placeholder for any additional calculations needed
        finalScore = score;
        return finalScore;
    }

    public int GetFinalScore()
    {
        return finalScore;
    }

    public void ResetScore()
    {
        score = 0;
        finalScore = 0;
        UpdateScoreUI();
    }
}
