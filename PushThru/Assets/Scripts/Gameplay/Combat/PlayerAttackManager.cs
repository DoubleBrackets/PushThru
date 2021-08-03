using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackManager : MonoBehaviour
{
    public InputManager inputManager;


    private void Awake()
    {
        inputManager.AttackKeyDown += PerformAttack;
    }

    private void PerformAttack()
    {

    }
}
