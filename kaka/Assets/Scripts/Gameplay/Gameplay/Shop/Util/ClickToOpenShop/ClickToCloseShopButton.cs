using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ClickToCloseShopButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup ShopCanva;

    [SerializeField] private Button myButton; // Assign in Inspector

    private void OnEnable()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (myButton != null)
        {
            // Always unsubscribe from UI events to prevent memory leaks or duplicate calls
            myButton.onClick.RemoveListener(OnButtonClicked);
        }
    }

    void OnButtonClicked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.Instance.SetMovement(true);
        ShopCanva.alpha = 0f;          // Invisible
        ShopCanva.interactable = false; // Disable button clicks
        ShopCanva.blocksRaycasts = false; // Allow clicks to pass through  
    }
}
