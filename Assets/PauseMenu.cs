using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI; // Drag PausePanel here
    private bool isPaused;
    private InputAction pauseAction;
    public TMP_InputField sensitivityInput;
    public ThirdPersonCamera mouseLook;

    private void Start()
    {
        float savedSens = PlayerPrefs.GetFloat("Sensitivity", 1f);

        sensitivityInput.text = savedSens.ToString("0.##");
        mouseLook.mouseSensitivity = savedSens;

        sensitivityInput.onEndEdit.AddListener(SetSensitivity);
    }

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
        else if (!isPaused && Time.timeScale == 0) return;
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
    public void SetSensitivity(string value)
    {
        if (float.TryParse(value, out float sens))
        {
            sens = Mathf.Clamp(sens, 0.1f, 10f);
            Debug.Log(sens);
            mouseLook.mouseSensitivity = sens;
            PlayerPrefs.SetFloat("Sensitivity", sens);

            sensitivityInput.text = sens.ToString("0.##");
        }
        else
        {
            // reset if invalid input
            sensitivityInput.text = mouseLook.mouseSensitivity.ToString("0.##");
        }
    }
}