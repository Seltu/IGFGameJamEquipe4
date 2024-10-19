using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyType {Shooter, Shotgunner, Machinegunner, Octogun};
[Serializable]
public struct EnemySpawn
{
    public EnemyType type;
    public int number;
}
[Serializable]
public class CombatEncounter
{
    public List<EnemySpawn> waves;
}

[CreateAssetMenu]
public class LevelInfoSO : ScriptableObject
{
    public List<EnemyController> enemyPrefabs = new();
    public List<CombatEncounter> combatEncounters = new();
}
