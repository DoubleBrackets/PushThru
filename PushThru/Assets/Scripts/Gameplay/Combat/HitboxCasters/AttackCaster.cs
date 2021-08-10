using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackCaster : MonoBehaviour
{
    public LayerMask targetLayerMask;
    public virtual HashSet<EntityCombatManager> CastForCombatManagers(Vector2 direction)
    {
        return null;
    }
  

}
