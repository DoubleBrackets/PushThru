using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialOffset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateOffsets();
    }
    private void UpdateOffsets()
    {
        Vector3 cameraPos = CameraSystem.Main.transform.position;
        MeshRenderer[] renderers = GameObject.FindObjectsOfType<MeshRenderer>();
        int l = renderers.Length;
        for(int x = 0;x <l;x++)
        {
            MeshRenderer renderer = renderers[x];
            renderer.material.SetVector("_StartOffset", renderer.transform.position);
        }
        SkinnedMeshRenderer[] skinnedrenderers = GameObject.FindObjectsOfType<SkinnedMeshRenderer>();
        l = skinnedrenderers.Length;
        for (int x = 0; x < l; x++)
        {
            SkinnedMeshRenderer renderer = skinnedrenderers[x];
            renderer.material.SetVector("_StartOffset", renderer.transform.position);
        }
    }


}
