using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;
    [SerializeField] private float _upForce;
    [SerializeField] private float _randomForce;

    public void InpulseThrow()
    {   
        Debug.Log("UAAAAA");
        
        _rb.useGravity = true;

        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = Mathf.Abs(randomDirection.y); 

        Vector3 force = (Vector3.up * _upForce) + (randomDirection * _randomForce);

        _rb.velocity = Vector3.zero;
        _col.enabled = false;
        _rb.AddForce(force, ForceMode.Impulse);
    }
}
