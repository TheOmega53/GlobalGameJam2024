using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    public int speedIndex = 2;
    public float[] speedLevels = { 5f, 7f, 10f };
    private float speed;

    public float jumpForce = 12f; // Adjust this value for a punchier jump
    public float bounceForce = 5f;
    public float quickFallForce = 20f; // Additional force to make the character fall faster
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isBouncy;
    private bool isSliding;
    private bool isFalling;

    public LayerMask obstacleLayer;
    public LayerMask ItemLayer;

    public Animator animator;

    private AudioController audioController;

    private SphereCollider sphereCollider;

#if UNITY_ANDROID
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public float swipeThreshold = 50f; // Adjust the threshold as needed
    public enum SwipeDirection
    {
        Up,
        Down,
        None
    }
#endif


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioController = GetComponent<AudioController>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
#if UNITY_ANDROID
        //Check for swipe
        SwipeDirection swipe = DetectSwipe();
#endif
        // Set Speed
        speed = speedLevels[speedIndex];

        // Check if the player is grounded
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        if (!wasGrounded && isGrounded)
        {
            audioController.PlayLand();
            animator.SetBool("isGrounded", true);
        } // Play landing sound whenever player gets grounded
        wasGrounded = isGrounded;



        // Check if the player is on bouncy ground
        //isBouncy = Physics.CheckSphere(groundCheck.position, 0.1f, bouncyLayer);        

        // Run
        Vector3 moveDirection = new Vector3(1, 0f, 0f);
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
        if (!audioController.IsPlaying())
        {
            audioController.PlayFootstep();
        }

        // Jump
#if UNITY_ANDROID

        if (isGrounded)
        {
            if (swipe == SwipeDirection.Up || Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zero out the y component to avoid adding to the current y velocity
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (isBouncy) rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                Debug.Log("jumping");
                audioController.PlayJump();
            }
        }
#else
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zero out the y component to avoid adding to the current y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if(isBouncy) rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            Debug.Log("jumping");
            audioController.PlayJump();
        }
#endif

        // Do Jump Animation
        animator.SetBool("isGrounded", isGrounded);


        // Quick fall
        if (rb.velocity.y < 0f) { isFalling = true; } else { isFalling = false; }
        if (isFalling && !isGrounded)
        {
            rb.AddForce(Vector3.down * quickFallForce, ForceMode.Acceleration);
        }

        // Slide
#if UNITY_ANDROID
        if (swipe == SwipeDirection.Down)
        {
            if (!isSliding)
            {
                Slide();
            }
        }
#else
        if (Input.GetButtonDown("Slide"))
        {
            if (!isSliding)
            {
                Slide();
            }
        }
#endif


        // Animation Flags
    }

    public void Slide()
    {
        isSliding = true;
        // Adjust collider or animation for sliding            
        animator.SetBool("isSliding", isSliding);
        sphereCollider.center = new Vector3(sphereCollider.center.x, sphereCollider.center.y * 0.5f, sphereCollider.center.z);
        sphereCollider.radius = 0.5f * sphereCollider.radius;
        audioController.PlaySlide();
    }

    public void stopSlide()
    {
        isSliding = false;
        animator.SetBool("isSliding", isSliding);
        sphereCollider.radius = 2 * sphereCollider.radius;
        sphereCollider.center = new Vector3(sphereCollider.center.x, sphereCollider.center.y * 2f, sphereCollider.center.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        CollisionTriggerEvent triggerEvent = other.gameObject.GetComponent<CollisionTriggerEvent>();
        if (triggerEvent != null)
        {
            triggerEvent.InvokeEvent();
        }

        //if ((other.gameObject.tag == "CameraTrigger")        
    }


    public void AdjustPlayerSpeed(int amount)
    {
        if ((speedIndex + amount) < speedLevels.Length && speedIndex + amount >= 0)
        {
            speedIndex = speedIndex + amount;
        }

        if (speedIndex == 1)
        {
            GameController.Instance.Danger(true);
        }
        else
        {
            GameController.Instance.Danger(false);
        }

        if ((speedIndex) == 0)
        {
            //Game Over Logic
            GameController.Instance.GameOver();

        }

    }

#if UNITY_ANDROID
    SwipeDirection DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Record the starting touch position
                startTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // Record the ending touch position
                endTouchPosition = touch.position;

                // Calculate the swipe direction
                Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                // Check if the swipe meets the threshold for vertical movement
                if (Mathf.Abs(swipeDirection.y) > swipeThreshold)
                {
                    // Determine if it's a swipe up or down
                    if (swipeDirection.y > 0)
                    {
                        Debug.Log("Swipe Up");
                        // Handle swipe up
                        return SwipeDirection.Up;
                    }
                    else
                    {
                        Debug.Log("Swipe Down");
                        // Handle swipe down
                        return SwipeDirection.Down;
                    }
                }
            }
        }


        // No significant swipe detected
        return SwipeDirection.None;
    }
#endif
}


