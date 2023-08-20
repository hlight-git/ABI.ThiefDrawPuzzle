using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomCover : GameUnit
{
    [SerializeReference] Transform lWallTF;
    [SerializeReference] Transform rWallTF;
    [SerializeReference] Transform tWallTF;
    [SerializeReference] Transform bWallTF;

    [SerializeField, Min(2)] int width;
    [SerializeField, Min(2)] int height;

    void RebuildLeftWall()
    {
        if (lWallTF == null)
        {
            return;
        }
        lWallTF.localPosition = width / 2f * Vector3.left;
        lWallTF.localScale += (height * 4 - lWallTF.localScale.z + 1) * Vector3.forward;
    }
    void RebuildRightWall()
    {
        if (rWallTF == null)
        {
            return;
        }
        rWallTF.localPosition = width / 2f * Vector3.right;
        rWallTF.localScale += (height * 4 - rWallTF.localScale.z + 1) * Vector3.forward;
    }
    void RebuildTopWall()
    {
        if (tWallTF == null)
        {
            return;
        }
        tWallTF.localPosition = height / 2f * Vector3.forward;
        tWallTF.localScale += (width * 4 - tWallTF.localScale.x + 1) * Vector3.right;
    }
    void RebuildBotWall()
    {
        if (bWallTF == null)
        {
            return;
        }
        bWallTF.localPosition = height / 2f * Vector3.back;
        bWallTF.localScale += (width * 4 - bWallTF.localScale.x + 1) * Vector3.right;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        RebuildLeftWall();
        RebuildRightWall();
        RebuildTopWall();
        RebuildBotWall();
    }

    [Button]
    void Unpack()
    {
        if (PrefabUtility.IsPartOfPrefabInstance(TF))
        {
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }
        //lWallTF.SetParent(TF.parent);
        //rWallTF.SetParent(TF.parent);
        //tWallTF.SetParent(TF.parent);
        //bWallTF.SetParent(TF.parent);
    }
#endif
}