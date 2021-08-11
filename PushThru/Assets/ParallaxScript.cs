using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    public Vector3 ratios;

    private void Start()
    {
        OrthoPixelMoveCamera.orthoCam.OrthoCamMove += MoveByRatio;
    }

    private void MoveByRatio(Vector3 pos)
    {
        pos.x *= ratios.x;
        pos.y *= ratios.y;
        pos.z *= ratios.z;
        transform.position += pos;
    }
}
