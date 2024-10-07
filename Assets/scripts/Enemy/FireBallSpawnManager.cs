using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawnManager : MonoBehaviour
{
    public GameObject fireballPrefab; // Assign the fireball prefab in the inspector
    public Transform player; // Assign the player's transform in the inspector
    public float spawnInterval = 1.0f; // Adjust the spawn interval as needed

    void Start()
    {
        StartCoroutine(SpawnFireballs());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    IEnumerator SpawnFireballs()
    {
        while (true)
        {
            // Randomize the x position based on the player's position
            float xPosition = player.position.x + Random.Range(-40f, 40f);
            float yPosition = 10f;

            // Define the spawn position
            Vector2 spawnPosition = new Vector2(xPosition, yPosition);

            // Instantiate the fireball prefab at the spawn position with no rotation
            GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

            // Add a Rigidbody2D to the fireball if it doesn't have one already
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = fireball.AddComponent<Rigidbody2D>();
            }

            // Determine the fall direction: straight down, slightly left, or slightly right
            int fallDirection = Random.Range(0, 3);
            Vector2 fallVelocity;

            switch (fallDirection)
            {
                case 0: // Straight down
                    fallVelocity = new Vector2(0f, -5f);
                    break;
                case 1: // Slightly left
                    fallVelocity = new Vector2(-2f, -5f);
                    break;
                case 2: // Slightly right
                    fallVelocity = new Vector2(2f, -5f);
                    break;
                default:
                    fallVelocity = new Vector2(0f, -5f);
                    break;
            }

            // Apply the velocity to the fireball
            rb.velocity = fallVelocity;

            // Wait for the specified interval before spawning the next fireball
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
