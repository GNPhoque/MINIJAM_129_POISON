using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;

    [SerializeField] float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    [SerializeField] float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    [SerializeField] float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    float ShakeElapsedTime = 0f;
    bool isShaking;

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Start()
    {
        if (instance) Destroy(instance);
        instance = this;

        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (isShaking)
        {
            if (ShakeElapsedTime < ShakeDuration)
            {
                ShakeElapsedTime += Time.deltaTime;
            }
            else
            {
                StopCameraShake();
            }
        }
    }

    public void StartShake()
    {
        VirtualCamera = GameManager.instance.rooms[0].VirtualCamera;
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

            isShaking = true;
            ShakeElapsedTime = 0f;
        }
    }

    public void StopCameraShake()
    {
        isShaking = false;
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
    }
}
