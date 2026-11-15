using UnityEngine;

public class InitialiseLevel : MonoBehaviour
{

    public int currentLevel;
    [SerializeField] private GameObject ManagersPrefab;
    private GameObject managers;
    private GameManager gameManager;

    void Awake()
    {
        if (GameObject.Find("Managers") == null)
        {
            managers = Instantiate(ManagersPrefab);
            DontDestroyOnLoad(managers);
        }

        gameManager = GameManager.instance;

        gameManager.UI.SetActive(true);
        gameManager.isPaused = false;
        gameManager.SetLevel(currentLevel);
        gameManager.ResetAttempts();
        gameManager.GameTimer.Reset();
        gameManager.GameTimer.Start();
        gameManager.inputActions.FindActionMap("Player").Enable();
        Time.timeScale = 1f;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
