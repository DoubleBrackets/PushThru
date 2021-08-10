using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    void Start()
    {
        DialogueTextManager.instance.interactButtonPressed += OnInteract;
    }

    protected abstract void OnInteract();

}
