using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    [SerializeField] private List<AnimalController> animalPrefabs;
    [SerializeField] private int initialAmount;
    private void Start()
    {
        for(int i = 0; i < initialAmount; i++) {

            Instantiate(animalPrefabs[Random.Range(0, animalPrefabs.Count)]);
        }
    }
}
