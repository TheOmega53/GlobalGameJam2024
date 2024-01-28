using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player's transform to follow
    public float smoothSpeed = 0.125f;
    public float smoothTime = 1.0f;
    public float Xoffset = 5f; // Offset from the player
    public float Yoffset = 6f; // Offset from the player

    private Vector3 velocity = Vector3.zero;


    void LateUpdate()
    {
        if (target != null && Time.timeScale != 0f)
        {
            // Follow the player horizontally and vertically
            Vector3 desiredPosition = new Vector3(target.position.x + Xoffset, target.position.y + Yoffset, transform.position.z);

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;

            // Keep the camera's rotation fixed
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}                                                                                                    
