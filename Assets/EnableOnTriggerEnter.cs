using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Renderer rendererToEnable;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            rendererToEnable.enabled = true;
    }
}
