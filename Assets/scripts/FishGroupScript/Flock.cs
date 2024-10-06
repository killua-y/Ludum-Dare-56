using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed = 1f;           // Speed of the fish
    public float rotationSpeed = 2.0f;   // How fast the fish rotates in 2D
    public float neighborDistance = 5f; // Distance to consider other fish as neighbors
    public float avoidanceDistance = 0.5f; // Distance to avoid other fish
    public bool turning = false;         // Are we turning around the boundary of the tank?

    void Start()
    {
        speed = Random.Range(0.5f, 1.0f); // Each fish will have a slightly different speed
    }

    void Update()
    {
        // Check if the fish is about to leave the boundary of the tank (2D check)
        if (Vector2.Distance(transform.position, Vector2.zero) >= GlobalFlock.tankSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        // If turning, move back towards the center of the tank
        if (turning)
        {
            Vector2 direction = Vector2.zero - (Vector2)transform.position; // Direction to the center
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1.0f); // Adjust speed randomly
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules(); // Apply the flocking behavior rules
            }
        }

        // Move the fish forward based on the current speed (2D movement)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = GlobalFlock.allFish;

        Vector2 vcenter = Vector2.zero; // Center of the group
        Vector2 vavoid = Vector2.zero;  // Avoidance vector to prevent collisions
        float gSpeed = 0.1f;            // Group speed
        float dist;                     // Distance to other fish

        Vector2 goalPos = GlobalFlock.goalPos; // Global goal position
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector2.Distance(go.transform.position, this.transform.position);

                if (dist <= neighborDistance)
                {
                    vcenter += (Vector2)go.transform.position; // Calculate the center of the group
                    groupSize++;

                    // Avoid fish that are too close
                    if (dist < avoidanceDistance)
                    {
                        vavoid = vavoid + ((Vector2)this.transform.position - (Vector2)go.transform.position);
                    }

                    // Get the speed of the neighboring fish
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            // Calculate the average center of the group
            vcenter = vcenter / groupSize + (goalPos - (Vector2)this.transform.position);

            // Set the speed based on the group's average speed
            speed = gSpeed / groupSize;

            // Calculate the direction the fish should move
            Vector2 direction = (vcenter + vavoid) - (Vector2)transform.position;

            // Rotate the fish towards the new direction (in 2D)
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
