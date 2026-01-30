using UnityEngine;
using UnityEngine.InputSystem;

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;
    public GameObject buyMenuUI;

    private DefaultInputActions inputActions;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inputActions = new DefaultInputActions();
    }

    void Start()
    {
        buyMenuUI.SetActive(false);
    }

    public void OpenMenu()
    {
        buyMenuUI.SetActive(true);
        Time.timeScale = 0f;

        inputActions.Player.Disable(); // stop movement/shooting

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        buyMenuUI.SetActive(false);
        Time.timeScale = 1f;

        inputActions.Player.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleMenu()
    {
        if (buyMenuUI.activeSelf)
            CloseMenu();
        else
            OpenMenu();
    }
}
