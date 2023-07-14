using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABSStateMachine<SelfT>: GameUnit where SelfT : ABSStateMachine<SelfT>
{
    [Header("State Machine:")]
    public string currentStateLog;
    public UnitState<SelfT> CurrentState { get; protected set; }
    private void Awake() => OnInit();
    protected virtual void Update()
    {
        if (IsActivating())
        {
            CurrentState.OnExecute();
        }
    }
    protected virtual void OnInit()
    {
        InitStates();
    }
    public virtual void ChangeState(UnitState<SelfT> state)
    {
        if (CurrentState != state)
        {
            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState?.OnEnter();
            currentStateLog = CurrentState.ToString();
        }
    }
    protected abstract bool IsActivating();
    protected abstract void InitStates();
}
