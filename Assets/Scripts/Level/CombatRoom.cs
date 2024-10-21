using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoom : MonoBehaviour
{
    [SerializeField] private LevelInfoSO _levelInfoSO;
    [SerializeField] private Collider _combatCollider;
    private List<EnemyController> _spawnedEnemies = new();
    private bool _defeated = false;
    private bool _spawning = false;
    private void Start()
    {
        EventManager.onDeathEvent += CheckEnemyDeath;
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
            if (_spawnedEnemies.Count <= 0 && !_spawning)
            {
                EventManager.OnCombatEndTrigger();
                _defeated = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EventManager.OnCombatStartTrigger();
        _combatCollider.enabled = false;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        _spawning = true;
        foreach (EnemySpawn wave in _levelInfoSO.combatEncounters[Random.Range(0, _levelInfoSO.combatEncounters.Count)].waves)
        {
            for (int i = 0; i < wave.number; i++)
            {
                Vector3 randomizedPosition = new Vector3(transform.position.x + Random.Range(-10f, 10f), 0f,
                    transform.position.z + Random.Range(-10f, 10f));
                Debug.Log(randomizedPosition);
                var enemy = Instantiate(_levelInfoSO.enemyPrefabs[(int)wave.type], randomizedPosition, Quaternion.identity);
                enemy.enabled = false;
                enemy.tag = "Untagged";
                _spawnedEnemies.Add(enemy);
                yield return new WaitForSeconds(1f);
                enemy.enabled = true;
                enemy.tag = "Enemy";
            }
        }
        _spawning = false;
    }
}
