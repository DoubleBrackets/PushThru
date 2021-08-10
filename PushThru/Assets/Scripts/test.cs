using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    bool alter = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            alter = false;
            transform.position += new Vector3(0.01f, 0, 0.02f);
        }
        else if (!alter)
            alter = true;
    }
}
