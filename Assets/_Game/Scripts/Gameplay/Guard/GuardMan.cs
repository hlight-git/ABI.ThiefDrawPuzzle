using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuardMan : ABSStateMachine<GuardMan>, IGuard
{
    [SerializeReference] Animator animator;
    [SerializeReference] Transform[] pathTF;
    [SerializeReference] GuardSight sight;
    [SerializeField] float patrolMoveSpeed;
    [SerializeField] float chaseMoveSpeed;

    Vector3[] path;
    AnimTrigger animTrigger;
    Tween patrolTween;
    bool IsBackToFirstPoint;
    public GuardIdleState IdleState { get; private set; }
    public GuardPatrolState PatrolState { get; private set; }
    public GuardChasingState ChasingState { get; private set; }

    protected override void OnInit()
    {
        base.OnInit();
        animTrigger = new AnimTrigger(animator, Constant.Anim.Guard.IDLE);
        path = pathTF.Select(tf => tf.position).ToArray();
        ChangeState(IdleState);
        patrolTween = TF.DOPath(path, patrolMoveSpeed, PathType.CatmullRom)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLookAt(0)
            .OnStepComplete(OnStepCompleted)
            .OnUpdate(() => TF.forward = IsBackToFirstPoint ? TF.forward : -TF.forward)
            .Pause();
    }
    protected override void InitStates()
    {
        IdleState = new GuardIdleState(this);
        PatrolState = new GuardPatrolState(this);
        ChasingState = new GuardChasingState(this);
    }

    protected override bool IsActivating() => GameManager.Ins.IsPlaying;
    public void ChangeAnim(string animName) => animTrigger.TriggerAnim(animName);

    public void OnSawThief()
    {
        if (CurrentState != ChasingState)
        {
            Player thief = InsManager.Ins.Player;
            ChangeState(ChasingState);
            sight.gameObject.SetActive(false);
            TF.DOMove(thief.TF.position - TF.forward, chaseMoveSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => PunchThief(thief));
            thief.OnCaught(this);
            LevelManager.Ins.OnLose();
        }
    }
    void PunchThief(Player thief)
    {
        ChangeAnim(Constant.Anim.Guard.PUNCH);
        thief.OnBePunched();
    }
    public void Stop()
    {
        patrolTween.Pause();
    }
    public void Move()
    {
        IsBackToFirstPoint = !IsBackToFirstPoint;
        patrolTween.Play();

    }
    void OnStepCompleted()
    {
        ChangeState(IdleState);
    }
}
public class GuardIdleState : UnitState<GuardMan>
{
    readonly ScheduledInvoker invoker = new ScheduledInvoker();
    public GuardIdleState(GuardMan unit) : base(unit)
    {
    }

    public override void OnEnter()
    {
        unit.Stop();
        unit.ChangeAnim(Constant.Anim.Guard.IDLE);
        invoker.Schedule(Patrol, Random.Range(1f, 2f));
    }
    public override void OnExecute()
    {
        invoker.Countdown();
    }
    public override void OnExit()
    {
        invoker.Clear();
    }
    void Patrol()
    {
        unit.ChangeState(unit.PatrolState);
    }
}
public class GuardPatrolState : UnitState<GuardMan>
{
    public GuardPatrolState(GuardMan unit) : base(unit)
    {
    }

    public override void OnEnter()
    {
        unit.Move();
        unit.ChangeAnim(Constant.Anim.Guard.PATROL);
    }
    public override void OnExit()
    {
        unit.Stop();
    }
}

public class GuardChasingState : UnitState<GuardMan>
{
    public GuardChasingState(GuardMan unit) : base(unit)
    {
    }

    public override void OnEnter()
    {
        unit.Stop();
        unit.ChangeAnim(Constant.Anim.Guard.CHASE);
    }
}
