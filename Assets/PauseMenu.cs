using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI; // Drag PausePanel here
    private bool isPaused;
    private InputAction pauseAction;

    void Awake()
    {
        pauseAction = new InputAction(
            "Pause",
            InputActionType.Button,
            "<Keyboard>/escape"
        );
        pauseAction.performed += _ => TogglePause();
    }

    void OnEnable() => pauseAction.Enable();
    void OnDisable() => pauseAction.Disable();

    void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}