using UnityEngine;
using UnityEngine.InputSystem;

public class CounterHandler : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxReachDistance = 5f;

    [SerializeField] private int amountPerClick = 1;

    private void Start()
    {
        // Automatically find the main camera if not manually assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
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
    }

    private void OnObjectClicked()
    {
        CurrencyManager.Instance.AddMoney(amountPerClick);
    }
}