using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 5f;
    private float jumpingPower = 8f;
    private float height = 0.5f;

    [SerializeField] private IndividualPlayerController currentControlledPlayer;
    private Rigidbody2D rb;
    private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;

    private CameraFollow cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (currentControlledPlayer != null)
        {
            playerTransform = currentControlledPlayer.transform;
            cameraFollow.assignNewTarget(playerTransform);
            rb = currentControlledPlayer.gameObject.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        //}

        Flip();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = new Vector2(playerTransform.position.x, playerTransform.position.y - height);

        return Physics2D.OverlapCircle(position, 0.1f, groundLayer);
    }

    private void Flip()
    {
        if (currentControlledPlayer.isFacingRight && horizontal < 0f || !currentControlledPlayer.isFacingRight && horizontal > 0f)
        {
            currentControlledPlayer.isFacingRight = !currentControlledPlayer.isFacingRight;
            Vector3 localScale = playerTransform.localScale;
            localScale.x *= -1f;
            playerTransform.localScale = localScale;
        }
    }

    public void ChangePlayerControll(IndividualPlayerController iPlayerController)
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        currentControlledPlayer = iPlayerController;
        playerTransform = currentControlledPlayer.transform;
        cameraFollow.assignNewTarget(playerTransform);
        rb = currentControlledPlayer.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
