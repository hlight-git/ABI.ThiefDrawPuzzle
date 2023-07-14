using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCam : GameUnit, IGuard
{
    [SerializeField] Vector3 endRotate;
    [SerializeField] float rotateSpeed;
    Tween rotateTween;
    private void Awake()
    {
        OnInit();
    }
    void OnInit()
    {
        rotateTween = TF.DORotate(endRotate, rotateSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(Idle);
    }
    void Idle()
    {
        rotateTween.Pause();
        Invoke(nameof(ResumeRotate), 1f);
    }
    void ResumeRotate()
    {
        rotateTween.Play();
    }
    public void OnSawThief()
    {
        rotateTween.Pause();
        InsManager.Ins.Player.OnCaught();
        LevelManager.Ins.OnLose();
    }
}
