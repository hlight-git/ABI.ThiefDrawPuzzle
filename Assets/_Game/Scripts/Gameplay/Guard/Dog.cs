using DG.Tweening;
using HlightSDK.Tool.Level.Compress;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum DogPatrolType
{
    None,
    Yoyo,
    Incremental,
    ForeverYoyo,
    ForeverIncremental,
}

[System.Serializable]
public class DogData : HlightSDK.Tool.Level.Compress.LvGOData
{
    public DogPatrolType patrolType;
    public float patrolMoveSpeed = 2f;
    public float chaseMoveSpeed = 5f;
    public float idleTime = 2f;
    public float visionRange = 3f;
    public float visionAngle = 45f;
    [ReadOnly] public Vector3[] path;
}
public class Dog : ABSStateMachine<Dog>, IGuard, HlightSDK.Tool.Level.Compress.IComplexLvGO
{
    [SerializeReference] Animator animator;
    [SerializeReference] NavMeshAgent agent;
    [SerializeReference] GuardSight sight;
    [SerializeReference] Transform[] pathTF;

    [SerializeField] DogData data;

    AnimTrigger animTrigger;
    public DogIdleState IdleState { get; private set; }
    public DogPatrolState PatrolState { get; private set; }
    public DogChasingState ChasingState { get; private set; }
    public float IdleTime => data.idleTime;
    public DogPatrolType PatrolType => data.patrolType;

    public float VisionRange => data.visionRange;

    public float VisionAngle => data.visionAngle;

    public string CompressData => data.OnCompressed(TF);

    protected override void OnInit()
    {
        base.OnInit();
        sight.OnInit(this);
        agent.speed = data.patrolMoveSpeed;
        animTrigger = new AnimTrigger(animator);
        ChangeState(IdleState);

    }
    protected override void InitStates()
    {
        IdleState = new DogIdleState(this);
        ChasingState = new DogChasingState(this);
        if (PatrolType != DogPatrolType.None)
        {
            PatrolState = new DogPatrolState(this, data.path);
        }
    }

    protected override bool IsActivating() => true;

    public void OnSawCat(Cat cat)
    {
        if (cat.IsInDisguise)
        {
            return;
        }
        sight.gameObject.SetActive(false);
        Chase(cat);
    }
    public void ChangeAnim(string animName) => animTrigger.TriggerAnim(animName);
    void Chase(Cat cat)
    {
        agent.speed = data.chaseMoveSpeed;
        ChasingState.Target = cat;
        ChangeState(ChasingState);
    }
    public void Attack(Cat cat)
    {
        TF.LookAt(cat.TF.position);
        ChangeAnim(Const.Anim.Dog.ATTACK);
        Invoke(nameof(Cheering), 2f);
        cat.OnBeAttacked(TF.position);
    }
    public void Cheering()
    {
        ChangeAnim(Const.Anim.Dog.VICTORY);
    }

    public void OnDespawn()
    {
        TF.DOKill();
    }
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
    public bool IsNearDestination(float acceptableDistance = 0.01f) => (TF.position - agent.destination).sqrMagnitude < acceptableDistance;

    public LvGOData OnExtract(string json)
    {
        data = JsonUtility.FromJson<DogData>(json);
        if (PatrolType != DogPatrolType.None)
        {
            PatrolState = new DogPatrolState(this, data.path);
        }
        return data;
    }
#if UNITY_EDITOR

    [Button]
    public void UpdatePath()
    {
        data.path = pathTF.Select(tf => tf.position).ToArray();
    }
#endif
}
public class DogIdleState : UnitState<Dog>
{
    readonly ScheduledInvoker invoker = new ScheduledInvoker();
    public DogIdleState(Dog unit) : base(unit)
    {
    }

    public override void OnEnter()
    {
        unit.ChangeAnim(Const.Anim.Dog.IDLE);
    }
    public override void OnExecute()
    {
        if (invoker.IsCounting)
        {
            invoker.Countdown();
        }
        else if (unit.PatrolType != DogPatrolType.None)
        {
            invoker.Schedule(Patrol, unit.IdleTime);
        }
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
public class DogPatrolState : UnitState<Dog>
{
    readonly Vector3[] path;
    int iCurPoint = 0;
    int increaseVal = 1;
    bool firstTrigger = true;
    public DogPatrolState(Dog unit, Vector3[] patrolPath) : base(unit)
    {
        path = patrolPath;
    }

    public override void OnEnter()
    {
        unit.ChangeAnim(Const.Anim.Dog.WALK);
        unit.SetDestination(path[iCurPoint]);
    }
    public override void OnExecute()
    {
        if (unit.IsNearDestination())
        {
            OnReachedDestination();
        }
    }
    void OnReachedDestination()
    {
        bool isTheLastPoint = false;
        if (unit.PatrolType == DogPatrolType.Yoyo || unit.PatrolType == DogPatrolType.ForeverYoyo)
        {
            if (!firstTrigger && (iCurPoint == 0 || iCurPoint == path.Length - 1))
            {
                isTheLastPoint = true;
                increaseVal *= -1;
            }
        }
        else
        {
            if (iCurPoint == path.Length - 1)
            {
                isTheLastPoint = true;
                iCurPoint = -1;
            }
        }
        firstTrigger = false;
        iCurPoint += increaseVal;

        if (isTheLastPoint && (unit.PatrolType == DogPatrolType.Yoyo || unit.PatrolType == DogPatrolType.Incremental))
        {
            unit.ChangeState(unit.IdleState);
            return;
        }
        unit.SetDestination(path[iCurPoint]);
    }
}

public class DogChasingState : UnitState<Dog>
{
    public Cat Target { get; set; }
    public DogChasingState(Dog unit) : base(unit)
    {

    }

    public override void OnEnter()
    {
        unit.ChangeAnim(Const.Anim.Dog.RUN);
    }
    public override void OnExecute()
    {
        if (Target != null)
        {
            if (unit.IsNearDestination(Const.DOG_CATCH_CAT_RANGE))
            {
                unit.SetDestination(unit.TF.position);
                unit.Attack(Target);
                Target = null;
            }
            else
            {
                unit.SetDestination(Target.TF.position);
            }
        }
    }
}
