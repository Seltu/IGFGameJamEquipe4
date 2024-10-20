using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Events
    public delegate void OnGameOver();
    public static event OnGameOver onGameOverEvent;

    public delegate void OnShakeCamera(float intensity, float frequency, float duration);
    public static event OnShakeCamera OnShakeCameraEvent;

    public delegate void OnDeath(GameObject deadObject);
    public static event OnDeath onDeathEvent;

    public delegate void OnTakeDamage(float damageTaken, GameObject damagedObject);
    public static event OnTakeDamage onTakeDamageEvent;

    public delegate void OnPlayerGotHit();
    public static event OnPlayerGotHit onPlayerGotHitEvent; 

    public delegate void OnUpdateAnimalCount(int count);
    public static event OnUpdateAnimalCount onUpdateAnimalCountEvent;
    
    public delegate void OnCombatStart();
    public static event OnCombatStart onCombatStartEvent;

    public delegate void OnCombatEnd();
    public static event OnCombatStart onCombatEndEvent;
    #endregion



    #region Triggers
    public static void OnGameOverTrigger()
    {
        onGameOverEvent?.Invoke();
    }

    public static void OnShakeCameraTrigger(float intensity, float frequency, float duration)
    {
        OnShakeCameraEvent?.Invoke(intensity, frequency, duration);
    }

    public static void OnDeathTrigger(GameObject deadObject)
    {
        onDeathEvent?.Invoke(deadObject);
    }

    public static void OnTakeDamageTrigger(int damageTaken, GameObject damagedObject)
    {
        onTakeDamageEvent?.Invoke(damageTaken, damagedObject);
    }

    public static void OnPlayerGotHitTrigger()
    {
        onPlayerGotHitEvent?.Invoke();
    }

    public static void OnUpdateAnimalCountTrigger(int count)
    {
        onUpdateAnimalCountEvent?.Invoke(count);
    
    }
    public static void OnCombatStartTrigger()
    {
        onCombatStartEvent?.Invoke();
    }

    public static void OnCombatEndTrigger()
    {
        onCombatEndEvent?.Invoke();
    }
    #endregion
}
