using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _deathDelay;
    [SerializeField] private float _maxLife;
    private float _currentLife;

    protected void Start()
    {   
        // Events
        EventManager.onDeathEvent += CallDeathCoroutine;
        EventManager.onTakeDamageEvent += TakeDamage;

        //Logic
        _currentLife = _maxLife;
    }

    private void OnDestroy()
    {
        EventManager.onDeathEvent -= CallDeathCoroutine;
        EventManager.onTakeDamageEvent -= TakeDamage;
    }

    private void TakeDamage(float damageTaken)
    {
        if(_currentLife - damageTaken <= 0)
        {
            EventManager.OnDeathTrigger();
        }
        else
        {
            _currentLife -= damageTaken;
        }
    }

    protected void CallDeathCoroutine()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        //play animations and sound

        yield return new WaitForSeconds(_deathDelay);
        
        Destroy(gameObject);
    }
}
