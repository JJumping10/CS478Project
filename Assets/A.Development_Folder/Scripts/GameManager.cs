using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int score = 0;
    const int MAX_SCORE = 9999999;
    public int attempts = 1;
    public int level = 0;
    public Timer GameTimer;

    public event Action<int> OnScoreUpdate;
    public event Action<int> OnLevelUpdate;
    public event Action<float> OnTimeUpdate;
    public event Action<int> OnAttemptsUpdate;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        GameTimer = new Timer();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameTimer.Tick(Time.deltaTime);
    }

    public void AddScore(int points)
    {
        // score cannot be negative or more than the UI can display
        int newScore = score + points;
        if (newScore >= 0 && newScore <= MAX_SCORE)
        {
            score = newScore;
        }
        else if (newScore < 0) {
            score = 0;
        }
        else if (newScore > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        OnScoreUpdate?.Invoke(score);
    }

    public void SetLevel(int levelNumber)
    {
        level = levelNumber;
        OnLevelUpdate?.Invoke(levelNumber);
    }

    public void AddAttempt()
    {
        attempts++;
        OnAttemptsUpdate?.Invoke(attempts);
    }

    public void ResetAttempts()
    {
        attempts = 1;
        OnAttemptsUpdate?.Invoke(attempts);
    }

}
