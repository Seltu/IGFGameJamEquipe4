using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _animalNumber;

    private void Awake()
    {
        EventManager.onUpdateAnimalCountEvent += UpdateText;
    }

    private void OnDestroy()
    {
        EventManager.onUpdateAnimalCountEvent += UpdateText;
    }

    private void UpdateText(int count)
    {
        _animalNumber.text = "x" + count;
    }
}
