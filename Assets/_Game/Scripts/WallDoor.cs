using HlightSDK.Tool.Level.Compress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallDoorData : HlightSDK.Tool.Level.Compress.LvGOData
{
    [Min(2)] public int width;
    [Range(0, 1)] public float doorPos;
}
public class WallDoor : PoolUnit, HlightSDK.Tool.Level.Compress.IComplexLvGO
{
    [SerializeReference] Transform lWallTF;
    [SerializeReference] Transform rWallTF;
    [SerializeReference] Transform doorTF;
    [SerializeField] WallDoorData data;
    public string CompressData => data.OnCompressed(transform);

    void RebuildDoor()
    {
        doorTF.localPosition = (data.doorPos - 0.5f) * (data.width - 2) * Vector3.right;
    }

    void RebuildLeftWall()
    {
        float wallWidth = data.width / 2f + doorTF.localPosition.x - 1;
        lWallTF.localPosition = (wallWidth - data.width) / 2 * Vector3.right;
        lWallTF.localScale += (wallWidth * 4 - lWallTF.localScale.x) * Vector3.right;
        lWallTF.gameObject.SetActive(lWallTF.localScale.x > 0);
    }

    void RebuildRightWall()
    {
        float wallWidth = data.width / 2f - doorTF.localPosition.x - 1;
        rWallTF.localPosition = (data.width - wallWidth) / 2 * Vector3.right;
        rWallTF.localScale += (wallWidth * 4 - rWallTF.localScale.x) * Vector3.right;
        rWallTF.gameObject.SetActive(rWallTF.localScale.x > 0);
    }
    void Rebuild()
    {
        RebuildDoor();
        RebuildLeftWall();
        RebuildRightWall();
    }
    public LvGOData OnExtract(string json)
    {
        data = JsonUtility.FromJson<WallDoorData>(json);
        Rebuild();
        return data;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Rebuild();
    }
#endif
}
