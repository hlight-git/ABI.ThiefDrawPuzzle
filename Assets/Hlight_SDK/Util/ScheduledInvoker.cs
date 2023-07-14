using System;
using UnityEngine;

public class ScheduledInvoker
{
    public Action action;
    float delayTime;
    public bool IsCounting => action != null;
    public void Schedule(Action action, float delayTime)
    {
        this.action = action;
        this.delayTime = delayTime;
    }

    public void Clear()
    {
        action = null;
    }

    public bool Countdown()
    {
        if (!IsCounting)
        {
            return false;
        }
        if (delayTime <= 0)
        {
            action?.Invoke();
            Clear();
            return true;
        }
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }
        return false;
    }
}
