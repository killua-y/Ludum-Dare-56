using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private float moveSpeed = 5f; // Speed of player movement
    private float scaleFactor = 1;
    public int point;
    public Vector2 movingDrection;
    public float rotationSpeed = 100f; // Speed of rotation

    LevelUpManager levelUpManager;

    private bool canMove = true;

    public Animator animator;

    private void Start()
    {
        levelUpManager = FindAnyObjectByType<LevelUpManager>();
    }

    void Update()
    {
        if (canMove)
        {
            // WASD controls for fish flock movement
            float horizontal = Input.GetAxis("Horizontal"); // A/D keys for horizontal movement
            float vertical = Input.GetAxis("Vertical");     // W/S keys for vertical movement

            Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

            // Flip the player on the Y axis when pressing A or D
            if (horizontal < 0)
            {
                // Flip to face left when pressing A
                transform.localScale = new Vector3(-1 * scaleFactor, 1 * scaleFactor, 1);
            }
            else if (horizontal > 0)
            {
                // Flip back to face right when pressing D
                transform.localScale = new Vector3(1 * scaleFactor, 1 * scaleFactor, 1);
            }

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
    }

    public void eat()
    {
        point += 1;
        levelUpManager.eat();
    }

    public void StopMoving()
    {
        canMove = false;
    }

    // 调用此函数来改变物体大小
    public void ResizeSpriteAndCollider(Vector2 Scale)
    {
        // 调整物体的缩放比例
        scaleFactor = Scale.x;
        transform.localScale = new Vector3(1 * scaleFactor, 1 * scaleFactor, 1);
    }

    // 改变游泳速度
    public void ChangeSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        Debug.Log("Now moving in speed :" + newSpeed);
    }

    // 改变游泳专向能力rotationSpeed
    public void ChangeRotation(float newRotation)
    {
        rotationSpeed = newRotation;
        Debug.Log("Now can rotate in :" + rotationSpeed);
    }

    // 改变物种贴图
    public void ChangeSprite(int index)
    {
        animator.SetInteger("AnimationInt", index);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy collided with the player
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FindAnyObjectByType<GameManager>().EndingFour();
        }
    }
}
