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
    public AudioClip shotColip;
    public AudioClip reloadClip;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    
    public Transform fireTransform;
    public Transform leftHandMount;

    public float damage = 25f;
    public float fireDistance = 100f;

    public int ammoRemain = 100f;
    public int magAmmo;
    public int magCapacity = 30;

    public float timeEetFire = 0.12f;
    public float reloadTime = 1.8f;

    [Range[0f, 10f)] public float maxSpread = 3f;
    [Range(1f, 10f)] public float stability = 1f;
    [Range[0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;
    private float currentSpread;
    private float currentSpreadVelocity;

    private float lastFireTime;

    private LayerMask excudeTarget;

    private void Awake()
    {

    }

    public void SetUp(PlayerShooter gunHoler)
    {

    }

    private void OnEnable()
    {

    }

    private void onDisable()
    {

    }

    public bool Fire(Vector3 aimTarget)
    {
        return false;
    }

    private void Shot(Vector3 StartPoint, Vector3 direction)
    {

    }

    private IEnumerator ShotEffet(Vector3 hitPosittion)
    {
        yeild return null;
    }

    public bool Reload()
    {
        return false;
    }

    private IEnumerator ReloadRoutine()
    {
        yeild return false;
    }

    private void Update()
    {
        
    }
}
