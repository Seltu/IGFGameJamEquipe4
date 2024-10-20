using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ThrowObject _throwObject;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _maxForce = 0.1f;
    [SerializeField] private float _separationDistance = 1.5f;
    [SerializeField] private float _separateForce = 1.5f;
    [SerializeField] private float _obstacleAvoidanceDistance = 5f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _avoidForce = 2f;
    [SerializeField] private Transform _target;  // Target for boids to follow
    [SerializeField] private float _attackTime = 1f;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;

    private List<AnimalController> _neighbors = new();
    private Vector3 _velocity;
    private Vector3 _acceleration;
    private float _yPosition;  // Store the initial Y position
    private float _speedMultiplier = 2f;
    private bool _attacking;
    private float _attackTimer;
    private bool _isDead = false;

    private void Start()
    {
        _velocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * _maxSpeed;
        _yPosition = transform.position.y;  // Store the initial Y position to keep boids on the same plane
    }

    private void Update()
    {
        if(_isDead) return;
        
        Vector3 separation = Separate(_neighbors) * _separateForce;
        Vector3 alignment = Align(_neighbors) * 0f;
        Vector3 cohesion = Cohere(_neighbors) * 1f;
        Vector3 avoidance = AvoidObstacles() * _avoidForce;
        Vector3 seekTarget = SeekTarget() * 1f;

        if (_attackTimer > 0)
            _attackTimer -= Time.deltaTime;
        else if (_attacking)
        {
            EventManager.OnTakeDamageTrigger(_attackDamage, _target.gameObject);
            _attackTimer = _attackTime;
        }

        // Apply behaviors
        ApplyForce(separation);
        ApplyForce(alignment);
        ApplyForce(cohesion);
        ApplyForce(avoidance);
        if (!_attacking)
            ApplyForce(seekTarget);

        // Move the boid (restricted to X and Z)
        Move();
    }

    public void SetNeighbors(List<AnimalController> neighbors)
    {
        this._neighbors = neighbors;
    }

    public ThrowObject GetThrowObject()
    {
        return _throwObject;
    }

    private Vector3 Separate(List<AnimalController> neighbors)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.transform.position);
            if (distance < _separationDistance)
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                diff.Normalize();
                if (distance > 0)
                    diff /= distance;
                steer += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steer /= count;
        }

        if (steer.magnitude > 0)
        {
            steer = steer.normalized * _maxSpeed - _velocity;
            steer = Vector3.ClampMagnitude(steer, _maxForce);
        }

        return steer;
    }

    private Vector3 Align(List<AnimalController> neighbors)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            sum += neighbor._velocity;
            count++;
        }

        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * _maxSpeed;
            Vector3 steer = sum - _velocity;
            return Vector3.ClampMagnitude(steer, _maxForce);
        }

        return Vector3.zero;
    }

    private Vector3 Cohere(List<AnimalController> neighbors)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            sum += neighbor.transform.position;
            count++;
        }

        if (count > 0)
        {
            sum /= count;
            return Seek(sum);
        }

        return Vector3.zero;
    }

    private Vector3 SeekTarget()
    {
        if (_target != null)
        {
            var distance = Vector3.Distance(transform.position, _target.position);
            _attacking = (_target.CompareTag("Door") || _target.CompareTag("Enemy")) && distance < 2f;
            return Seek(_target.position) * Vector3.Distance(transform.position, _target.position);
        }
        return Vector3.zero;
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired = desired.normalized * _maxSpeed;
        Vector3 steer = desired - _velocity;
        return Vector3.ClampMagnitude(steer, _maxForce);
    }

    private Vector3 AvoidObstacles()
    {
        Vector3 avoidanceForce = Vector3.zero;

        // Cast rays in the boid's current forward direction to check for obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _velocity.normalized, out hit, _obstacleAvoidanceDistance, _obstacleMask))
        {
            // Calculate a direction to steer away from the obstacle
            Vector3 avoidDirection = Vector3.Reflect(_velocity, hit.normal);
            avoidanceForce = avoidDirection.normalized * _maxSpeed;
            avoidanceForce -= _velocity;
            avoidanceForce = Vector3.ClampMagnitude(avoidanceForce, _maxForce);
        }

        return avoidanceForce;
    }

    private void ApplyForce(Vector3 force)
    {
        _acceleration += force;
    }

    public Transform GetTarget()
    {
        return _target;
    }

    public void SetTarget(Transform target)
    {
        this._target = target;
    }

    public void SetCollider(bool state)
    {
        _collider.enabled = state;
    }

    private void Move()
    {
        _velocity += _acceleration;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        // Constrain velocity to X and Z axes (set Y to 0)
        _velocity.y = 0;

        // Apply movement (only in X and Z axes)
        _rb.velocity = _velocity * _speedMultiplier;

        _spriteRenderer.flipX = _rb.velocity.x < 0;
        /* Rotate to face the direction it's moving (ignore Y rotation)
        if (velocity != Vector3.zero)
        {
            Vector3 flatForward = new Vector3(velocity.x, 0, velocity.z).normalized;
            transform.forward = flatForward;
        }*/

        _acceleration = Vector3.zero;
    }

    internal void SetSpeed(float v)
    {
        _speedMultiplier = v;
    }

    public void AnimalDeath()
    {
        _velocity = Vector3.zero;
        _isDead = true;
    }
}
