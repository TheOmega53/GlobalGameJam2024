using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player's transform to follow
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the player


    void LateUpdate()
    {
        if (target != null)
        {
            // Keep the camera's Y position fixed
            float desiredY = transform.position.y;
            float smoothedY = Mathf.Lerp(transform.position.y, desiredY, smoothSpeed);

            // Follow the player horizontally
            Vector3 desiredPosition = new Vector3(target.position.x, smoothedY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Keep the camera's rotation fixed
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
