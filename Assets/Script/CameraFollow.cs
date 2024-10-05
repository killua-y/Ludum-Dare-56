using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform fish; // Drag the fish object to this in the Inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        // Move the camera to follow the fish
        Vector3 desiredPosition = fish.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
