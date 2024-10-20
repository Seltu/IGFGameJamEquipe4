using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _cannonVisual;

    [Header("Variables")]
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletAmount = 1;
    [SerializeField] private float _bulletAngle = 0f;
    [SerializeField] private float _bulletSpeed = 8f;
    [SerializeField] private float _minStopDistance;
    [SerializeField] private float _maxStopDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _separationRadius = 2.0f;
    [SerializeField] private float _separationForce = 2.0f;
    [SerializeField] private AudioSource shotSound;
    private float _stopDistance;
    private float _cooldown;
    private Transform _playerObject;
    private Vector3 _direction;
    private bool _hasReachedPlayer;


    private void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        _stopDistance = RandomizeStopDistance();
        _hasReachedPlayer = false;
        _cooldown += _fireRate * Random.value;
    }

    private void Update()
    {
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;
        else
            ShootPlayer();

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        _direction = _playerObject.position - transform.position;
        _direction.y = 0;
        float distanceToPlayer = _direction.magnitude;

        // Get the separation force
        Vector3 separationForce = GetSeparationForce();

        // If the enemy is far from the player, follow the player while applying separation
        if (distanceToPlayer > _stopDistance && !_hasReachedPlayer)
        {
            _direction.Normalize();

            // Add separation force to the movement
            Vector3 finalDirection = (_direction + separationForce).normalized;

            transform.position += finalDirection * _speed * Time.deltaTime;
            Vector3 flatForward = new Vector3(finalDirection.x, 0, finalDirection.z).normalized;
            _visual.forward = Vector3.Lerp(_visual.forward, flatForward, Time.deltaTime);
        }

        Vector3 cannonDirection = new Vector3(_direction.x, 0, _direction.z);
        _cannonVisual.forward = cannonDirection;

        if (distanceToPlayer < _stopDistance && !_hasReachedPlayer)
        {
            _hasReachedPlayer = true;
        }
        else if (distanceToPlayer > _stopDistance && _hasReachedPlayer)
        {
            _stopDistance = RandomizeStopDistance();
            _hasReachedPlayer = false;
        }
    }



    private void ShootPlayer()
    {
        var angleStep = _bulletAngle / (_bulletAmount - 1); // Increment between each bullet's angle
        var currentAngle = -_bulletAngle / 2; // Start from negative half of the total angle

        for (var i = 0; i < _bulletAmount; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
            shotSound.Play();

            // Get the normalized direction towards the player
            Vector3 direction = (_playerObject.transform.position - _shootPoint.position).normalized;

            // Create a rotation based on the current angle around the Y-axis (horizontal plane for 3D)
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);

            // Apply the rotation to the direction
            Vector3 rotatedDirection = rotation * direction;

            // Set bullet velocity in the rotated direction
            bulletScript.GetBulletRigidBody().velocity = rotatedDirection * _bulletSpeed;
            // Increment the angle for the next bullet
            currentAngle += angleStep;

            _cooldown = _fireRate;
        }
    }



    private float RandomizeStopDistance()
    {
        return Random.Range(_minStopDistance, _maxStopDistance);
    }

    private Vector3 GetSeparationForce()
    {
        Vector3 separation = Vector3.zero;
        int nearbyEnemiesCount = 0;

        // Get all enemies within a certain radius
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, _separationRadius);

        foreach (Collider enemy in nearbyEnemies)
        {
            // Make sure the detected collider is an enemy and not the current one
            if (enemy.gameObject != this.gameObject && enemy.CompareTag("Enemy"))
            {
                // Calculate the distance and direction from the other enemy
                Vector3 directionAway = transform.position - enemy.transform.position;
                float distance = directionAway.magnitude;

                // The closer the enemy, the stronger the repulsion force
                if (distance > 0)
                {
                    separation += directionAway.normalized / distance;
                    nearbyEnemiesCount++;
                }
        }
    }

    // Average out the separation force if there are nearby enemies
    if (nearbyEnemiesCount > 0)
    {
        separation /= nearbyEnemiesCount;
    }

    return separation * _separationForce;
}
}
