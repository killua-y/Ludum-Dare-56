using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform[] seaImages; // Array to hold the three sea images
    public float imageWidth = 19f; // The width of each sea image
    public float seaSurface = 5f;  // Highest point player can reach
    public float seaFloor = -5f;   // Lowest point player can reach
    public Transform player;       // Reference to the player object

    private void Update()
    {
        // Limit the player's Y position between seaSurface and seaFloor
        //Vector3 playerPos = player.position;
        //playerPos.y = Mathf.Clamp(playerPos.y, seaFloor, seaSurface);
        //player.position = playerPos;

        // Check if the player reaches the leftmost image
        if (player.position.x < seaImages[0].position.x + imageWidth / 2)
        {
            ScrollRight();
        }
        // Check if the player reaches the rightmost image
        else if (player.position.x > seaImages[2].position.x - imageWidth / 2)
        {
            ScrollLeft();
        }
    }

    // Moves the rightmost image to the left to simulate endless scrolling
    public void ScrollRight()
    {
        // Move the rightmost image to the left of the leftmost image
        Transform rightMostImage = seaImages[2];
        rightMostImage.position = new Vector3(seaImages[0].position.x - imageWidth, rightMostImage.position.y, rightMostImage.position.z);

        // Shift the images in the array
        ShiftImagesArrayRight();
    }

    // Moves the leftmost image to the right to simulate endless scrolling
    public void ScrollLeft()
    {
        // Move the leftmost image to the right of the rightmost image
        Transform leftMostImage = seaImages[0];
        leftMostImage.position = new Vector3(seaImages[2].position.x + imageWidth, leftMostImage.position.y, leftMostImage.position.z);

        // Shift the images in the array
        ShiftImagesArrayLeft();
    }

    // Shift the sea images array to the right (0 -> 1 -> 2 -> 0)
    private void ShiftImagesArrayRight()
    {
        Transform temp = seaImages[2];
        seaImages[2] = seaImages[1];
        seaImages[1] = seaImages[0];
        seaImages[0] = temp;
    }

    // Shift the sea images array to the left (0 -> 1 -> 2 -> 0)
    private void ShiftImagesArrayLeft()
    {
        Transform temp = seaImages[0];
        seaImages[0] = seaImages[1];
        seaImages[1] = seaImages[2];
        seaImages[2] = temp;
    }
}
