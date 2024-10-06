using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private float moveSpeed = 5f; // Speed of player movement
    public int point;
    public Vector2 movingDrection;
    public float rotationSpeed = 100f; // Speed of rotation

    LevelUpManager levelUpManager;

    private void Start()
    {
        levelUpManager = FindAnyObjectByType<LevelUpManager>();
    }

    void Update()
    {
        // WASD controls for fish flock movement
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys for horizontal movement
        float vertical = Input.GetAxis("Vertical");     // W/S keys for vertical movement

        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        // If no input is given, continue moving in the last direction
        if (inputDirection.magnitude == 0)
        {
            inputDirection = transform.up;
        }

        movingDrection = inputDirection;

        // Calculate the final direction
        Vector2 direction = movingDrection;

        // Calculate the target rotation based on direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the player in the final direction
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    public void eat()
    {
        point += 1;
        levelUpManager.eat();
    }
}
