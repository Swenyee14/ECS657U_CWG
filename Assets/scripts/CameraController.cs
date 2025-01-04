using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 100f;

    private Vector3 currentRotation;
    private float vertical = 0f;
    private void Start()
    {
        // Get the PlayerInput component attached to the same GameObject
        playerInput = GetComponent<PlayerInput>();

        // Find the "movement" action
        moveAction = playerInput.actions["movement"];

        speed = PlayerPrefs.GetInt("speed");
        rotationSpeed = PlayerPrefs.GetInt("sensitivity");
    }

    private void Update()
    {
        MoveCamera();

        // rotate camera only if right mouse button is held
        if (Mouse.current.rightButton.isPressed)
        {
            RotateCamera();
        }
        speed = PlayerPrefs.GetInt("speed");
        rotationSpeed = PlayerPrefs.GetInt("sensitivity");
    }

    // Method to move the camera/player
    void MoveCamera()
    {
        // Read movement input as a Vector3
        Vector3 inputDirection = moveAction.ReadValue<Vector3>();

        Vector3 forward = transform.forward;  
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        // Normalize vectors to prevent diagonal movement from being faster
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * inputDirection.z + right * inputDirection.x;

        // Apply the movement to the camera
        transform.position += moveDirection * speed * Time.deltaTime;

        // Apply vertical movement (up/down)
        transform.position += Vector3.up * inputDirection.y * speed * Time.deltaTime;
    }

    // Method to rotate the camera while holding the right mouse button
    void RotateCamera()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float horizontal = mouseDelta.x * rotationSpeed * Time.deltaTime;
        currentRotation.y += horizontal;

        float verticalDelta = -mouseDelta.y * rotationSpeed * Time.deltaTime; 
        vertical = Mathf.Clamp(vertical + verticalDelta, -90f, 90f);

        transform.eulerAngles = new Vector3(vertical, currentRotation.y, 0f);
    }
}