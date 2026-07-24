using UnityEngine;
// 1. You must import the new Input System namespace
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera & Rotation")]
    public Camera playerCamera;
    public float lookSpeed = 0.5f; // Decreased slightly as Mouse.current returns raw deltas
    public float lookXLimit = 45f; // Limits vertical pitch (Up/Down)
    public float lookYLimit = 45f; // Limits horizontal yaw (Left/Right)

    [Header("Movement Speeds")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;

    [Header("Mechanics")]
    public float jumpPower = 7f;
    public float gravity = 20f;

    [Header("Height Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0; // Tracks clamped Y rotation
    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Capture initial starting rotation
        rotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // Ensure keyboard and mouse devices are connected and available
        if (Keyboard.current == null || Mouse.current == null) return;

        // 1. Handle Crouching
        float currentWalkSpeed = walkSpeed;
        float currentRunSpeed = runSpeed;

        if (Keyboard.current[Key.LeftCtrl].isPressed && canMove) 
        {
            characterController.height = crouchHeight;
            // slower walk/run speed when crouching
            currentWalkSpeed = crouchSpeed;
            currentRunSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
        }

        // 2. Calculate Movement Direction (Replacing Input.GetAxis)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Keyboard.current[Key.LeftShift].isPressed;

        // Read WASD keys manually to mimic the old Vertical/Horizontal axes
        float moveVertical = 0f;
        if (Keyboard.current[Key.W].isPressed) moveVertical += 1f;
        if (Keyboard.current[Key.S].isPressed) moveVertical -= 1f;

        float moveHorizontal = 0f;
        if (Keyboard.current[Key.D].isPressed) moveHorizontal += 1f;
        if (Keyboard.current[Key.A].isPressed) moveHorizontal -= 1f;

        float curSpeedX = canMove ? (isRunning ? currentRunSpeed : currentWalkSpeed) * moveVertical : 0;
        float curSpeedY = canMove ? (isRunning ? currentRunSpeed : currentWalkSpeed) * moveHorizontal : 0;
        
        float movementDirectionY = moveDirection.y; 
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 3. Handle Jumping & Gravity
        if (characterController.isGrounded)
        {
            if (moveDirection.y < 0) 
            {
                moveDirection.y = -2f; 
            }

            if (Keyboard.current[Key.Space].wasPressedThisFrame && canMove)
            {
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection.y = movementDirectionY - (gravity * Time.deltaTime);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // 4. Handle Camera and Player Rotation
        if (canMove && playerCamera != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            // Vertical pitch clamping
            rotationX += -mouseDelta.y * lookSpeed * 0.1f;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Horizontal yaw clamping
            rotationY += mouseDelta.x * lookSpeed * 0.1f;
            rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
            transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}