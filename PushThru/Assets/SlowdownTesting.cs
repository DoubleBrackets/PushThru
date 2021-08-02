using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownTesting : MonoBehaviour
{
    private bool isInSlowdown = false;
    public float slowdownFactor = 0.1f;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(isInSlowdown)
            {
                Time.timeScale = slowdownFactor;
            }
            else
            {
                Time.timeScale = 1;
            }
            isInSlowdown = !isInSlowdown;
        }
    }
}
