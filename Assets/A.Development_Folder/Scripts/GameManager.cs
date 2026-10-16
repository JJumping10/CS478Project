using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using NUnit.Framework;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public float score = 0;
    public const int MAX_SCORE = 9999999;
    public const int BASE_ATTEMPTS = 4; // # of attempts which give no points

    public int level = 0;
    public const int MAX_LEVEL = 5;

    public int attempts = 1;
    public const int ATTEMPT_POINTS = 10000;

    public Timer GameTimer;

    public event Action<float> OnScoreUpdate;
    public event Action<int> OnLevelUpdate;
    public event Action<float> OnTimeUpdate;
    public event Action<int> OnAttemptsUpdate;

    [SerializeField] public TMP_Text bonusPointsBox;
    public GameObject debugMenu;

    public void Awake()
    {
Debug.Log($"Awake on {gameObject.name}, ID: {GetEntityId()}");
        if (instance == null)
        {
            instance = this;
        }
        else
        {
Debug.Log("Duplicate GameManager destroyed");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
Debug.Log($"GameTimer assigned: {GameTimer != null}");
    }

    private void OnDestroy()
    {
        if (instance = this)
        {
            instance = null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
//Debug.Log("GameManager Update started");
//Debug.Log($"GameTimer is null: {GameTimer == null}");
        GameTimer.Tick(Time.deltaTime);
        OnTimeUpdate?.Invoke(GameTimer.GameTime);

        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftAlt) &&
        Input.GetKey(KeyCode.LeftShift) &&
        Input.GetKeyDown(KeyCode.D))
        {
            debugMenu.SetActive(!debugMenu.activeSelf);
        }
        #endif
    }

    public void AddScore(float points)
    {
        // score cannot be negative or more than the UI can display
        float newScore = score + points;
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
        if (levelNumber >= 1 && levelNumber <= MAX_LEVEL)
        {
            level = levelNumber;
            OnLevelUpdate?.Invoke(levelNumber);
        }
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

    public void AddAttemptsPoints()
    {
        StartCoroutine(AttemptsToPoints());
    }

    private IEnumerator AttemptsToPoints()
    {
        int totalPoints = ATTEMPT_POINTS * (BASE_ATTEMPTS - attempts);
        attempts = 0;
        OnAttemptsUpdate?.Invoke(attempts);
        bonusPointsBox.gameObject.SetActive(true);
        Color bonusColour;
        string sign = "+";
        if (totalPoints > 0)
        {
            bonusColour = Color.green;
        }
        else if (totalPoints < 0)
        {
            bonusColour = Color.red;
            sign = "";
        }
        else
        {
            bonusColour = Color.white;
        }
        bonusPointsBox.color = bonusColour;
        bonusPointsBox.text = $"{sign}{totalPoints}";
        yield return new WaitForSecondsRealtime(0.75f);
        AddScore(totalPoints);
        yield return new WaitForSecondsRealtime(0.50f);
        bonusPointsBox.gameObject.SetActive(false);
    }

}
