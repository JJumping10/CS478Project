using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseButton : MonoBehaviour
{
    public Button pauseButton, resumeButton, quitButton;
    public GameObject resumeButtonGO, quitButtonGO;
    public bool isPaused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton.onClick.AddListener(PauseButtonClicked);//Top right button on the screen can be interacted and something happens
        resumeButton.onClick.AddListener(ResumeButtonClicked);//After pause button, displays a button in the middle of the screen to exit pause, back to game 
        quitButton.onClick.AddListener(QuitButtonClicked);//After pause button, displays a button in the middle of the screen to exit pause, back to title screen
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) PauseButtonClicked();
    }

    void PauseButtonClicked()
    {
        if (isPaused){
            resumeButtonGO.SetActive(false);
            quitButtonGO.SetActive(false);
            isPaused = !isPaused;
        } else {
            resumeButtonGO.SetActive(true);
            quitButtonGO.SetActive(true);
            isPaused = !isPaused;
        }
    }

    void ResumeButtonClicked()
    {
        if (isPaused){
            resumeButtonGO.SetActive(false);
            quitButtonGO.SetActive(false);
            isPaused = !isPaused;
        }
    }

    void QuitButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
