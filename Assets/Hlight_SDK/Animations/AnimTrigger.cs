using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ABSAnimTrigger<T> where T : class
{
    [SerializeField] Animator animator;
    public T CurrentAnim { get; private set; }
    protected abstract string CurrentAnimTrigger { get; }
    public ABSAnimTrigger() { }
    public ABSAnimTrigger(Animator animator, T defaultAnim = default)
    {
        this.animator = animator;
        CurrentAnim = defaultAnim;
        if (defaultAnim != default)
        {
            this.animator.SetTrigger(CurrentAnimTrigger);
        }
    }
    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
        CurrentAnim = default;
    }
    public void TriggerAnim(T animName)
    {
        if (!animName.Equals(CurrentAnim))
        {
            if (CurrentAnim != default)
            {
                animator.ResetTrigger(CurrentAnimTrigger);
            }
            CurrentAnim = animName;
            animator.SetTrigger(CurrentAnimTrigger);
        }
    }
    public void SetPlaySpeed(float speed)
    {
        animator.speed = speed;
    }
}

[Serializable]
public class AnimTrigger : ABSAnimTrigger<string>
{
    public AnimTrigger(Animator animator, string defaultAnim = null) : base(animator, defaultAnim)
    {
    }

    protected override string CurrentAnimTrigger => CurrentAnim;
}

//[Serializable]
//public class AnimTrigger<T> : ABSAnimTrigger<T> where T : Enum
//{
//    [SerializeField] AnyStateAnimSet animSet;

//    public AnimTrigger(Animator animator, T defaultAnim) : base(animator, defaultAnim)
//    {
//    }

//    protected override string CurrentAnimTrigger => animSet.Get(CurrentAnim).trigger;
//}