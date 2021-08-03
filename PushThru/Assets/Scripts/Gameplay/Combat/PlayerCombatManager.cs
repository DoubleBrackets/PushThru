using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerCombatManager : EntityCombatManager
{
    //Dependencies
    private PlayerEntity playerEntity;


    public bool isBlocking = false;

    private void Awake()
    {
        playerEntity = GetComponent<PlayerEntity>();
    }
    public override void ReceiveAttack(Attack attack)
    {
        if(!isBlocking)
        {
            playerEntity.TakeDamage(attack);
        }
    }
}
