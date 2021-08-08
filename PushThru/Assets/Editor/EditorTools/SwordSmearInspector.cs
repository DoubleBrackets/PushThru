using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwordSmearEffect))]
public class SwordSmearInspector : Editor
{
    private SwordSmearEffect effect;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        effect = target as SwordSmearEffect;
        handleTransform = effect.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        int lineSteps = effect.steps;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.blue;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);

        Handles.color = Color.red;
        Vector3 lineStart = effect.GetPoint(0f);
        Handles.color = Color.green;
        Handles.DrawLine(lineStart, lineStart + effect.GetDirection(0f));
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = effect.GetPoint(i / (float)lineSteps);
            Handles.color = Color.red;
            Handles.DrawLine(lineStart, lineEnd);
            Handles.color = Color.green;
            Handles.DrawLine(lineEnd, lineEnd + effect.GetDirection(i / (float)lineSteps));
            lineStart = lineEnd;
        }
    }



    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(effect.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(effect, "Move Point");
            EditorUtility.SetDirty(effect);
            effect.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;

    }
}
