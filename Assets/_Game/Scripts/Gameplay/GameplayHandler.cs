using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayHandler : ABSStateMachine<GameplayHandler>
{
    public DrawingState DrawingState;
    public MoveControllingState MoveControllingState;
    private void OnEnable()
    {
        ChangeState(DrawingState);
    }
    protected override void InitStates()
    {
        DrawingState = new DrawingState(this);
        MoveControllingState = new MoveControllingState(this);
    }

    protected override bool IsActivating() => GameManager.Ins.IsPlaying;
}

public abstract class ABSGameplayState : UnitState<GameplayHandler>
{
    public bool isPressing;

    protected ABSGameplayState(GameplayHandler unit) : base(unit)
    {
    }

    public override void OnExecute()
    {
        if (isPressing)
        {
            if (Input.GetMouseButton(0))
            {
                OnHold();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnRelease();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                OnPress();
            }
        }
    }
    protected abstract void OnPress();
    protected abstract void OnHold();
    protected abstract void OnRelease();
}
public class DrawingState : ABSGameplayState
{
    bool lastPointIsValid;
    public DrawingState(GameplayHandler unit) : base(unit)
    {
    }
    public override void OnEnter()
    {
        isPressing = false;
    }
    protected override void OnPress()
    {
        RaycastHit? hit = CastRay();
        if (hit.HasValue && hit.Value.collider.CompareTag(Const.Tag.FLOOR) && IsNear(LevelManager.Ins.Cat.TF.position, hit.Value.point))
        {
            UIManager.Ins.GetUI<UIGamePlay>().ShowShopButtons(false);
            isPressing = true;
            LevelManager.Ins.DrawLine.UpdateLine(LevelManager.Ins.Cat.TF.position);
            LevelManager.Ins.DrawLine.UpdateLine(hit.Value.point);
        }
    }

    protected override void OnHold()
    {
        RaycastHit? hit = CastRay();
        if (!hit.HasValue)
        {
            return;
        }

        if (Physics.Linecast(hit.Value.point + Const.DRAWLINE_AND_FLOOR_OFFSET, LevelManager.Ins.DrawLine.LastPoint, Util.LayerOf(Const.Layer.CATCH_RAY)))
        {
            lastPointIsValid = false;
            return;
        }

        if (IsRaycastHitDestination(hit.Value))
        {
            OnFinishLine(hit.Value);
        }
        else if (hit.Value.collider.CompareTag(Const.Tag.FLOOR))
        {
            lastPointIsValid = true;
            LevelManager.Ins.DrawLine.UpdateLine(hit.Value.point);
        }
    }

    protected override void OnRelease()
    {
        isPressing = false;
        LevelManager.Ins.DrawLine.Remove();
    }
    void OnFinishLine(RaycastHit hit)
    {
        LevelManager.Ins.DrawLine.UpdateLine(hit.point);
        unit.ChangeState(unit.MoveControllingState);
    }
    bool IsRaycastHitDestination(RaycastHit hit) => hit.collider.CompareTag(Const.Tag.DESTINATION) && lastPointIsValid;
    bool IsNear(Vector3 hitPos, Vector3 position) => (hitPos - position).sqrMagnitude <= Const.ACCEPTABLE_DISTANCE_FROM_CAT_TO_HITPOINT;

    RaycastHit? CastRay() => MainCamera.Ins.Raycast(Util.LayerOf(Const.Layer.CATCH_RAY));
}

public class MoveControllingState : ABSGameplayState
{
    public MoveControllingState(GameplayHandler unit) : base(unit)
    {
    }

    protected override void OnPress()
    {
        isPressing = true;
        LevelManager.Ins.Cat.Move();
    }

    protected override void OnHold()
    {

    }

    protected override void OnRelease()
    {
        isPressing = false;
        LevelManager.Ins.Cat.Stop();
    }
}
