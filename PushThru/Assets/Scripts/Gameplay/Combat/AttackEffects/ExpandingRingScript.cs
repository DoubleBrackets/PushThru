using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingRingScript : MonoBehaviour
{
    public Vector3 speed;
    public float lifeTime;
    public int fixedUpdateInterval;
    private int fixedUpdateCounter = 0;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if(fixedUpdateCounter % fixedUpdateInterval == 0)
        {
            float change = Time.fixedDeltaTime * fixedUpdateCounter;
            Vector3 scale = transform.localScale;
            scale.x += speed.x * change;
            scale.z += speed.z * change;
            scale.y += speed.y * change;
            transform.localScale = scale;
        }

        fixedUpdateCounter++;
    }
}
