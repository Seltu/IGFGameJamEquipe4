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
            EventManager.OnShakeCameraTrigger(6, 3, 0.7f);
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
