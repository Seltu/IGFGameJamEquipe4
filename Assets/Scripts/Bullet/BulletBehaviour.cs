using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Collider _col;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _bulletSpeed;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player pegou Bala");
            Destroy(gameObject);
        }
        else if(col.gameObject.layer == 3)
        {
            Debug.Log("Bala perdida");
            Destroy(gameObject);
        }
    }

    public Rigidbody GetBulletRigidBody()
    {
        return _rb;
    }

    public float GetBulletSpeed()
    {
        return _bulletSpeed;
    }
}
