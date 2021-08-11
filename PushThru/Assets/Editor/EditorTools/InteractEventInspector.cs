using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractableObjectEvent))]
public class InteractEventInspector : Editor
{


    private void OnSceneGUI()
    {
        InteractableObjectEvent dialogue = target as InteractableObjectEvent;

        Handles.DrawWireArc(dialogue.transform.position, Vector3.up, Vector3.right, 360f, dialogue.interactDistance);
    }

}
