using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        // Get the Rigidbody component (3D, not 2D)
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from the player (WASD or arrow keys)
        // Use X for left-right (horizontal) and Z for forward-backward (vertical) in 3D
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Move the character in the XZ plane (3D movement while ignoring Y for a flat plane)
        rb.velocity = movement * moveSpeed;
    }
}
