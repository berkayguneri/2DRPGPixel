using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After Image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("Camera Shake")]
    private CinemachineImpulseSource cameraShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    [Space]
    [SerializeField] private ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CameraShake(Vector3 _shakePower)
    {
        cameraShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        cameraShake.GenerateImpulse();
    }

    public void CreateAfteerImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;

            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position + new Vector3(0, 0.6f), transform.rotation);

            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, spriteRenderer.sprite);

            //AudioManager.instance.PlaySFX(12, null);

            AudioManager.instance.PlaySFX(37, null);
        }

    }

    public void PlayDustFx()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
