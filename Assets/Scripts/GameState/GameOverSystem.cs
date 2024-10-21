using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private AudioSource gameOverSound;
    [SerializeField] private float _lossDelay = 1.5f;
    [SerializeField] private AudioClip _gameOverclip;

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
        StartCoroutine(LossRoutine());
    }

    private IEnumerator LossRoutine()
    {
        gameOverSound.PlayOneShot(_gameOverclip);
        yield return new WaitForSeconds(_lossDelay);
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }
}