using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehavior : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the enemy collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().EndingFour();
        }
    }
}
