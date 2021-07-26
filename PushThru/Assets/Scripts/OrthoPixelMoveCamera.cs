using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPixelMoveCamera : MonoBehaviour
{
    public static OrthoPixelMoveCamera orthoCam;

    public float xspeed;
    public float zspeed;
    public float lockStep;
    public Camera cam;
    public RenderTexture pixelTex;
    public GameObject lockStepTarget;

    private bool locked = false;
    private Vector3 offset;

    public Transform target;

    private void Awake()
    {
        orthoCam = this;
        offset = lockStepTarget.transform.localPosition;
        offset.y = 0;
    }

    void Update()
    {
        float edgeWidth = 100;
        Vector2 mousePos = Input.mousePosition;
        Vector2 size = new Vector2(Screen.width,Screen.height);
        float z = 0; //Input.GetAxisRaw("Vertical");
        float x = 0;// Input.GetAxisRaw("Horizontal");
        if (mousePos.x < edgeWidth)
        {
            x = -1;
        }
        else if (mousePos.x > size.x - edgeWidth)
            x = 1;
        if (mousePos.y < edgeWidth)
        {
            z = -1;
        }
        else if (mousePos.y > size.y - edgeWidth)
            z = 1;

        if (Input.GetKeyDown(KeyCode.R))
        {
            locked = !locked;
        }
        float modifier = 1;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            modifier = 0.2f;
        }
        Vector3 movement = new Vector3(x, 0, z).normalized;
        movement.x *= xspeed * modifier;
        movement.z *= zspeed * modifier;
        Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y,0);
        if (!locked)
            transform.position += rot * movement  * Time.deltaTime;

    }
    private void LerpToTarget(Transform toLerp, float time)
    {
        Vector3 targetPos = target.transform.position;
        Vector3 cPos = transform.position;
        toLerp.position = new Vector3(Mathf.SmoothStep(cPos.x, targetPos.x, time), transform.position.y, Mathf.SmoothStep(cPos.z, targetPos.z, time));
    }


    private void LateUpdate()
    {
        if (locked)
        {
            float time = 25 * Time.deltaTime;
            LerpToTarget(transform, 1);
        }
    }



}
