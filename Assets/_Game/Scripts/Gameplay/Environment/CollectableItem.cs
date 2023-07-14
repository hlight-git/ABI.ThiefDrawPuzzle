using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : GameUnit
{
    [SerializeField] Vector3 rotateWhenCollect = Vector3.up * 180; 
    [SerializeField] float flyDuration = .75f;
    [SerializeField] float jumpPower = 3f;
    [SerializeField] float value = 1f;
    public float Value => value;
    public void OnBeCollected(Transform collectorTF)
    {
        TF.parent = collectorTF;
        TF.DOLocalJump(collectorTF.localPosition, jumpPower, 1, flyDuration);
        TF.DOScale(0, flyDuration);
        TF.DORotate(rotateWhenCollect , flyDuration);
    }
}
