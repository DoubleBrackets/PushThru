using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class DestroyableObjectCombatManager : EntityCombatManager
{
    private Entity attachedEntity;

    private void Awake()
    {
        attachedEntity = GetComponent<Entity>();        
    }


    public override void ReceiveAttack(Attack attack)
    {
        attachedEntity.TakeDamage(attack);
    }
}
