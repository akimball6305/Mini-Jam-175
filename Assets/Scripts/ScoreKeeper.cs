using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance; // Singleton instance for easy access

    public int score = 0; // Player's base score
    public float time = 0; // Timer
    public bool isTimerRunning = true;

    public TextMeshProUGUI scoreText; // UI text element to display the score
    public TextMeshProUGUI timerText; // UI text element to display the timer

    public float timeBonusMultiplier = 10f; // Multiplier to scale time bonus
    public int finalScore = 0; // Final calculated score for the win scene

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of ScoreManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep score between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetGame();

    }

    private void Update()
    {
        if (isTimerRunning)
        {
            time += Time.deltaTime; // Increment the timer
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(time);
        }
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

    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        time = 0;
        UpdateTimerUI();
    }

    public int CalculateFinalScore()
    {
        // Formula for final score
        float timeBonus = Mathf.Max(0, (1 / time) * timeBonusMultiplier); // Higher bonus for shorter times
        finalScore = Mathf.RoundToInt(score + timeBonus);
        return finalScore;
    }

    public int GetFinalScore()
    {
        return finalScore;
    }

    public void ResetGame()
    {
        // Reset the score and timer
        score = 0;
        time = 0;

        // Reset the timer running state if necessary
        isTimerRunning = true;

        // Update the UI
        UpdateUI();
    }

}
