using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABSCollectItem : ABSEffector
{
    [SerializeReference] Transform renderTF;

    [SerializeField] Vector3 rotateWhenCollect = Vector3.up * 180; 
    [SerializeField] float flyDuration = .5f;
    [SerializeField] float jumpPower = 3f;
    [SerializeField] float value = 1f;


    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatMaxHeight = 1f;
    public float Value => value;
    Tween rotateTween;
    Tween floatTween;

    private void OnDisable()
    {
        rotateTween.Kill();
        floatTween.Kill();
        TF.DOKill();
    }
    private void Start()
    {
        PlayVFX();
    }
    private void PlayVFX()
    {
        Vector3 targetRotate = renderTF.rotation.eulerAngles + 180 * Vector3.up;
        rotateTween = renderTF.DOLocalRotate(targetRotate, rotateSpeed).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        floatTween = renderTF.DOLocalMoveY(floatMaxHeight, floatSpeed).SetLoops(-1, LoopType.Yoyo);
    }
    public void OnBeCollected(Cat cat)
    {
        rotateTween.Pause();
        floatTween.Pause();

        TF.parent = cat.TF;
        TF.DOLocalJump(Vector3.zero, jumpPower, 1, flyDuration).SetEase(Ease.OutQuad);
        TF.DOScale(0, flyDuration);
        TF.DORotate(rotateWhenCollect , flyDuration);
        OnEffect(cat);
    }
}
