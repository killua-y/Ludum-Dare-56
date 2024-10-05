using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    private Transform target;
    public GameObject[] targets; // Array of GameObjects to keep in view


    private bool isBirdView = false;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Store the original camera size and position
        originalSize = cam.orthographicSize;
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (!isBirdView)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            if (targets.Length == 0)
                return;

            // Find the center and size of the area containing all targets
            Bounds bounds = CalculateBounds();

            // Update the camera position
            UpdateCameraPosition(bounds);

            // Update the camera's orthographic size to fit all targets
            UpdateCameraSize(bounds);
        }

        if (Input.GetKeyDown(resetKey))
        {
            if (isBirdView)
            {
                isBirdView = false;
                RestoreOriginalCamera();
            }
            else
            {
                isBirdView = true;
            }
        }
    }

    public void assignNewTarget(Transform newtarget)
    {
        target = newtarget;
        isBirdView = false;
    }

    public float padding = 2.0f;  // Padding around the objects
    public float minSize = 5.0f;  // Minimum orthographic size of the camera
    public KeyCode resetKey = KeyCode.F; // Key to reset camera to its original size

    private Camera cam;
    private float originalSize; // Store the original orthographic size
    private Vector3 originalPosition; // Store the original camera position


    // Restore the camera's original size and position
    void RestoreOriginalCamera()
    {
        cam.orthographicSize = originalSize;
        transform.position = originalPosition;
    }

    // Calculates the bounds that include all the GameObjects in the array
    Bounds CalculateBounds()
    {
        Bounds bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 1; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }
        return bounds;
    }

    // Updates the camera position based on the bounds
    void UpdateCameraPosition(Bounds bounds)
    {
        Vector3 newPosition = bounds.center;
        newPosition.z = transform.position.z; // Keep the camera's original Z position
        transform.position = newPosition;
    }

    // Adjusts the orthographic size of the camera to fit all objects
    void UpdateCameraSize(Bounds bounds)
    {
        float cameraHeight = bounds.size.y;
        float cameraWidth = bounds.size.x / cam.aspect;

        cam.orthographicSize = Mathf.Max(cameraHeight / 2 + padding, cameraWidth / 2 + padding / cam.aspect, minSize);
    }
}