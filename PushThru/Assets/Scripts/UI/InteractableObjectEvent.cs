using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractableObjectEvent : InteractableObject
{
    public float interactDistance;
    public bool removeOnInteract;
    public bool disableOnInteract;

    public UnityEvent interactEvent;
    protected override void OnInteract()
    {
        Vector3 dist = PlayerEntity.player.transform.position - transform.position;
        dist.y = 0;
        if (dist.magnitude <= interactDistance)
        {
            interactEvent?.Invoke();
            if (removeOnInteract)
                Destroy(gameObject);
            if (disableOnInteract)
                DisableInteract();
        }
    }
}
