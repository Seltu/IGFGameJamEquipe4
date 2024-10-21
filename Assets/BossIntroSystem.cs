using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroSystem : MonoBehaviour
{
    [SerializeField] private GameObject _bossIntroScreen;

    private void Start()
    {
        EventManager.onBossSpawnEvent += ActivateScreen;
    }

    private void OnDestroy()
    {
        EventManager.onBossSpawnEvent -= ActivateScreen;
    }

    private void ActivateScreen()
    {
        StartCoroutine(IntroSceneActivation());
    }

    private IEnumerator IntroSceneActivation()
    {
        _bossIntroScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        _bossIntroScreen.SetActive(false);
    }
}
