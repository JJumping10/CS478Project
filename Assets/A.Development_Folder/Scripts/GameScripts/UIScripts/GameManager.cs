using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public ItemManager ItemManager;

    public int score = 0;
    const int MAX_SCORE = 9999999;
    const int ATTEMPT_POINTS = 10000;
    const int BASE_ATTEMPTS = 4; // # of attempts which give no points
    public int attempts = 1;
    public int level = 4;
    const int MAX_LEVEL = 5;
    public Timer GameTimer;

    public event Action<int> OnScoreUpdate;
    public event Action<int> OnLevelUpdate;
    public event Action<float> OnTimeUpdate;
    public event Action<int> OnAttemptsUpdate;

    private InputActionReference PauseResume;
    public InputActionAsset inputActions;

    [SerializeField] public TMP_Text bonusPointsBox;
    public GameObject UI;
    public GameObject debugMenu;
    public GameObject pauseMenu;
    [SerializeField] public Pause pause;

    public Boolean isPaused = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (instance != this)
        {
            Destroy(transform.root.gameObject);
        }
        inputActions.FindActionMap("Player").Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (levelNumber > 0 && levelNumber <= MAX_LEVEL)
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

    public IEnumerator AttemptsToPoints()
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

    public void Pause()
    {
        isPaused = true;
        GameTimer.Stop();
        Time.timeScale = 0f;
        inputActions.FindActionMap("Player").Disable();
    }

    public void NextLevel()
        {
            Pause();
            StartCoroutine(LoadNextLevel());
        }


    public IEnumerator LoadNextLevel()
    {
        var sceneName = SceneManager.GetActiveScene().name;//Get the scene name to compare to other scenes
        string nextScene;
        int background;
        yield return StartCoroutine(GameTimer.TimeToPoints());
        yield return AttemptsToPoints();
        yield return new WaitForSecondsRealtime(2f);

        switch (sceneName)
        {
            case "PlatformerLevel1Scene": nextScene = "PlatformerLevel2Scene"; background = 0; break;
            case "PlatformerLevel2Scene": nextScene = "PlatformerLevel3Scene"; background = 1; break;
            case "PlatformerLevel3Scene": nextScene = "PlatformerLevel4Scene"; background = 1; break;
            case "PlatformerLevel4Scene": nextScene = "PlatformerLevel5Scene"; background = 1; break;
            case "PlatformerLevel5Scene": nextScene = "PlatformerLevel1Scene"; background = 0; break;
            default: nextScene = "TitleScreen"; background = 0; break;
        }

        //playerScript.updatedPos = new Vector2(0,0.5f);                                                          //Signify spawn to beginning of a new level
        //player.transform.position = playerScript.updatedPos;                                                    //Move the character to the new spawn
        //playerBackground.GetComponent<Image>().sprite = backgroundImageScript.ChangeBackground(background);     //Change background to next level
        //DontDestroyOnLoad(GameObject.Find("Player"));                                                           //Move the player game object to new scene
        SceneManager.LoadScene(nextScene);                                                                      //Load scene
    }


}
