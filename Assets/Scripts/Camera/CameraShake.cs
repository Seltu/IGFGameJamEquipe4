using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _channelsPerlin;

    private void Awake()
    {
        EventManager.OnShakeCameraEvent += ShakeCamera;
    }

    private void OnDestroy()
    {
        EventManager.OnShakeCameraEvent -= ShakeCamera;
    }

    private void Start()
    {
        _channelsPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _channelsPerlin.m_AmplitudeGain = 0;
        _channelsPerlin.m_FrequencyGain = 0;
    }

    public void ShakeCamera(float intensity, float frequency, float duration)
    {
        _channelsPerlin.m_AmplitudeGain = intensity;
        _channelsPerlin.m_FrequencyGain = frequency;
        StartCoroutine(StopShaking(duration));
    }

    private IEnumerator StopShaking(float duration)
    {
        yield return new WaitForSeconds(duration);
        _channelsPerlin.m_AmplitudeGain = 0;
        _channelsPerlin.m_FrequencyGain = 0;
    }
}
