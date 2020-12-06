using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorInput : IActorManagerInterface
{
    protected Vector3 movingVec;
    protected bool enableInput;
    public bool running;
    public ActorController ac;
    public Vector3 GetMoveVec()
    {
        return movingVec;
    }

    public void InputToggle(bool value)
    {
        enableInput = value;
    }

    public bool EnableInput()
    {
        return enableInput;
    }

    public float GetInputmag()
    {
        return movingVec.magnitude;
    }

}
