using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static PlayerEntity player;

    public GameObject[] deathToRemove;

    public string deathScene;

    private void Awake()
    {
        player = this;
    }

    public override void TakeDamage(Attack attack)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= attack.damage;
        InvokeHealthChanged(currentHealth);
        if(currentHealth <= 0)
        {
            PlayerDeath();
        }
        if(attack.damage > 0)
        {
            OverlayEffectsScript.instance.PlayEffect("HurtEffect");
            if(currentHealth > 0)
                ParticleManager.particleManager.PlayParticle("PlayerHurtParticles");
        }
        TimeUtils.instance.FreezeTime(0.01f, 0.15f);
    }

    private void PlayerDeath()
    {
        InputManager.instance.IncrementAllInputEnabled();
        ShadowBroEnemyAI ai = FindObjectOfType<ShadowBroEnemyAI>();
        if(ai)
            ai.enabled = false;
        ParticleManager.particleManager.PlayParticle("PlayerDeathParticles");
        foreach(GameObject g in deathToRemove)
        {
            g.SetActive(false);
        }
        SceneChanger.instance.ChangeScenes(deathScene,2.5f);
    }

    [ContextMenu("Testdamage")]
    public void Test()
    {
        TakeDamage(new Attack(1, Vector3.zero));
    }
}
