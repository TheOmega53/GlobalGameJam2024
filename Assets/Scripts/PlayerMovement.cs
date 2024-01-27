using System;
using UnityEditor.Build.Content;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioController = GetComponent<AudioController>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {        
        // Set Speed
        speed = speedLevels[speedIndex];

        // Check if the player is grounded
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
        if (!wasGrounded && isGrounded) { 
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
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Zero out the y component to avoid adding to the current y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if(isBouncy) rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            Debug.Log("jumping");
            audioController.PlayJump();
        }

        // Do Jump Animation
        animator.SetBool("isGrounded", isGrounded);


        // Quick fall
        if (rb.velocity.y < 0f) { isFalling = true; } else { isFalling = false; }
        if (isFalling && !isGrounded)
        {
            rb.AddForce(Vector3.down * quickFallForce, ForceMode.Acceleration);
        }

        // Slide
        if (Input.GetButtonDown("Slide"))
        {
            Slide();
        }


        // Animation Flags
    }

    public void Slide()
    {
        isSliding = true;
        // Adjust collider or animation for sliding            
        animator.SetBool("isSliding", isSliding);
        sphereCollider.radius = 0.5f * sphereCollider.radius;
        audioController.PlaySlide();    
    }

    public void stopSlide()
    {
        if (isSliding)
        {
            isSliding = false;
            animator.SetBool("isSliding", isSliding);
            sphereCollider.radius = 2 * sphereCollider.radius;
        }
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
        } else
        {
            GameController.Instance.Danger(false);
        }

        if ((speedIndex) == 0)
        {
            //Game Over Logic
            GameController.Instance.GameOver();
            
        }
        
    }
}


