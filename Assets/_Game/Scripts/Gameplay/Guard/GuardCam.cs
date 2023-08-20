using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCam : PoolUnit, IGuard
{
    [SerializeReference] GuardSight sight;
    [SerializeField] float visionRange = 4f;
    [SerializeField] float visionAngle = 45f;
    [SerializeField] float idleTime = 2f;
    [SerializeField] Vector3 startAngle;
    [SerializeField] Vector3 endAngle;
    [SerializeField] float rotateSpeed;
    Tween rotateTween;

    public float VisionRange => visionRange;

    public float VisionAngle => visionAngle;

    private void Awake()
    {
        OnInit();
    }
    private void OnDisable()
    {
        rotateTween.Kill();
    }
    void OnInit()
    {
        sight.OnInit(this);
        TF.rotation = Quaternion.Euler(startAngle);
        rotateTween = TF.DORotate(endAngle, rotateSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(Idle);
    }
    void Idle()
    {
        rotateTween.Pause();
        Invoke(nameof(ResumeRotate), idleTime);
    }
    void ResumeRotate()
    {
        rotateTween.Play();
    }
    public void OnSawCat(Cat player)
    {
        if (GameManager.Ins.IsPlaying)
        {
            rotateTween.Pause();
            player.OnCaught();
        }
    }
}
