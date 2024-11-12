using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _animalNumber;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _overlayAnim;

    private void Awake()
    {
        EventManager.onUpdateAnimalCountEvent += UpdateText;
        EventManager.onCreateNewAnimalEvent += PlayPositiveFeedback;
        EventManager.onPlayerGotHitEvent += PlayNegativeFeedback;
        EventManager.onCombatStartEvent += RetractMinimap;
        EventManager.onCombatEndEvent += ReturnMinimap;
    }

    private void OnDestroy()
    {
        EventManager.onUpdateAnimalCountEvent -= UpdateText;
        EventManager.onCreateNewAnimalEvent -= PlayPositiveFeedback;
        EventManager.onPlayerGotHitEvent -= PlayNegativeFeedback;
        EventManager.onCombatStartEvent -= RetractMinimap;
        EventManager.onCombatEndEvent -= ReturnMinimap;
    }

    private void ReturnMinimap()
    {
        _anim.SetBool("Open", false);
    }

    private void RetractMinimap()
    {
        _anim.SetBool("Open", true);
    }

    private void UpdateText(int count)
    {
        _animalNumber.text = "x" + count;
    }

    private void PlayNegativeFeedback()
    {
        _overlayAnim.SetTrigger("PlayerHit");
    }

    private void PlayPositiveFeedback(Transform dead)
    {
        if(dead.CompareTag("Enemy"))
        {
            //_overlayAnim.SetTrigger("Positive");
        }
    }
}
