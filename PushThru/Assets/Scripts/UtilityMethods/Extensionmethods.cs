﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensionmethods
{
    public static Vector3 MoveUp(this Vector3 start)
    {
        start = start + Vector3.up * Time.deltaTime;
        return start;
    }

    public static Vector3 angle2Vector(this float theta)
    {
        return new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), 0, Mathf.Sin(theta * Mathf.Deg2Rad));
    }

    public static Vector3 Vector2To3TopDown(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static float YRotationDegrees(this Vector3 vec)
    {
        return Mathf.Rad2Deg*Mathf.Atan2(vec.z, vec.x);
    }

    public static float Angle(this Vector2 vec)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vec.y, vec.x);
    }

    static float lockStep = 0.06f;
    public static Vector3 RoundToPixel(this Vector3 vec)
    {
        return new Vector3(Mathf.FloorToInt(vec.x / lockStep) * lockStep, vec.y, Mathf.FloorToInt(vec.z / lockStep * 0.5f) * lockStep * 2);
    }

    public static float RoundToIntMultiple(this float val,int roundTo)
    {
        return Mathf.RoundToInt(val / roundTo) * roundTo;
    }
}
