using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private BossController _bossPrefab;
    [SerializeField] private Collider _combatCollider;
    private BossController _spawnedBoss;
    private bool _defeated = true;
    private void Start()
    {
        EventManager.onDeathEvent += CheckBossDeath;
    }

    private void OnDestroy()
    {
        EventManager.onDeathEvent -= CheckBossDeath;
    }

    private void CheckBossDeath(GameObject deadObject)
    {
        if (_defeated) return;
        if (deadObject.Equals(_spawnedBoss.gameObject))
        {
            EventManager.OnGameWinTrigger();
            MusicManager.Instance.StopMusic();
            _defeated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EventManager.OnCombatStartTrigger();
        _combatCollider.enabled = false;
        StartCoroutine(SpawnBoss());
    }

    private IEnumerator SpawnBoss()
    {
        _defeated = false;
        var boss = Instantiate(_bossPrefab, transform.position, Quaternion.identity);
        boss.enabled = false;
        boss.tag = "Untagged";
        _spawnedBoss = boss;
        Time.timeScale = 0f;
        EventManager.OnBossSpawnTrigger();
        MusicManager.Instance.PlayMusic(1);
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        boss.enabled = true;
        boss.tag = "Enemy";
    }
}
