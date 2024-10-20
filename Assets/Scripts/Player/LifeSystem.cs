using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _deathDelay;
    [SerializeField] private float _maxLife;
    private float _currentLife;

    public bool Trosso = false;
    public bool isDead = false;

    protected virtual void Start()
    {   
        // Events
        EventManager.onTakeDamageEvent += TakeDamage;

        //Logic
        _currentLife = _maxLife;
    }

    private void OnDestroy()
    {
        EventManager.onTakeDamageEvent -= TakeDamage;
    }

    private void TakeDamage(float damageTaken, GameObject damagedObject)
    {
        if (!damagedObject.Equals(gameObject)) return;
        if(_currentLife - damageTaken <= 0)
        {
            CallDeathCoroutine();
        }
        else
        {
            _currentLife -= damageTaken;
        }
    }

    protected void CallDeathCoroutine()
    {

        if (isDead) return;
        isDead = true;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        //play animations and sound
        yield return new WaitForSeconds(_deathDelay);
        EventManager.OnDeathTrigger(gameObject);
        Destroy(gameObject);
    }
}
