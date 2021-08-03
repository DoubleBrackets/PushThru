using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    public float hoverHeight = 5f;

    public float period = 5f;
    private float hoverTimer = 0f;
    private int dir = 1;
    private Vector3 startPos;

    public float rotateAngle;
    public float rotatePeriod;
    private float rotateTimer = 0f;

    // Update is called once per frame
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        hoverTimer += dir * Time.deltaTime;
        if(hoverTimer >= period)
        {
            hoverTimer = period;
            dir = -1;
        }
        else if(hoverTimer <= 0)
        {
            hoverTimer = 0;
            dir = 1;
        }
        float time = hoverTimer / period;
        float distance = Mathf.SmoothStep(0, hoverHeight, time);
        transform.position = startPos + Vector3.up * distance;

        rotateTimer -= Time.deltaTime;
        if(rotateTimer <= 0)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y += rotateAngle;
            transform.rotation = Quaternion.Euler(rot);
            rotateTimer = rotatePeriod;
        }
    }
}
