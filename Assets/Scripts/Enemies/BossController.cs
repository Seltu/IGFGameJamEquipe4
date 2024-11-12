using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : EnemyController
{
    [SerializeField] private EnemyController _minionPrefab;
    private bool _shootingAround;
    private bool _stopShooting;
    private void Start()
    {
        base.Start();
        StartCoroutine(BossBehavior());
    }

    protected override void Update()
    {
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;
        else
        {
            if (_stopShooting) return;
            if(_shootingAround)
                ShootAround();
            else
                ShootPlayer();
        }
    }

    protected override void CheckEnemyDeath(GameObject deadObject)
    {
        if (gameObject.Equals(deadObject))
        {
            enabled = false;
            EventManager.OnCombatEndTrigger();
        }
    }

    private void ShootAround()
    {
        var angleStep = _bulletAngle / (_bulletAmount - 1); // Increment between each bullet's angle
        var currentAngle = -_bulletAngle / 2 + _tilt; // Start from negative half of the total angle

        for (var i = 0; i < _bulletAmount; i++)
        {
            BulletBehaviour bullet = Instantiate(_bulletPrefab, _shootPoint.position - Vector3.right*2, Quaternion.identity);
            shotSound.Play();

            // Get the normalized direction towards the player

            // Create a rotation based on the current angle around the Y-axis (horizontal plane for 3D)
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);

            // Apply the rotation to the direction
            Vector3 rotatedDirection = rotation * _direction;

            // Set bullet velocity in the rotated direction
            bullet.GetBulletRigidBody().velocity = rotatedDirection * _bulletSpeed;
            // Increment the angle for the next bullet
            currentAngle += angleStep;
        }
        angleStep = _bulletAngle / (_bulletAmount - 1); // Increment between each bullet's angle
        currentAngle = -_bulletAngle / 2 - _tilt; // Start from negative half of the total angle

        for (var i = 0; i < _bulletAmount; i++)
        {
            BulletBehaviour bullet = Instantiate(_bulletPrefab, _shootPoint.position + Vector3.right * 2, Quaternion.identity);
            shotSound.Play();

            // Get the normalized direction towards the player

            // Create a rotation based on the current angle around the Y-axis (horizontal plane for 3D)
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);

            // Apply the rotation to the direction
            Vector3 rotatedDirection = rotation * _direction;

            // Set bullet velocity in the rotated direction
            bullet.GetBulletRigidBody().velocity = rotatedDirection * _bulletSpeed;
            // Increment the angle for the next bullet
            currentAngle += angleStep;
        }

        _cooldown = _fireRate;
        _tilt += _bulletTilt;
    }

    private IEnumerator BossBehavior()
    {
        _bulletAngle = 0;
        _bulletAmount = 1;
        _shootingAround = true;
        _direction = Vector3.back;
        _fireRate = 0.2f;
        _bulletSpeed = 5f;
        _bulletTilt = -20f;
        _tilt = 0;
        yield return new WaitForSeconds(20f);
        _bulletAmount = 3;
        _bulletAngle = 60;
        _fireRate = 0.5f;
        _bulletTilt = 0f;
        _tilt = 0;
        _bulletSpeed = 8f;
        _shootingAround = false;
        yield return new WaitForSeconds(10f);
        _stopShooting = true;
        for (int i = 0; i < 4; i++)
        {
            Vector3 randomizedPosition = new Vector3(transform.position.x + Random.Range(-10f, 10f), 0f,
                transform.position.z + Random.Range(-10f, 10f));
            Debug.Log(randomizedPosition);
            var enemy = Instantiate(_minionPrefab, randomizedPosition, Quaternion.identity);
            enemy.enabled = false;
            enemy.tag = "Untagged";
            yield return new WaitForSeconds(1f);
            enemy.enabled = true;
            enemy.tag = "Enemy";
        }
        yield return new WaitForSeconds(10f);
        _bulletAngle = 360;
        _stopShooting = false;
        _bulletAmount = 8;
        _shootingAround = false;
        _direction = Vector3.back;
        _fireRate = 0.5f;
        _bulletSpeed = 4f;
        _bulletTilt = 22.5f;
        _tilt = 0;
        yield return new WaitForSeconds(15f);
        StartCoroutine(BossBehavior());
    }
}
