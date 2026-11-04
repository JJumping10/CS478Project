using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{

    public static UI instance;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text attempts;

    void Awake()
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
        gameManager.OnScoreUpdate += UpdateScore;
        UpdateScore(gameManager.score);
        gameManager.OnLevelUpdate += UpdateLevel;
        UpdateLevel(gameManager.score);
        gameManager.OnAttemptsUpdate += UpdateAttempts;
        gameManager.ResetAttempts();
        gameManager.OnTimeUpdate += UpdateTime;
        UpdateTime(gameManager.GameTimer.GameTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScore(int newScore)
    {
        score.text = $"{newScore:0000000}";
    }
    private void UpdateLevel(int levelNumber)
    {
        level.text = $"{levelNumber}";
    }
    private void UpdateTime(float newTime)
    {
        float timerTime = gameManager.GameTimer.GameTime;
        int minutes = Mathf.FloorToInt(timerTime / 60f);
        int seconds = Mathf.FloorToInt(timerTime % 60f);
        time.text = $"{minutes:00}{seconds:00}";
    }
    private void UpdateAttempts(int newAttempts)
    {
        attempts.text = $"{newAttempts}";
    }
}
