using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Events
    public delegate void OnDeath(GameObject deadObject);
    public static event OnDeath onDeathEvent;

    public delegate void OnTakeDamage(float damageTaken, GameObject damagedObject);
    public static event OnTakeDamage onTakeDamageEvent;
    #endregion



    #region Triggers
    public static void OnDeathTrigger(GameObject deadObject)
    {
        onDeathEvent?.Invoke(deadObject);
    }

    public static void OnTakeDamageTrigger(int damageTaken, GameObject damagedObject)
    {
        onTakeDamageEvent?.Invoke(damageTaken, damagedObject);
    }
    #endregion
}
