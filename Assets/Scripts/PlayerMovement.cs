using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    public int speedIndex = 1;
    public float[] speedLevels = { 5f, 7f, 10f };
    private float speed;

    public float jumpForce = 12f; // Adjust this value for a punchier jump
    public float quickFallForce = 20f; // Additional force to make the character fall faster
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isSliding;
    private bool isFalling;

    public LayerMask obstacleLayer;

    private Animator animator;

    private AudioController audioController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioController = GetComponent<AudioController>();
    }

    void Update()
    {        
        // Set Speed
        speed = speedLevels[speedIndex];

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Run
        Vector3 moveDirection = new Vector3(1, 0f, 0f);
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
        if (!audioController.IsPlaying())
        {
            audioController.PlayFootstep();
        }

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zero out the y component to avoid adding to the current y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jumping");
        }

        // Quick fall
        if (rb.velocity.y < 0f) { isFalling = true; } else { isFalling = false; }
        if (isFalling && !isGrounded)
        {
            rb.AddForce(Vector3.down * quickFallForce, ForceMode.Acceleration);
        }

        // Slide
        if (Input.GetButtonDown("Slide"))
        {
            isSliding = true;
            // Adjust collider or animation for sliding            
        }
        else if (Input.GetButtonUp("Slide"))
        {
            isSliding = false;
            // Reset collider or animation for standing
        }


        // Animation Flags
        //animator.SetBool("isGrounded", isGrounded);
        //animator.SetBool("isSliding", isSliding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((obstacleLayer.value & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log("collision occured");
            if(speedIndex != 0) 
            {
                speedIndex--;
            }
        }   
    }
}
