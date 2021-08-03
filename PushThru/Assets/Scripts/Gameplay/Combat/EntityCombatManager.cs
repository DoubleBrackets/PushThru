using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityCombatManager : MonoBehaviour
{
    public abstract void ReceiveAttack(Attack attack);
}
