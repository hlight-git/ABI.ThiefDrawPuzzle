using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : GameUnit
{
    [SerializeReference] ThiefBackpack backpack;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.Tag.COLLECTABLE_ITEM))
        {
            CollectableItem item = CollectableItemCache.Get(other);
            item.OnBeCollected(TF);
            backpack.Collect(item);
        }
    }
}
