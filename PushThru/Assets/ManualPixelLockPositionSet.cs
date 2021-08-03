using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPixelLockPositionSet : MonoBehaviour
{
    public SkinnedMeshRenderer[] meshes;

    private void LateUpdate()
    {
        foreach(SkinnedMeshRenderer renderer in meshes)
        {
            renderer.material.SetVector("_GlobalPosition", transform.position);
        }
    }

}
