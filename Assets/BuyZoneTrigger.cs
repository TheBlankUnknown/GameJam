using UnityEngine;
using UnityEngine.InputSystem;

public class BuyZone : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference buyAction;

    private bool playerInside;

    void OnEnable()
    {
        buyAction.action.Enable();
        buyAction.action.performed += OnBuyPressed;
    }

    void OnDisable()
    {
        buyAction.action.performed -= OnBuyPressed;
        buyAction.action.Disable();
    }

    void OnBuyPressed(InputAction.CallbackContext context)
    {
        if (!playerInside)
            return;

        BuyMenu.Instance.ToggleMenu();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            BuyMenu.Instance.CloseMenu();
        }
    }
}
