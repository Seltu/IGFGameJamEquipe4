using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _visual;

    public void Explode()
    {
        Debug.Log("boom");
        EventManager.OnCreateNewAnimalTrigger(this.gameObject.transform);
        _visual.SetActive(false);
        _explosion.SetActive(true);
    }
}
