using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICatInteractable
{
    void OnInteracted(Cat cat);
}
public partial class Cat
{
    public bool CanBreakWall { get; set; }
}
public partial class Cat : PoolUnit
{
    [Header("- References:")]
    [SerializeReference] Animator animator;
    [SerializeReference] Transform renderTF;
    [SerializeReference] Transform disguiseTF;

    [SerializeReference] Raycaster3D raycaster;
    [Header("- Fields:")]
    [SerializeField] float renderMaxSize = .6f;
    [SerializeField] float disguiseMaxSize = 2f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotateSpeed = 20f;
    [SerializeField] float disguiseDuration = 0.5f;
    Vector3 movingDirection;
    AnimTrigger animTrigger;
    int curTargetPoint;
    public bool IsMoving { get; private set; }
    public bool IsInDisguise { get; private set; }
    public DrawLine DrawLine { get; set; }
    public List<Vector3> Path => DrawLine.Points;
    void Awake() => OnInit();
    void Update()
    {
        if (!IsMoving)
        {
            return;
        }
        RaycastHit? hit = raycaster.Raycast(movingDirection, .3f, Util.LayerOf(Const.Layer.CATCH_RAY_OF_CAT));
        if (hit.HasValue)
        {
            if (hit.Value.collider.TryGetComponent(out ICatInteractable obj))
            {
                obj.OnInteracted(this);
            }
        }

        TF.forward = Vector3.Lerp(TF.forward, movingDirection, Time.deltaTime * rotateSpeed);
        TF.Translate(Time.deltaTime * moveSpeed * movingDirection, Space.World);

        if (IsReachedTargetPoint())
        {
            OnReachedTargetPoint();
        }
    }
    bool IsReachedTargetPoint()
    {
        if (IsMoving)
        {
            Vector3 tmp = Path[curTargetPoint] - TF.position;
            tmp.y = 0;
            return tmp.sqrMagnitude < 0.01f;
        }
        return false;
    }
    void OnReachedTargetPoint()
    {
        if (curTargetPoint == Path.Count - 1)
        {
            OnReachedDestination();
            return;
        }
        curTargetPoint++;
        movingDirection = Path[curTargetPoint] - TF.position;
        movingDirection.y = 0;
        movingDirection = movingDirection.normalized;
    }
    void OnInit()
    {
        animTrigger = new AnimTrigger(animator, Const.Anim.Cat.IDLE);
    }
    public void OnSpawn(Vector3 position, Quaternion rotation)
    {
        TF.SetPositionAndRotation(position, rotation);
        renderTF.localScale = Vector3.one;
    }
    public void OnDespawn()
    {
        renderTF.DOKill();
        disguiseTF.DOKill();
    }
    public void Disguise(bool value)
    {
        IsInDisguise = value;
        renderTF.DOScale(value ? 0 : renderMaxSize, disguiseDuration);
        disguiseTF.DOScale(value ? disguiseMaxSize : 0, disguiseDuration);
    }
    public void Stop()
    {
        IsMoving = false;
        Disguise(true);
        ChangeAnim(Const.Anim.Cat.IDLE);
    }
    void OnReachedDestination()
    {
        IsMoving = false;
        Disguise(false);
        TF.forward = Vector3.back;
        ChangeAnim(Const.Anim.Cat.VICTORY);
        LevelManager.Ins.Win();
    }
    public void Move()
    {
        IsMoving = true;
        ChangeAnim(Const.Anim.Cat.RUN);
        Disguise(false);
    }
    public void OnBeAttacked(Vector3 sourcePos)
    {
        IsMoving = false;
        ChangeAnim(Const.Anim.Cat.IDLE);
        renderTF.LookAt(sourcePos);
        Invoke(nameof(OnDeath), 1);
        //yield return Wait.ForSec(1f);
        //OnDeath();
    }
    public void OnCaught(Dog guard)
    {
        IsMoving = false;
        ChangeAnim(Const.Anim.Cat.IDLE);
        Vector3 lookPos = guard.TF.position;
        lookPos.y = TF.position.y;
        TF.LookAt(lookPos);
        //ChangeAnim(Constant.Anim.Cat.FIGHT);
    }
    public void OnCaught()
    {
        IsMoving = false;
        //ChangeAnim(Constant.Anim.Cat.SURRENDER);
    }
    public void ChangeAnim(string animName) => animTrigger.TriggerAnim(animName);
    public void OnDeath()
    {
        Stop();
        ChangeAnim(Const.Anim.Cat.DIE);
        LevelManager.Ins.Lose();
        //disguiseTF.DOJump(disguiseTF.position - 5 * TF.forward, 3, 3, 1f);
        //disguiseTF.gameObject.SetActive(false);
        //yield return Wait.ForSec(1f);

    }
}
