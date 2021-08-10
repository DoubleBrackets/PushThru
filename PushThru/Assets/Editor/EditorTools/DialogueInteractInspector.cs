using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueObject))]
public class DialogueInteractInspector : Editor
{


    private void OnSceneGUI()
    {
        DialogueObject dialogue = target as DialogueObject;

        Handles.DrawWireArc(dialogue.transform.position, Vector3.up, Vector3.right, 360f, dialogue.interactDistance);
    }

}
