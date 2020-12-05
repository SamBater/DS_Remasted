using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFactory : MonoBehaviour
{
    private static AnimatorFactory instance = null;
    [Header("One Hand Motion")]
    public AnimatorOverrideController a02;
    public AnimatorOverrideController a03;

    [Header("Two Hand Motion")]
    [Tooltip("双手持剑")]
    public  AnimatorOverrideController sword2H;
    [Tooltip("双手扛剑")]
    public  AnimatorOverrideController great2H;
    [Tooltip("弓箭")]
    public  AnimatorOverrideController bow;
    [Tooltip("拳套/盾牌")]
    public  AnimatorOverrideController shield_Fists;

    private static Dictionary<int, AnimatorOverrideController> animators = new Dictionary<int, AnimatorOverrideController>();
    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        animators.Add(2,a02);
        animators.Add(3,a03);
        animators.Add(10, sword2H);
        animators.Add(12, great2H);
        animators.Add(14, bow);
        animators.Add(15, shield_Fists);
    }

    public static AnimatorFactory Instance()
    {
        return instance;
    }

    public static void SetLocalMotion(Animator animator,int id)
    {
        AnimatorOverrideController targetAnimator = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animators.TryGetValue(id,out targetAnimator);
        animator.runtimeAnimatorController = targetAnimator;
    }

}
