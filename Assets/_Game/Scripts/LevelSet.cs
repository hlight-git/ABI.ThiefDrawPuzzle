using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "LevelSet", menuName = "SO/LevelSet")]
public class LevelSet : ScriptableObject
{
    public string navMeshDir;
    public string lvDataSetDir;
    public List<LevelData> levels;
    [Button]
    public void CollectLevels()
    {
        levels = new List<LevelData>();
        HlightSDK.Tool.Level.Compress.DataSet[] dataSets = Resources.LoadAll<HlightSDK.Tool.Level.Compress.DataSet>(lvDataSetDir);
        NavMeshData[] navMeshDatas = Resources.LoadAll<NavMeshData>(navMeshDir);
        for (int i = 0; i < dataSets.Length; i++)
        {
            LevelData lvData = new LevelData
            {
                childGOData = dataSets[i]
            };
            try
            {
                lvData.navMeshData = navMeshDatas.Single(nmd => nmd.name.EndsWith("Lv" + (i + 1)));
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning("Lv " + (i + 1) + " missing NavMeshData!");
            }
            levels.Add(lvData);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public HlightSDK.Tool.Level.Compress.DataSet childGOData;
    public NavMeshData navMeshData;
}