using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int score = 0;
    const int MAX_SCORE = 9999999;
    const int ATTEMPT_POINTS = 10000;
    const int BASE_ATTEMPTS = 4; // # of attempts which give no points
    public int attempts = 1;
    public int level = 0;
    public Timer GameTimer;

    public event Action<int> OnScoreUpdate;
    public event Action<int> OnLevelUpdate;
    public event Action<float> OnTimeUpdate;
    public event Action<int> OnAttemptsUpdate;

    [SerializeField] private InputActionReference PauseResume;

    [SerializeField] public TMP_Text bonusPointsBox;
    public GameObject UI;
    public GameObject debugMenu;

    public Boolean isPaused = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameTimer.Stop();
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void CalculateAttemptsPoints()
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
