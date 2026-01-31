using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public Slider musicSlider;
    public AudioSource musicSource;
    public bool isMenuOpen = false; // set from menu script

    void Update()
    {
        if (isMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
    }
    private void Start()
    {
        // Set initial slider value
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        musicSlider.value = savedVolume;
        musicSource.volume = savedVolume;
        Debug.Log("stop");
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        isMenuOpen = true;
        ShowMenu(true); // show menu on start
    }

    public void PlayGame()
    {
        Debug.Log("Game");
        ShowMenu(false);
        isMenuOpen=false;
    }

    public void QuitGame()
    {

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        //isMenuOpen = false;
        //Application.Quit();
        Debug.Log("Quit Game"); // works in editor
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void ShowMenu(bool show)
    {
        
        mainMenuUI.SetActive(show);
        Time.timeScale = show ? 0f : 1f;

        if (show)
        {
            Time.timeScale = 0f; // pause game
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isMenuOpen = true;
            Debug.Log("stop");
        }
        else
        {
            Time.timeScale = 1f; // resume game
                                 // Lock and hide cursor for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}