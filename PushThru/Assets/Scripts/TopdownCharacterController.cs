using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownCharacterController : MonoBehaviour
{
    public float velocity;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 inputVector = new Vector3(x, 0,y).normalized;
        inputVector.z *= 2;
        Vector3 rot = Camera.main.transform.rotation.eulerAngles;
        Quaternion camRot = Quaternion.Euler(0, rot.y, 0);
        rb.velocity = camRot * inputVector * velocity;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.z, rb.velocity.x);
        if(inputVector != Vector3.zero)
            transform.rotation = Quaternion.Euler(0,90-angle, 0);
    }
}
