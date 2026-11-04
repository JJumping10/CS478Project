using UnityEngine;

public class InitialiseLevel : MonoBehaviour
{

    public int currentLevel;


    void Awake()
    {
        GameManager gameManager = GameManager.instance;

        gameManager.UI.SetActive(true);
        gameManager.pauseMenu.SetActive(false);
        gameManager.SetLevel(currentLevel);
        gameManager.ResetAttempts();
        gameManager.GameTimer.Start();
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
