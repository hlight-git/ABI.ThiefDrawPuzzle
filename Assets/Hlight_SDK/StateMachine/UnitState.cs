using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState <T> where T : ABSStateMachine<T>
{
    protected T unit;
    public UnitState(T unit) => this.unit = unit;
    public virtual void OnEnter() { }
    public virtual void OnExecute() { }
    public virtual void OnExit() { }
}
