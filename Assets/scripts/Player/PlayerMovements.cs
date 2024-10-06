using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private float moveSpeed = 5f; // Speed of player movement
    public int point;
    public TextMeshProUGUI foodText;
    public Vector2 movingDrection;

    private void Start()
    {
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

        // Calculate the rotation angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Update fish flock rotation and movement
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    public void eat()
    {
        point += 1;
        foodText.text = "point: " + point;
    }

}
