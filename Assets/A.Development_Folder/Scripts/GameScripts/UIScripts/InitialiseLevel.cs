using UnityEngine;

public class InitialiseLevel : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject UI;
    public GameObject PauseMenu;
    public int currentLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI.SetActive(true);
        PauseMenu.SetActive(false);
        gameManager.SetLevel(currentLevel);
        gameManager.ResetAttempts();
        gameManager.GameTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
