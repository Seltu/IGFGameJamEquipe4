using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _animalNumber;
    [SerializeField] private Animator _anim;

    private void Awake()
    {
        EventManager.onUpdateAnimalCountEvent += UpdateText;
        EventManager.onDeathEvent += PlayPositiveFeedback;
        EventManager.onPlayerGotHitEvent += PlayNegativeFeedback;
    }

    private void OnDestroy()
    {
        EventManager.onUpdateAnimalCountEvent -= UpdateText;
        EventManager.onDeathEvent -= PlayPositiveFeedback;
        EventManager.onPlayerGotHitEvent -= PlayNegativeFeedback;
    }

    private void UpdateText(int count)
    {
        _animalNumber.text = "x" + count;
    }

    private void PlayNegativeFeedback()
    {
        _anim.SetTrigger("PlayerHit");
    }

    private void PlayPositiveFeedback(GameObject dead)
    {
            Debug.Log("mamagueavi");
        if(dead.CompareTag("Enemy"))
        {
            Debug.Log("woah");
            _anim.SetTrigger("Positive");
        }
    }
}
