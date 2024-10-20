using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private AudioSource gameOverSound;

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
        gameOverSound.Play();
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }
}