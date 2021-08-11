using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PencilGenerator))]
public class PencilGeneratorInspector : Editor
{


    private void OnSceneGUI()
    {
        PencilGenerator pencilGen = target as PencilGenerator;

        Handles.DrawWireArc(pencilGen.transform.position, Vector3.up, Vector3.right, 360f, pencilGen.minRad);
        Handles.DrawWireArc(pencilGen.transform.position, Vector3.up, Vector3.right, 360f, pencilGen.maxRad);
    }

}
