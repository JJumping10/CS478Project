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
        pauseButton.onClick.AddListener(PauseButtonClicked);
        resumeButton.onClick.AddListener(ResumeButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonClicked();
        }
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
        SceneManager.LoadScene("TitleScene");
    }
}
