using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPixelSizeCalculateScript : MonoBehaviour
{
    //currently 0.016
    // Start is called before the first frame update
    int counter = 0;
    Vector3 pos;
    float pixelOffset;
    void Start()
    {
        pixelOffset = (float)System.Math.Sqrt(3) * 0.00625f;
        print(pixelOffset);
        pos = transform.position;
        print((pixelOffset * 4 / 3 * 4).ToString());
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            counter++;
            transform.position = pos - Vector3.up * (float)(counter * pixelOffset * 4/3 * 4);
        }
    }
}
