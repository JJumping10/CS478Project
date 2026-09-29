using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private Text lives;
    private Text fruit;
    private PinkMan pinkMan;
    [SerializeField] protected GameObject inGameMenu;
    private bool menuPaused = false;
    Vector3 pauseMousePosition;
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        // Make in-game menu invisible
        inGameMenu.SetActive(false);
        // Get a reference to the Text components
        lives = GameObject.Find("Lives_Text").GetComponent<Text>();
        fruit = GameObject.Find("Fruit_Text").GetComponent<Text>();
        // Get a reference to the PinkMan component
        pinkMan = GameObject.Find("PinkMan").GetComponent<PinkMan>();
    }

    // Update is called once per frame
    void Update()
    {
        // On Escape key press
        OnEscPress();
        // Update the text field to display the number of lives
        lives.text = $"x{pinkMan.Lives.ToString()}";
        // Update the text field to display the number of fruits collected
        fruit.text = $"{pinkMan.ItemsCount.ToString()}";
/*
        //prevent buttons from highlighting immediately
        if (!canvasGroup.interactable && Input.mousePosition != pauseMousePosition)
        {
            canvasGroup.interactable = true;
        }
*/

    }

    void OnEscPress()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPaused)
            {
                // Resume game
                Resume();
            }
            else
            {
                // Pause game
                PauseMenu();
            }
        }
    }

    void PauseMenu()
    {
        // Make in-game menu visible
//        canvasGroup.interactable = false;
        inGameMenu.SetActive(true);
        pauseMousePosition = Input.mousePosition;
        // Pause game
        Time.timeScale = 0;
        menuPaused = true;
    }

    public void Resume()
    {
        // Make in-game menu invisible
        inGameMenu.SetActive(false);
        // Resume game
        Time.timeScale = 1;
        menuPaused = false;
    }

    public void ReturnToTitle()
    {
        // Load the title screen
        SceneManager.LoadScene("TitleScreen");
    }
}