using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float waitTime = 2.0f;
    public float timeLeft;
    public bool playing = false;
    public bool autoStart = false;
    public Action onTimerComplete;

    void Start()
    {
        timeLeft = waitTime;
        if (autoStart)
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (playing)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                playing = false;
                timeLeft = 0f;
                onTimerComplete?.Invoke();
            }
        }
    }

    public void StartTimer()
    {
        playing = true;
        timeLeft = waitTime;
    }
}
