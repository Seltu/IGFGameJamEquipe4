using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Events
    public delegate void OnDeath();
    public static event OnDeath onDeathEvent;

    public delegate void OnTakeDamage(float damageTaken);
    public static event OnTakeDamage onTakeDamageEvent;
    #endregion



    #region Triggers
    public static void OnDeathTrigger()
    {
        onDeathEvent?.Invoke();
    }

    public static void OnTakeDamageTrigger(int damageTaken)
    {
        onTakeDamageEvent?.Invoke(damageTaken);
    }
    #endregion
}
