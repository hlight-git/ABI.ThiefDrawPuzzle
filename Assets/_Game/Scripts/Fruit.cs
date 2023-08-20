using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : ABSCollectItem
{
    [SerializeField] float scaleUp = 1.4f;
    public override void OnEffect(Cat cat)
    {
        cat.TF.localScale = scaleUp * Vector3.one;
        cat.CanBreakWall = true;
    }
}
