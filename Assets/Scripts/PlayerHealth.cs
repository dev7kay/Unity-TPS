using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{

    private Animator animator;
    private AudioSource playerAudioPlyaer;

    public AudioClip DeathClip;
    public AudioClip hitClip;

    void Awake()
    {
        playerAudioPlyaer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateUI();
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        UIManager.Instance.UpdateHealthText(dead ? 0f : health);
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if(!base.ApplyDamage(damageMessage)) return false;

        EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, 
        transform, EffectManager.EffectType.Flesh);
        playerAudioPlyaer.PlayOneShot(hitClip);

        UpdateUI();

        return true;
    }

    public override void Die()
    {
        base.Die();

        UpdateUI();

        playerAudioPlyaer.PlayOneShot(DeathClip);

        animator.SetTrigger("Die");
    }
}
