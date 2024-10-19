using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Renderer rendererToEnable;
    [SerializeField] private bool disableInstead;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            rendererToEnable.enabled = !disableInstead;
    }
}
