using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Camera mainCam;
    public CharacterController controller;
    public float speed = 12f;
    public float jumpHeight = 50f; // Adjust this value for desired jump height

    private Vector3 velocity; // Store vertical velocity for jumping

    void Awake()
    {
        mainCam = Camera.main; // Cache the camera reference
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = mainCam.transform.right * x + mainCam.transform.forward * z;

        // Prevent vertical (y) movement
        move.y = 0f; // Set the y component to zero

        // Apply gravity (assuming gravity is negative in your game)
        float gravityForce = Physics.gravity.y * Time.deltaTime;
        move.y += gravityForce; // Add gravity force

        // Jumping logic
        if (controller.isGrounded) // Check if the character is on the ground
        {
            if (Input.GetButtonDown("Jump")) // Space key pressed
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce); // Calculate jump velocity
            }
        }
        else
        {
            velocity.y += gravityForce; // Apply gravity when not grounded
        }

        // Normalize move if its magnitude is greater than 1 to prevent faster diagonal movement
        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }

        controller.Move(move * speed * Time.deltaTime);

        // Apply vertical velocity for jumping
        controller.Move(velocity * Time.deltaTime);
    }
}