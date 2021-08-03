using System.Collections;
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
}
