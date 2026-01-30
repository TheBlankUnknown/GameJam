using UnityEngine;
using UnityEngine.InputSystem;

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;
    public GameObject buyMenuUI;
    public bool IsOpen { get; private set; }
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
        IsOpen = true;

        buyMenuUI.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        IsOpen = false;

        buyMenuUI.SetActive(false);
        Time.timeScale = 1f;

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
