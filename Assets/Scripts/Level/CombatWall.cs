using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatWall : MonoBehaviour
{
    [SerializeField] private Animator _wallAnimator;
    [SerializeField] private Collider _blockerCollider;
    [SerializeField] private LifeSystem _breakableDoorLife;

    private void Start()
    {
        EventManager.onCombatStartEvent += Close;
        EventManager.onCombatEndEvent += Open;
    }

    private void OnDestroy()
    {
        EventManager.onCombatStartEvent -= Close;
        EventManager.onCombatEndEvent -= Open;
    }

    private void Open()
    {
        _wallAnimator.SetBool("Open", true);
        _blockerCollider.enabled = false;
        if (_breakableDoorLife != null)
        {
            _breakableDoorLife.enabled = true;
            _breakableDoorLife.tag = "Enemy";
        }
    }

    private void Close()
    {
        _wallAnimator.SetBool("Open", false);
        _blockerCollider.enabled = true;
        if (_breakableDoorLife != null)
        {
            _breakableDoorLife.enabled = false;
            _breakableDoorLife.tag = "Untagged";
        }
    }
}
