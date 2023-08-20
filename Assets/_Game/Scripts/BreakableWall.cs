using HlightSDK.Tool.Level.Compress;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BreakableWall : PoolUnit, ICatInteractable
{
    const float BOX_SIZE = 0.6f;
    [SerializeReference] BoxCollider boxCollider;
    public List<PoolUnit> walls;

    public void OnInteracted(Cat cat)
    {
        if (!cat.CanBreakWall)
        {
            cat.OnDeath();
        }
    }
#if UNITY_EDITOR
    PoolUnit SampleA => PrefabUtility.GetCorrespondingObjectFromOriginalSource(walls[0]);
    PoolUnit SampleB => PrefabUtility.GetCorrespondingObjectFromOriginalSource(walls[1]);


    void AddWall(bool isPlaying)
    {
        return;
        PoolUnit sample = walls.Count % 2 == 0 ? SampleA : SampleB;
        if (sample == null)
        {
            sample = walls.Count % 2 == 0 ? walls[0] : walls[1];
        }
        PoolUnit wall;
        if (isPlaying)
        {
            wall = Instantiate(sample, TF);//SimplePool.Spawn<PoolUnit>(sample.poolType, TF);
        }
        else
        {
            wall = PrefabUtility.InstantiatePrefab(sample, TF) as PoolUnit;
        }
        walls.Add(wall);

        UpdateAfterChangeWallList();
    }
    void UpdateAfterChangeWallList()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            walls[i].TF.localPosition = BOX_SIZE * (i - walls.Count / 2f + .5f) * Vector3.right;
            walls[i].TF.localRotation = Quaternion.identity;
            walls[i].TF.localScale = Vector3.one;
            boxCollider.size += (walls.Count * BOX_SIZE + .1f - boxCollider.size.x) * Vector3.right;
        }
    }

    [Button]
    void Append()
    {
        AddWall(false);
    }

    [Button]
    void Pop()
    {
        if (walls.Count == 2)
        {
            return;
        }
        DestroyImmediate(walls.Last().gameObject);
        walls.RemoveAt(walls.Count - 1);
        UpdateAfterChangeWallList();
    }
#endif
}
