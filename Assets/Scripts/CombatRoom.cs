using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    [SerializeField] private LevelInfoSO _levelInfoSO;
    [SerializeField] private Collider _combatCollider;
    private List<EnemyController> _spawnedEnemies = new();
    private bool _defeated;
    private void Start()
    {
        EventManager.onDeathEvent += CheckEnemyDeath;
        foreach(EnemySpawn wave in _levelInfoSO.combatEncounters[Random.Range(0, _levelInfoSO.combatEncounters.Count)].waves)
        {
            for(int i = 0; i < wave.number; i++)
            {
                _spawnedEnemies.Add(Instantiate(_levelInfoSO.enemyPrefabs[(int)wave.type],
                    new Vector3(transform.position.x+Random.Range(0f, _combatCollider.bounds.extents.x), 0f, 
                    transform.position.z + Random.Range(0f, _combatCollider.bounds.extents.z)), Quaternion.identity));
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.onDeathEvent -= CheckEnemyDeath;
    }

    private void CheckEnemyDeath(GameObject deadObject)
    {
        if (_defeated) return;
        if (_spawnedEnemies.Exists(enemy => enemy.gameObject.Equals(deadObject)))
        {
            _spawnedEnemies.Remove(_spawnedEnemies.Find(enemy => enemy.gameObject.Equals(deadObject)));
        }
        if (_spawnedEnemies.Count <= 0)
        {
            EventManager.OnCombatEndTrigger();
            _defeated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        EventManager.OnCombatStartTrigger();
        _combatCollider.enabled = false;
        foreach(EnemyController enemy in _spawnedEnemies)
        {
            enemy.enabled = true;
            enemy.tag = "Enemy";
        }
    }
}
