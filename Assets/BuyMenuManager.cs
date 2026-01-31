using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // If using TextMeshPro

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;
    public GameObject buyMenuUI;
    public bool IsOpen { get; private set; }

    [Header("Player UI")]
    public GameObject playerUIRoot;          // The main player UI root
    public TMP_Text buyMenuMoneyText;        // Text element inside Buy Menu to show money

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

        // Hide main UI
        if (playerUIRoot != null)
            playerUIRoot.SetActive(false);

        // Copy the current money from main UI
        if (buyMenuMoneyText != null && playerUIRoot != null)
        {
            TMP_Text mainMoneyText = playerUIRoot.GetComponentInChildren<TMP_Text>();
            if (mainMoneyText != null)
                buyMenuMoneyText.text = mainMoneyText.text;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        IsOpen = false;
        buyMenuUI.SetActive(false);

        // Show main UI
        if (playerUIRoot != null)
            playerUIRoot.SetActive(true);

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
