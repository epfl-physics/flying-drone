using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private PlayButton playButton;
    [SerializeField] private float maxTime = 99.99f;

    private float elapsedTime;
    private bool timerIsRunning;
    private bool maxTimeReached;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (timerIsRunning && !maxTimeReached)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= maxTime)
            {
                elapsedTime = maxTime;
                maxTimeReached = true;
            }
            UpdateTimeDisplay(elapsedTime);
        }
    }

    public void TogglePlayPause()
    {
        if (timerIsRunning)
        {
            PauseTimer();
        }
        else
        {
            StartTimer();
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
        playButton?.Play();
    }

    public void PauseTimer()
    {
        timerIsRunning = false;
        playButton?.Pause();
    }

    public void ResetTimer()
    {
        PauseTimer();
        elapsedTime = 0;
        UpdateTimeDisplay(elapsedTime);
        maxTimeReached = false;
    }

    private void UpdateTimeDisplay(float time)
    {
        if (timeDisplay)
        {
            timeDisplay.text = time.ToString("00.00");
        }
    }

    public void SetMaxTime(float time, bool reset = true)
    {
        maxTime = time;
        if (reset) ResetTimer();
    }
}
