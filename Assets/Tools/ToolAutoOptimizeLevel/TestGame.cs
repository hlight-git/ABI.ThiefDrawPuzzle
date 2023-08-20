using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestGame : MonoBehaviour
{
    public LevelSet LevelSet;
    public int lv;
    [Button]
    private void Clear()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    [Button]
    private void InitLevel()
    {
        Clear();
        SimplePool.CollectAll();
        LoadLevel(LevelSet.levels[lv]);
    }
    void LoadLevel(LevelData levelData)
    {
        levelData.childGOData.ExtractTo(transform);
        NavMesh.AddNavMeshData(levelData.navMeshData);
    }
}
