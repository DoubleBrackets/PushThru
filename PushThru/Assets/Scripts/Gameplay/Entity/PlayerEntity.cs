using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static PlayerEntity player;
    private void Awake()
    {
        player = this;
    }
    public override void TakeDamage(Attack attack)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= attack.damage;
        if(currentHealth <= 0)
        {
            PlayerDeath();
        }
        if(attack.damage > 0)
        {
            OverlayEffectsScript.instance.PlayEffect("HurtEffect");
        }
        ParticleManager.particleManager.PlayParticle("PlayerHurtParticles");
        TimeUtils.instance.FreezeTime(0.01f, 0.15f);
    }

    private void PlayerDeath()
    {
        PlayerPrefs.SetInt("PencilCount", PlayerPrefs.GetInt("PencilCount", 1) + 1);
        SceneChanger.instance.ChangeScenes("Start");
    }
}
