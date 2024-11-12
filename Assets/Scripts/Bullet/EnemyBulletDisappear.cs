using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDisappear : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private void Start()
    {
        EventManager.onCombatEndEvent += PlayDisappear;
    }

    private void OnDestroy()
    {
        EventManager.onCombatEndEvent -= PlayDisappear;
    }

    private void PlayDisappear()
    {
        _anim.SetTrigger("Disappear");
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
