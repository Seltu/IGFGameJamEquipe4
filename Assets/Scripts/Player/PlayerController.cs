using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator; // Reference to Animator

    void Start()
    {
        // Get the Rigidbody component (3D, not 2D)
        rb = GetComponent<Rigidbody>();

        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input from the player (WASD or arrow keys)
        // Use X for left-right (horizontal) and Z for forward-backward (vertical) in 3D
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Check if the player is moving
        bool isMoving = movement.x != 0 || movement.z != 0;

        // Update the animator with the movement state
        if (isMoving)
        {
            animator.Play("Move"); // Play "Move" animation when moving
        }
        else
        {
            animator.Play("Still"); // Play "Still" animation when standing still
        }
    }

    void FixedUpdate()
    {
        // Move the character in the XZ plane (3D movement while ignoring Y for a flat plane)
        rb.velocity = movement * moveSpeed;
    }
}
