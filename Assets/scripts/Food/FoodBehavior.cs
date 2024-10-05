using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Call the player's eat method
            collision.gameObject.GetComponent<PlayerMovements>().eat();
            FindAnyObjectByType<SpwanManager>().ReturnToPool(this.gameObject);
        }
    }
}
