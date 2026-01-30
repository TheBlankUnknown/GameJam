using UnityEngine;
using UnityEngine.InputSystem;

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;

    [Header("UI")]
    public GameObject buyMenuUI;

    [Header("Input")]
    public InputActionAsset inputActions; // assign your Input System asset here

    [Header("References")]
    public PlayerMovement player; // assign your player script here

    public bool IsOpen { get; private set; }

    private InputAction buyAction;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        buyMenuUI.SetActive(false);

        // Setup Buy action from your InputActions asset
        if (inputActions != null)
        {
            var map = inputActions.FindActionMap("Player");
            if (map != null)
            {
                buyAction = map.FindAction("Buy");
                if (buyAction != null)
                    buyAction.performed += ctx => ToggleMenu();
                buyAction?.Enable();
            }
        }
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

        // Reset player look input immediately to prevent camera snapping
        if (player != null)
            player.ResetLookInput();

        // Optional: still block camera input briefly
        ThirdPersonCamera cam = FindObjectOfType<ThirdPersonCamera>();
        if (cam != null)
        {
            cam.blockLookInput = true;
            cam.blockLookInput = false; // we reset immediately since mouse delta is already cleared
        }

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
