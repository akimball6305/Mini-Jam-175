using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance; // Singleton instance for easy access

    public float time = 0; // Timer
    public bool isTimerRunning = true;
    public TextMeshProUGUI timerText; // UI text element to display the timer

   /* private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate timers
            return;
        }
    } */

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

    public float GetElapsedTime()
    {
        return time;
    }
}
