using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : InteractableObject
{
    public float interactDistance;
    [TextArea]
    public string[] text;
    protected override void OnInteract()
    {
        Vector3 dist = PlayerEntity.player.transform.position - transform.position;
        dist.y = 0;
        if(dist.magnitude <= interactDistance)
        {
            foreach(string str in text)
            {
                DialogueTextManager.instance.QueueMessage(str);
            }
        }
    }

}
