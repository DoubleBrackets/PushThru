using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Camera theoreticalViewCamera;
    public Camera projectToRTCamera;
    public Camera actualViewCamera;
    public GameObject displayQuad;

    public OrthoPixelMoveCamera pixelMoveCamera;

    public float rtSizeRatio;

    public static Camera Main;

    private void Awake()
    {
        Main = theoreticalViewCamera;
        UpdateQuadSize();
    }

    [ContextMenu("Update Quad Size")]

    private void UpdateQuadSize()
    {
        projectToRTCamera.orthographicSize = actualViewCamera.orthographicSize * rtSizeRatio;
        Vector2 res = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        float orthoSizeFull = actualViewCamera.orthographicSize * 2 * rtSizeRatio;
        displayQuad.transform.localScale = new Vector3(orthoSizeFull * res.x / res.y, orthoSizeFull, 1);
    }

    private void LateUpdate()
    {
        if (pixelMoveCamera.locked)
            actualViewCamera.transform.localPosition = Vector3.zero;
        else
            actualViewCamera.transform.localPosition = new Vector3(pixelMoveCamera.currentPixelOffset.x, pixelMoveCamera.currentPixelOffset.z / 2f, 0);
    }

}
