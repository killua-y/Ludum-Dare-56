using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of player movement
    private Vector2 movement;
    public int point;

    // Update is called once per frame
    void Update()
    {
        // Get input from the user for horizontal and vertical movement
        float moveX = Input.GetAxisRaw("Horizontal"); // Left (-1) and Right (1)
        float moveY = Input.GetAxisRaw("Vertical");   // Down (-1) and Up (1)

        // Normalize the vector to maintain consistent diagonal speed
        movement = new Vector2(moveX, moveY).normalized;
    }

    // Called at fixed time intervals for physics calculations
    void FixedUpdate()
    {
        // Move the player by applying the movement vector
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void eat()
    {
        point += 1;
    }

}
