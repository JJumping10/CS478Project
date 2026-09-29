using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    private AudioSource menuAudioSource;
    private AudioSource cameraAudioSource;
    public Slider musicVol;

    public AudioClip buttonClickSFX;
    public AudioClip buttonSelectSFX;

    public Toggle sfxToggle;

    public void LoadLevel()
    {
        SceneManager.LoadScene("PlatformerLevel1Scene");
    }

    public void Start()
    {
        menuAudioSource = GetComponent<AudioSource>();
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();
        musicVol.value = cameraAudioSource.volume;

        sfxToggle.isOn = Configs.SFX;
    }

    public void OnVolChange()
    {
        cameraAudioSource.volume = musicVol.value;
    }

    public void OnButtonClickSFX(string buttonName)
    {
        if(Configs.SFX)
        {
            Debug.Log("Button clicked: " + buttonName);
            menuAudioSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void OnButtonSelectSFX(BaseEventData data)
    {
        GameObject button = ((PointerEventData)data).pointerEnter;

        if(Configs.SFX)
        {
            Debug.Log("Hovered: " + button.name);
            menuAudioSource.PlayOneShot(buttonSelectSFX);
        }
    }

    public void onSFXToggle()
    {
        Configs.SFX = !Configs.SFX;
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
