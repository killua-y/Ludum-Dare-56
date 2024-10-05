using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 2f;
    private float backgroundWidth;
    private Vector3 startPosition;

    void Start()
    {
        // Get the width of the background to know when to reposition it
        backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x * transform.localScale.x;
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the background left
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // If the background has moved off-screen, reset its position
        if (transform.position.x < -backgroundWidth)
        {
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        // Move the background to the right to loop
        transform.position = new Vector3(transform.position.x + 2 * backgroundWidth, transform.position.y, transform.position.z);
    }
}
