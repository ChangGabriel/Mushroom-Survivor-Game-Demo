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

    // Camerashake related
    [SerializeField] private float cameraShakeCooldown;
    private bool canShake;

    private void Awake()
    {
       Instance = this;
       cinemachineVCam = GetComponent<CinemachineVirtualCamera>();
       canShake = true;
    }

    public void shakeCamera(float intensity, float time)
    {
        //Shake Cam
        if (canShake)
        {
            cinemachineShake = cinemachineVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineShake.m_AmplitudeGain = intensity;
            shakeTimer = time;
            StartCoroutine(shakeCooldown(cameraShakeCooldown));
            canShake = false;
        }
        
    }

    private void Update()
    {
        // resets thet shake amplitude
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineShake.m_AmplitudeGain = 0f;
            }
        }
    }

    //handles timer for Camera shaking
    public IEnumerator shakeCooldown(float cameraShakeTimer)
    {
        yield return new WaitForSeconds(cameraShakeTimer);
        canShake = true;
    }

}
