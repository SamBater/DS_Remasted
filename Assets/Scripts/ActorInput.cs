using UnityEngine;

public abstract class ActorInput : IActorManagerInterface
{
    private Vector3 movingVec;

    public Vector3 MovingVec
    {
        get => movingVec;
        protected set => movingVec = value;
    }

    protected bool EnableInput;
    public bool running;
    public ActorController ac;
    
    /// <summary>
    /// 允许/禁止控制器输入
    /// </summary>
    /// <param name="value">True:允许输入 Flase:禁止输入</param>
    public void InputToggle(bool value)
    {
        EnableInput = value;
    }

    public bool GetEnableInput()
    {
        return EnableInput;
    }
    
    
    /// <summary>
    /// 获取输入力度
    /// </summary>
    /// <returns>输入力度</returns>
    public float GetInputMag()
    {
        return Mathf.Clamp(MovingVec.magnitude,0,1);
    }

    public void MoveForward(float val)
    {
        movingVec.z = val;
    }

    public void MoveRight(float val)
    {
        movingVec.x = val;
    }

}
