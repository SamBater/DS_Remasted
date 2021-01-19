using UnityEngine;

public abstract class ActorInput : IActorManagerInterface
{
    protected Vector3 MovingVec;
    protected bool EnableInput;
    public bool running;
    public ActorController ac;
    public Vector3 GetMoveVec()
    {
        return MovingVec;
    }
    
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
        return MovingVec.magnitude;
    }

}
