using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitlePlayButton : MonoBehaviour
{
    public Button playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //When the button on game is clicked, execute the "PlayButtonClicked" function
        playButton.onClick.AddListener(PlayButtonClicked); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayButtonClicked()//Changes scene to "PlatformerLevel1Scene"
    {
        SceneManager.LoadScene("PlatformerLevel1Scene");
    }
}
