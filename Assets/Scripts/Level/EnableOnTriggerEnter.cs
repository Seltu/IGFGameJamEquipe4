using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Renderer rendererToEnable;
    [SerializeField] private bool disableInstead;
    private bool triggeredOnce;
    private void OnTriggerEnter(Collider other)
    {
        if (triggeredOnce) return;
        if (other.CompareTag("Player"))
        {
            rendererToEnable.enabled = !disableInstead;
            triggeredOnce = true;
        }
    }
}
