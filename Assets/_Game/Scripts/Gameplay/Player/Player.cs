using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameUnit
{
    [SerializeReference] Animator animator;
    [SerializeReference] Transform renderTF;
    [SerializeReference] Transform disguiseTF;

    [SerializeField] float moveSpeed;
    [SerializeField] float disguiseSpeed;

    AnimTrigger animTrigger;
    Tween moveTween;
    public bool IsDisguising { get; private set; } 
    void Awake() => OnInit();
    void OnInit()
    {
        animTrigger = new AnimTrigger(animator, Constant.Anim.Player.IDLE);
    }
    public void Disguise(bool isTrue)
    {
        IsDisguising = isTrue;
        renderTF.DOScale(isTrue ? 0 : 2, disguiseSpeed);
        disguiseTF.DOScale(isTrue ? 2 : 0, disguiseSpeed);
    }
    public void SetPath(Vector3[] path)
    {
        moveTween = TF.DOPath(path, moveSpeed).SetSpeedBased(true).SetEase(Ease.Linear).SetLookAt(0.02f).OnUpdate(() => TF.rotation = Quaternion.Euler(0, TF.rotation.eulerAngles.y, 0))//, true)//.SetOptions(true, lockRotation: AxisConstraint.Z)
            .OnComplete(OnReachedDestination);
        moveTween.Pause();
    }
    public void Stop()
    {
        moveTween.Pause();
        Disguise(true);
        ChangeAnim(Constant.Anim.Player.IDLE);
    }
    void OnReachedDestination()
    {
        Disguise(false);
        ChangeAnim(Constant.Anim.Player.IDLE);
        LevelManager.Ins.OnWin();
    }
    public void Move()
    {
        ChangeAnim(Constant.Anim.Player.SNEAK);
        Disguise(false);
        moveTween.Play();
    }
    public void OnBePunched()
    {
        ChangeAnim(Constant.Anim.Player.KNOCKEDOUT);
    }
    public void OnCaught(GuardMan guard)
    {
        moveTween.Pause();
        Vector3 lookPos = guard.TF.position;
        lookPos.y = TF.position.y;
        TF.LookAt(lookPos);
        ChangeAnim(Constant.Anim.Player.FIGHT);
    }
    public void OnCaught()
    {
        moveTween.Pause();
        ChangeAnim(Constant.Anim.Player.SURRENDER);
    }
    public void ChangeAnim(string animName) => animTrigger.TriggerAnim(animName);
}
