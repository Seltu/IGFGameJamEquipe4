using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Collider _col;
    [SerializeField] private Rigidbody _rb;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            EventManager.OnPlayerGotHitTrigger();
            Destroy(gameObject);
        }
        else if(col.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }

    public Rigidbody GetBulletRigidBody()
    {
        return _rb;
    }
}
