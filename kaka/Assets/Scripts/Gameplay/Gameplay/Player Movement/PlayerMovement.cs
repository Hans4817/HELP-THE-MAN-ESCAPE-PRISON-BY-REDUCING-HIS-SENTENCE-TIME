using System;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("Camera & Rotation")]
    public Camera playerCamera;
    public float lookSpeed = 0.5f;
    public float lookXLimit = 90f; // Limits vertical pitch (Up/Down)

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
    private float rotationX = 0f;
    private CharacterController characterController;
    private bool canMove = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // 1. Handle Crouching
        float currentWalkSpeed = walkSpeed;
        float currentRunSpeed = runSpeed;

        if (Keyboard.current[Key.LeftCtrl].isPressed && canMove) 
        {
            characterController.height = crouchHeight;
            currentWalkSpeed = crouchSpeed;
            currentRunSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
        }

        // 2. Calculate Movement Direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Keyboard.current[Key.LeftShift].isPressed;

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

            // Vertical pitch (Look Up/Down) - Clamped to prevent flipping upside down
            rotationX += -mouseDelta.y * lookSpeed * 0.1f;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Horizontal yaw (Look Left/Right) - Unclamped 360-degree rotation
            transform.Rotate(Vector3.up * (mouseDelta.x * lookSpeed * 0.1f));
        }
    }

    public void SetMovement(bool statement) => canMove = statement;
}