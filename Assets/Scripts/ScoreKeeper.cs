using UnityEngine;
using TMPro;
using System;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance; // Singleton instance for easy access

    public int score = 0; // Player's score
    public float time = 0; // Timer
    public bool isTimerRunning = true;
    public TextMeshProUGUI scoreText; // UI text element to display the score
    public TextMeshProUGUI timerText;//UI text element to display the timer

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
        UpdateUI();
    }
    private void Update()
    {
        if (isTimerRunning)
        {
            time += Time.deltaTime; // Increment the timer by the time elapsed since the last frame
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
}
