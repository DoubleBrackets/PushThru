using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMovementScript : MonoBehaviour
{
    /*Component ref store and accessors*/
    [HideInInspector] public Rigidbody rigidBody;
    public Vector3 velocity
    {
        get { return rigidBody.velocity; }
        set { rigidBody.velocity = value; }
    }
    public float zVelocity
    {
        get { return rigidBody.velocity.z; }
        set { rigidBody.velocity = new Vector3(xVelocity, rigidBody.velocity.y,value); }
    }
    public float xVelocity
    {
        get { return rigidBody.velocity.x; }
        set { rigidBody.velocity = new Vector3(value, rigidBody.velocity.y, zVelocity); }
    }
    /*Input fields and accessors*/
    private float _horizontalInput;
    public float horizontalInput { get => _horizontalInput; }
    private float _verticalInput;
    public float verticalInput { get => _verticalInput; }
    public Vector2 inputVector
    {
        get => new Vector2(horizontalInput, verticalInput);
        set { _horizontalInput = value.x; _verticalInput = value.y; }
    }

    /*Movement physics fields*/
    public float baseMaxMoveSpeed;
    private float currentMaxMoveSpeed;
    private float movementBonus = 0f;
    public float movementForce;
    public float zAxisMultiplier = 2;
    //Acceleration magnitude per fixedUpdate based on movementForce
    private float fixedAccelMagnitude;
    //Velocity multiplier per fixed update when slowdown is active
    public float slowdownFactor;
    /*Movement States and accessors*/
    private int _movementActive = 0;
    public bool movementActive { get => _movementActive == 0; }
    private int _slowdownActive = 0;
    public bool slowdownActive { get => _slowdownActive == 0; }


    public Vector2 facing;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentMaxMoveSpeed = baseMaxMoveSpeed;
        fixedAccelMagnitude = (movementForce / rigidBody.mass) * Time.fixedDeltaTime;
    }

    private void OnValidate()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentMaxMoveSpeed = baseMaxMoveSpeed;
        fixedAccelMagnitude = (movementForce / rigidBody.mass) * Time.fixedDeltaTime;
    }



    public void FixedUpdate()
    {
        //Horizontal movement
        Vector2 inputVector = this.inputVector;
        //applies z multiplier
        inputVector.y *= zAxisMultiplier;

        bool useXForce = false, useZForce = false;
        if (movementActive)
        {
            float xVelStep = fixedAccelMagnitude * inputVector.x;
            float zVelStep = fixedAccelMagnitude * inputVector.y;
            float zVelAfterForce = zVelocity + zVelStep;
            float xVelAfterForce = xVelocity + xVelStep;
            Vector2 unscaledVelAfterForce = new Vector2(xVelAfterForce, zVelAfterForce/zAxisMultiplier);
            Vector2 scaledVelocity = velocity;
            scaledVelocity.y /= zAxisMultiplier;
            bool velOverLimit = scaledVelocity.magnitude > currentMaxMoveSpeed + movementBonus;
            bool forceWithinLimit = unscaledVelAfterForce.magnitude <= currentMaxMoveSpeed + movementBonus && !velOverLimit;
            //force goes over limit or current speed is over but force is directed in opposite direction, or force does not exceed limit, apply force
            if (xVelStep != 0 && (Mathf.Sign(xVelocity) != Mathf.Sign(xVelStep) || forceWithinLimit))
            {
                xVelocity = xVelAfterForce;
                useXForce = true;
            }
            if (zVelStep != 0 && (Mathf.Sign(zVelocity) != Mathf.Sign(zVelStep) || forceWithinLimit))
            {
                zVelocity = zVelAfterForce;
                useZForce = true;
            }
            if (!velOverLimit)//current velocity is within limit but adding force will bring it over, set velocity to limit
            {
                if (verticalInput != 0 && !useZForce)
                {
                    zVelocity = (inputVector.y * (currentMaxMoveSpeed + movementBonus));
                    useZForce = true;
                }
                if (horizontalInput != 0 && !useXForce)
                {
                    xVelocity = (inputVector.x * (currentMaxMoveSpeed + movementBonus));
                    useXForce = true;
                }

            }
        }
        //Slowdown
        if (slowdownActive)
        {
            if (!useXForce || !movementActive)
                xVelocity *= slowdownFactor;
            if (!useZForce || !movementActive)
                zVelocity *= slowdownFactor;
        }
        facing = new Vector2(xVelocity, zVelocity).normalized;
    }

    public void IncrementControlAndSlowdown()
    {
        IncrementMovementActive();
        IncrementSlowdownActive();
    }

    public void DecrementControlAndSlowdown()
    {
        DecrementMovementActive();
        DecrementSlowdownActive();
    }

    public IEnumerator DisableControlAndSlowdown(float slowdownduration, float controlduration)
    {
        IncrementMovementActive();
        IncrementSlowdownActive();
        yield return new WaitForSeconds(slowdownduration);
        DecrementSlowdownActive();
        yield return new WaitForSeconds(controlduration - slowdownduration);
        DecrementMovementActive();
    }

    #region helper methods

    public void IncrementMovementActive()
    {
        _movementActive++;
    }

    public void DecrementMovementActive()
    {
        _movementActive--;
    }

    public void IncrementSlowdownActive()
    {
        _slowdownActive++;
    }

    public void DecrementSlowdownActive()
    {
        _slowdownActive--;
    }

    public void MultMoveSpeed(float val)
    {
        currentMaxMoveSpeed *= val;
    }
    #endregion
}
