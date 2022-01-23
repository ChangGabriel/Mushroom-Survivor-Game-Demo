using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour

{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVCam;
    private CinemachineBasicMultiChannelPerlin cinemachineShake;
    private float shakeTimer;

    private void Awake()
    {
       Instance = this;
       cinemachineVCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCamera(float intensity, float time)
    {
        cinemachineShake = cinemachineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineShake.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineShake.m_AmplitudeGain = 0f;
            }
        }
    }
}   
