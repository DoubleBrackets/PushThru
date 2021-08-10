using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatActionManager : MonoBehaviour
{
    [HideInInspector] public Vector2 currentActionDirection;

    public abstract bool IsPerformingAction();
}
