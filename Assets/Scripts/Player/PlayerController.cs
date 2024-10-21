using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed = 5f; // Movement speed
    private Rigidbody _rb;
    private Vector3 _movement;
    private Animator _animator; // Reference to Animator

    void Start()
    {
        EventManager.onPlayerGotHitEvent += TriggerHitAnimation;

        // Get the Rigidbody component (3D, not 2D)
        _rb = GetComponent<Rigidbody>();

        // Get the Animator component attached to the player
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        EventManager.onPlayerGotHitEvent -= TriggerHitAnimation;
    }

    void Update()
    {
        // Get input from the player (WASD or arrow keys)
        // Use X for left-right (horizontal) and Z for forward-backward (vertical) in 3D
        _movement.x = Input.GetAxisRaw("Horizontal");
        _spriteRenderer.flipX = _movement.x > 0;
        _movement.z = Input.GetAxisRaw("Vertical");

        // Check if the player is moving
        bool isMoving = _movement.x != 0 || _movement.z != 0;

        // Update the animator with the movement state
        if (isMoving)
        {
            _animator.SetBool("PlayerWalk", true); // Play "Move" animation when moving
        }
        else
        {
            _animator.SetBool("PlayerWalk", false); // Play "Still" animation when standing still
        }
    }

    void FixedUpdate()
    {
        // Move the character in the XZ plane (3D movement while ignoring Y for a flat plane)
        _rb.velocity = _movement * _moveSpeed;
    }

    private void TriggerHitAnimation()
    {
        _animator.SetTrigger("PlayerHit");
    }
}
