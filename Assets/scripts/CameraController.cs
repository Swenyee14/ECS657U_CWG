using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float Speed = 4.0f;
    [SerializeField] private float Sensitivity = 2.0f;

    private Vector3 velocity = Vector3.zero;
    private float xRotation = 0f;

    private void Update()
    {
        HandleMovementInput();
        HandleCameraRotation();
    }

    private void HandleMovementInput()
    {
        // Get WASD input and translate to movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;

        // Handle vertical movement (space for up, shift for down)
        velocity.y = Input.GetKey(KeyCode.Space) ? 1f : (Input.GetKey(KeyCode.LeftShift) ? -1f : 0f);

        // Apply movement using the CharacterController
        Controller.Move(movement * Speed * Time.deltaTime);
        Controller.Move(velocity * Speed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        // Only rotate camera if right-click (Mouse1) is held down
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * Sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity;

        // Adjust camera's vertical rotation (up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera and player body
        PlayerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical (up/down)
        transform.Rotate(Vector3.up * mouseX); // Horizontal (left/right)
    }
}