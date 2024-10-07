using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaScorpionBehavior : MonoBehaviour
{
    public float walkSpeed = 2f; // Speed of the enemy's random walk
    public float DestoryDistance = 40f;
    public float detectionRadius = 5f; // Radius to detect the player
    public float jumpForce = 10f; // Force applied when jumping towards the player
    public float jumpDelay = 1f; // Time to wait before jumping towards the player
    public float jumpCoolDown = 5f;
    public float directionChangeTimeMin = 1f; // Minimum time before changing direction
    public float directionChangeTimeMax = 2f; // Maximum time before changing direction
    private float directionChangeTimer;

    private float counter;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 walkDirection;
    private bool isJumping;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set a random initial walk direction
        walkDirection = new Vector2(Random.Range(-1f, 1f), 0).normalized;
        directionChangeTimer = Random.Range(directionChangeTimeMin, directionChangeTimeMax); // Set initial timer
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > DestoryDistance)
        {
            this.transform.position = new Vector3(player.transform.position.x + (Random.value > 0.5f ? 25f : -25f), -5, this.transform.position.z);

        }

        if (counter < jumpCoolDown)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
            // If the player is within the detection radius, start jumping coroutine
            if (distanceToPlayer <= detectionRadius && !isJumping)
            {
                StartCoroutine(JumpToPlayer());
            }
        }

        // If the enemy is not jumping, keep walking randomly
        if (!isJumping)
        {
            Walk();
        }
    }

    private void Walk()
    {
        // Move the enemy in the walking direction
        rb.velocity = new Vector2(walkDirection.x * walkSpeed, rb.velocity.y);

        // Flip the enemy's sprite based on the walking direction
        FlipSprite(walkDirection.x);

        // Reduce the direction change timer
        directionChangeTimer -= Time.deltaTime;

        // If the timer reaches zero, change direction and reset the timer
        if (directionChangeTimer <= 0f)
        {
            walkDirection = new Vector2(Random.Range(-1f, 1f), 0).normalized;
            directionChangeTimer = Random.Range(directionChangeTimeMin, directionChangeTimeMax); // Reset the timer
        }
    }

    private IEnumerator JumpToPlayer()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpDelay);
        animator.SetBool("AnimationBool", true);

        // Calculate the direction towards the player and jump
        Vector2 jumpDirection = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(jumpDirection.x * jumpForce, jumpForce);

        // Flip the enemy's sprite based on the jump direction
        FlipSprite(jumpDirection.x);

        yield return new WaitForSeconds(1f); // Wait 1 second before allowing another jump
        animator.SetBool("AnimationBool", false);
        isJumping = false;
    }

    private void FlipSprite(float direction)
    {
        // Flip the sprite when moving left (direction < 0) or right (direction > 0)
        if (direction < 0)
        {
            transform.localScale = new Vector3(5, transform.localScale.y, transform.localScale.z);
        }
        else if (direction > 0)
        {
            transform.localScale = new Vector3(-5, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a circle in the editor to visualize the detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
