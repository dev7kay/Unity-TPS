using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state {get; private set; }

    private PlayerShooter gunHolder;
    private LineRenderer bulletLineRenderer;

    private AudioSource GunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    
    public Transform fireTransform;
    public Transform leftHandMount;

    public float damage = 25f;
    public float fireDistance = 100f;

    public int ammoRemain = 100;
    public int magAmmo;
    public int magCapacity = 30;

    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;

    [Range(0f, 10f)] public float maxSpread = 3f;
    [Range(1f, 10f)] public float stability = 1f;
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;
    private float currentSpread;
    private float currentSpreadVelocity;

    private float lastFireTime;

    private LayerMask excudeTarget;

    private void Awake()
    {
        GunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    public void SetUp(PlayerShooter gunHolder)
    {
        this.gunHolder = gunHolder;
        excudeTarget = gunHolder.excludeTarget;
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
        currentSpread = 0f;
        lastFireTime = 0f;
        state = State.Ready;
    }

    private void onDisable()
    {

    }

    public bool Fire(Vector3 aimTarget)
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            var xError = Utility.GetRondomNormalDistribution(0f, currentSpread);
            var yError = Utility.GetRondomNormalDistribution(0f, currentSpread);

            var fireDirection = aimTarget - fireTransform.position;

            fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
            fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

            currentSpread += 1f / stability;

            lastFireTime = Time.time;

            Shot(fireTransform.position, fireDirection);
            
            return true;
        }

        return false;
    }

    private void Shot(Vector3 StartPoint, Vector3 direction)
    {
        RaycastHit hit;

        var hitPosition = Vector3.zero;
        
        if(Physics.Raycast(StartPoint, direction, out hit, fireDistance, ~excudeTarget))
        {
            var target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damager = gunHolder.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;

                target.ApplyDamage(damageMessage);
            }
            else
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);
            }

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = StartPoint + direction * fireDistance;
        }

        StartCoroutine(ShotEffet(hitPosition));

        magAmmo--;
        if(magAmmo <= 0)
        state = State.Empty;
    }

    private IEnumerator ShotEffet(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        GunAudioPlayer.PlayOneShot(shotClip);

        bulletLineRenderer.enabled = true;
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain < 0 || magAmmo >= magCapacity)
            return false;

        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        GunAudioPlayer.PlayOneShot(reloadClip); 

        yield return new WaitForSeconds(reloadTime);

        var ammoToFill = magCapacity - magAmmo;

        if(ammoRemain < ammoToFill) ammoToFill = ammoRemain;

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }

    private void Update()
    {
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);
        currentSpread = Mathf.Clamp(currentSpread, 0f, maxSpread);
    }
}
