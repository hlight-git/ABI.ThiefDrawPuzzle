using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ABSPressState : UnitState<PressHandler>
{
    protected bool isPressing;
    public ABSPressState(PressHandler unit) : base(unit)
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
            if (Input.GetMouseButtonDown(0))
            {
                OnPress();
            }
        }
    }
    protected abstract void OnPress();
    protected abstract void OnHold();
    protected abstract void OnRelease();
}

public class PressToDrawState : ABSPressState
{
    public PressToDrawState(PressHandler unit) : base(unit)
    {
    }

    bool IsNear(Vector3 hitPos, Vector3 position) => Vector3.Distance(hitPos, position) <= Constant.NEAR_DISTANCE;
    protected override void OnPress()
    {
        Vector3? hitPos = RaycastHitPos();
        if (hitPos != null && IsNear(hitPos.Value, InsManager.Ins.Player.TF.position))
        {
            isPressing = true;
            unit.AddPoint(InsManager.Ins.Player.TF.position);
        }
    }

    protected override void OnHold()
    {
        Vector3? hitPos = RaycastHitPos();
        if (hitPos != null && Vector3.Distance(unit.LastLinePoint, hitPos.Value) > unit.Min2PointDistance)
        {
            unit.AddPoint(hitPos.Value);
        }
    }


    protected override void OnRelease()
    {
        isPressing = false;
        if (IsNear(unit.LastLinePoint, InsManager.Ins.Level.Destination))
        {
            InsManager.Ins.Player.SetPath(unit.LinePoints);
            GameManager.Ins.ChangeState(GameState.Moving);
            unit.ChangeState(unit.PressToMoveState);
        }
        else
        {
            unit.ClearPoints();
        }
    }
    Vector3? RaycastHitPos()
    {
        RaycastHit hit = CastRay();
        if (hit.collider != null)
        {
            return hit.point + Vector3.up * 0.1f;
        }
        return null;
    }

    RaycastHit CastRay()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenMousePosFar = new Vector3(mousePos.x, mousePos.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out RaycastHit hit, float.PositiveInfinity, Util.LayerOf(Constant.Layer.FLOOR));
        return hit;
    }
}

public class PressToMoveState : ABSPressState
{
    public PressToMoveState(PressHandler unit) : base(unit)
    {
    }

    protected override void OnPress()
    {
        isPressing = true;
        InsManager.Ins.Player.Move();
    }

    protected override void OnHold()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnRelease()
    {
        InsManager.Ins.Player.Stop();
        isPressing = false;
    }
}

public class PressHandler : ABSStateMachine<PressHandler>
{
    [SerializeReference] LineRenderer lineRenderer;
    [SerializeField] float min2PointDistance = 0.05f;

    readonly List<Vector3> points = new List<Vector3>();

    public float Min2PointDistance => min2PointDistance;
    public int PointCount => points.Count;
    public Vector3 LastLinePoint => points.Last();
    public Vector3[] LinePoints => points.ToArray(); 
    public UnitState<PressHandler> IdleState { get; private set; }
    public ABSPressState PressToDrawState { get; private set; }
    public ABSPressState PressToMoveState { get; private set; }

    protected override bool IsActivating() => GameManager.Ins.IsPlaying;

    protected override void InitStates()
    {
        PressToDrawState = new PressToDrawState(this);
        PressToMoveState = new PressToMoveState(this);
    }
    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(null);
    }
    public void AddPoint(Vector3 point)
    {
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }
    public void ClearPoints()
    {
        points.Clear();

        lineRenderer.positionCount = 0;
    }
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        OnPress();
    //    }

    //    if (isDrawing)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {
    //            OnHold();
    //        }

    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            OnRelease();
    //        }
    //    }
    //}

    //void OnPress()
    //{
    //    Vector3? hitPos = RaycastHitPos();
    //    if (hitPos != null && IsNear(hitPos.Value, InsManager.Ins.Player.TF.position))
    //    {
    //        isDrawing = true;
    //        AddPoint(InsManager.Ins.Player.TF.position);
    //    }
    //}
    //void OnHold()
    //{
    //    Vector3? hitPos = RaycastHitPos();
    //    if (hitPos != null && Vector3.Distance(points.Last(), hitPos.Value) > Min2PointDistance)
    //    {
    //        AddPoint(hitPos.Value);
    //    }
    //}

    //void OnRelease()
    //{
    //    isDrawing = false;
    //    if (IsNear(points.Last(), InsManager.Ins.Destination.position))
    //    {
    //        InsManager.Ins.Player.Move(points.ToArray());
    //        enabled = false;
    //    }
    //    else
    //    {
    //        ClearPoints();
    //    }
    //}
    //RaycastHit CastRay()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    Vector3 screenMousePosFar = new Vector3(mousePos.x, mousePos.y, Camera.main.farClipPlane);
    //    Vector3 screenMousePosNear = new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane);
    //    Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
    //    Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

    //    Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out RaycastHit hit, float.PositiveInfinity, Util.LayerOf(Constant.Layer.FLOOR));
    //    return hit;
    //}

    //Vector3? RaycastHitPos()
    //{
    //    RaycastHit hit = CastRay();
    //    if (hit.collider != null)
    //    {
    //        return hit.point + Vector3.up * 0.1f;
    //    }
    //    return null;
    //}

}
