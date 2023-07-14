using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBackpack : GameUnit
{
    float value = 0f;
    public void Collect(CollectableItem item)
    {
        value += item.Value;
        TF.DOScale(0.25f + value / 100, .75f);
    }
}
