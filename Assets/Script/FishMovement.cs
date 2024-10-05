using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 5f;
    public float boundaryOffset = 0.5f; // To keep fish within view of the camera

    private Vector2 screenBounds;

    void Start()
    {
        // Get screen bounds based on camera's view
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        // Fish movement input (left/right)
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);

        // Keep the fish within the camera's bounds
        Vector3 fishPosition = transform.position;
        fishPosition.x = Mathf.Clamp(fishPosition.x, screenBounds.x * -1 + boundaryOffset, screenBounds.x - boundaryOffset);
        transform.position = fishPosition;
    }
}
