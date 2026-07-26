using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickToOpenShop : MonoBehaviour
{
    [SerializeField] private float maxReachDistance = 5f;

    [SerializeField] private CanvasGroup ShopCanva;

    private Camera playerCamera;

    private void Start()
    {
        // Automatically find the main camera if not manually assigned
        if (playerCamera == null)
        {
            playerCamera = FindFirstObjectByType<Camera>();
        }
    }

    private void Update()
    {
        // Detect left mouse click while the cursor is locked
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (playerCamera == null) return;

            // Shoots a ray out from the exact center of the screen (0.5, 0.5)
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, maxReachDistance))
            {
                // Check if the object we aimed at and clicked is THIS object
                if (hit.transform == transform)
                {
                    OnObjectClicked();
                }
            }
        }
        else if(Keyboard.current[Key.E].isPressed)
        {
            OnObjectClicked();
        }
    }

    private void OnObjectClicked()
    {
        if(ShopCanva.alpha == 0f) // if its invisible
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMovement.Instance.SetMovement(false);
            ShopCanva.alpha = 1f;          // Invisible
            ShopCanva.interactable = true; // Disable button clicks
            ShopCanva.blocksRaycasts = true; // Allow clicks to pass through  
        } 
    }
}
