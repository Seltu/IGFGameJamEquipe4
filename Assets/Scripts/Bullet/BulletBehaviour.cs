using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private Collider _col;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private AudioSource impactSound;

    private void OnTriggerEnter(Collider col)
    {
        float impactAudioLenght = impactSound.clip.length;
        if(col.gameObject.CompareTag("Player"))
        {
            EventManager.OnPlayerGotHitTrigger();
            EventManager.OnShakeCameraTrigger(6, 3, 0.7f);
            impactSound.Play();
            Destroy(gameObject, impactAudioLenght);
        }
        else if(col.gameObject.layer == 3)
        {
            impactSound.Play();
            Destroy(gameObject, impactAudioLenght);
        }
    }

    public Rigidbody GetBulletRigidBody()
    {
        return _rb;
    }
}
