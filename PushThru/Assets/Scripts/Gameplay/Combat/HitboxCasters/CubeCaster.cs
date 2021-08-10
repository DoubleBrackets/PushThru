using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCaster : AttackCaster
{
    public float width;
    public float length;
    public override HashSet<EntityCombatManager> CastForCombatManagers(Vector2 direction)
    {
        HashSet<EntityCombatManager> found = new HashSet<EntityCombatManager>();
        float targetAngle = direction.Angle() * Mathf.Deg2Rad;
        float widthMultiplier = 1+0.5f * Mathf.Abs(Mathf.Cos(targetAngle));
        float lengthMultiplier = 1+0.5f * Mathf.Abs(Mathf.Sin(targetAngle));
        Vector3 size = new Vector3(length * lengthMultiplier, 0.5f, width * widthMultiplier);
        Vector3 center = transform.position + direction.Vector2To3TopDown() * length/2f * lengthMultiplier;
        RaycastHit[] hits = Physics.BoxCastAll(center, size / 2f, direction.Vector2To3TopDown(), 
            Quaternion.Euler(0, targetAngle * Mathf.Rad2Deg, 0), 0.1f, targetLayerMask);
        foreach (RaycastHit hit in hits)
        {
            EntityCombatManager combatManager = hit.collider.GetComponent<EntityCombatManager>();
            if (combatManager)
            {
                    found.Add(combatManager);
            }
        }
        return found;
    }
}
