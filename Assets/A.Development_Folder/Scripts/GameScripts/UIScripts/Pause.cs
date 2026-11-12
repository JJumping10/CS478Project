using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;

    [SerializeField] private InputActionAsset inputActions;
    private InputAction PauseAction;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject InstructionMenu;
    [SerializeField] private GameObject SettingsMenu;

    void Awake()
    {
        PauseAction = inputActions.FindAction("Pause-Resume");
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseAction.WasPressedThisFrame())
        {
            if (SettingsMenu.activeSelf)
            {
                CloseSettings();
            }
            else if (InstructionMenu.activeSelf)
            {
                CloseInstructions();
            }
            else if (PauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        gameManager.isPaused = true;
        gameManager.GameTimer.Stop();
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        inputActions.FindActionMap("Player").Disable();
        PauseAction.Enable();
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameManager.GameTimer.Start();
        inputActions.FindActionMap("Player").Enable();
        gameManager.isPaused = false;
    }

    public void CloseInstructions()
    {
        InstructionMenu.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
