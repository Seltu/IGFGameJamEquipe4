using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private float _delay;

    private void Start()
    {
        EventManager.onGameOverEvent += OnGameLoss;
    }

    private void OnDestroy()
    {
        EventManager.onGameOverEvent -= OnGameLoss;
    }

    private void OnGameLoss()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(_delay);
    }
}