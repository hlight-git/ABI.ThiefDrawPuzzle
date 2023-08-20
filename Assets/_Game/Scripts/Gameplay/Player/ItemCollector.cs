using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : GameUnit
{
    [SerializeReference] Cat cat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.Tag.COLLECTABLE_ITEM))
        {
            ABSCollectItem item = CollectItemCache.Get(other);
            item.OnBeCollected(cat);
        }
    }
}
