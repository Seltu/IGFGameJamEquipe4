using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _winDelay = 3;
    [SerializeField] private AudioClip _victoryClip;

    void Start()
    {
        EventManager.onGameWinEvent += BigWin;
    }

    private void OnDestroy()
    {
        EventManager.onGameWinEvent -= BigWin;    
    }

    private void BigWin()
    {
        StartCoroutine(VictoryRoutine());
    }

    private IEnumerator VictoryRoutine()
    {
        _audioSource.PlayOneShot(_victoryClip);
        yield return new WaitForSeconds(_winDelay);
        _victoryScreen.SetActive(true);
    }
}
