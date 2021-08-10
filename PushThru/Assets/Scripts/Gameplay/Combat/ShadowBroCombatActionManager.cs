using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBroCombatActionManager : CombatActionManager
{
    public override bool IsPerformingAction()
    {
        return false;
    }
}
