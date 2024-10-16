using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;

    [Header("Variables")]
    [SerializeField] private float _fireRate;
    [SerializeField] private float _minStopDistance;
    [SerializeField] private float _maxStopDistance;
    [SerializeField] private float _speed;
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

        if (distanceToPlayer > _stopDistance && !_hasReachedPlayer)
        {
            _direction.Normalize();

            transform.position += _direction * _speed * Time.deltaTime;
        }
        else if(distanceToPlayer < _stopDistance && !_hasReachedPlayer)
        {
            _hasReachedPlayer = true;
        }
        else if(distanceToPlayer > _stopDistance && _hasReachedPlayer)
        {
            _stopDistance = RandomizeStopDistance();
            _hasReachedPlayer = false;
        }
    }

    private void ShootPlayer()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        
        Vector3 direction = (_playerObject.transform.position - _shootPoint.position).normalized;

        bulletScript.GetBulletRigidBody().velocity = direction * bulletScript.GetBulletSpeed();

        _cooldown = _fireRate;
    }

    private float RandomizeStopDistance()
    {
        return Random.Range(_minStopDistance, _maxStopDistance);
    }
}
