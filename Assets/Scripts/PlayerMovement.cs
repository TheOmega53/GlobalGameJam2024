using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f; // Adjust this value for a punchier jump
    public float quickFallForce = 20f; // Additional force to make the character fall faster
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isSliding;
    private bool isFalling;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Run
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zero out the y component to avoid adding to the current y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Quick fall
        if(rb.velocity.y < 0f) { isFalling = true; }
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

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);
    }

    void FixedUpdate()
    {
        // Additional physics-related adjustments can be made here if needed
    }
}
