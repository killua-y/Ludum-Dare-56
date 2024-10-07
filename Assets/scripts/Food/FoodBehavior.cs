using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    Transform player;

    private float distance = 40f;
    private float interval = 3f;
    public float moveInterval = 1f;
    public float moveSpeed = 0.5f; // Adjust speed as necessary
    private Vector2 moveDirection;

    public bool isSeaFloor;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovements>().transform;
        StartCoroutine(CheckDistance());
        StartCoroutine(MoveRandomly());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (Vector2.Distance(this.transform.position, player.position) >= distance)
            {
                FindAnyObjectByType<SpawnManager>().ReturnToPool(this.gameObject);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // Choose a random direction (normalized to prevent diagonal speed increase)
            moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            // Move in that direction for a specified interval
            yield return new WaitForSeconds(moveInterval);
        }
    }

    void Update()
    {
        // Apply movement in the chosen direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the object is moving outside the y-bounds of -5 and +5
        if (transform.position.y > 4f)
        {
            // Reverse the Y direction to keep it in bounds
            moveDirection.y = -Mathf.Abs(moveDirection.y);
        }
        else if (transform.position.y < -4f)
        {
            // Reverse the Y direction to keep it in bounds
            moveDirection.y = Mathf.Abs(moveDirection.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isSeaFloor)
            {
                FindAnyObjectByType<LevelUpManager>().EatSeaFloorFood();
                Destroy(this.gameObject);
                FindAnyObjectByType<EffectManager>().PlayEffect(0, this.transform.position, 0.7f);
                FindAnyObjectByType<SoundManager>().PlaySound(0, 0);
            }
            else
            {
                Debug.Log("Collide with player");
                // Call the player's eat method
                collision.gameObject.GetComponent<PlayerMovements>().eat();
                FindAnyObjectByType<SpawnManager>().ReturnToPool(this.gameObject);
                FindAnyObjectByType<EffectManager>().PlayEffect(0, this.transform.position, 0.7f);
                FindAnyObjectByType<SoundManager>().PlaySound(0, 0);
            }
        }
    }
}
