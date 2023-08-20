using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeReference] GameplayHandler gameplayHandler;
    [SerializeReference] LevelSet levelSet;
    [SerializeReference] Transform containerTF;

    NavMeshDataInstance navMeshDataInstance;
    public int curLevelIndex = 0;
    bool isLoaded = false;
    public Cat Cat;
    public DrawLine DrawLine { get; private set; }

    public void LoadLevel(int levelIndex)
    {
        if (isLoaded)
        {
            DespawnCurLevel();
        }
        gameplayHandler.enabled = true;
        if (levelSet.levels[levelIndex].navMeshData != null)
        {
            navMeshDataInstance = NavMesh.AddNavMeshData(levelSet.levels[levelIndex].navMeshData);
        }
        containerTF = new GameObject("LvContainer").transform;
        levelSet.levels[levelIndex].childGOData.ExtractTo(containerTF);
        Cat = FindObjectOfType<Cat>();
        DrawLine = SimplePool.Spawn<DrawLine>(PoolType.DrawLine);
        Cat.DrawLine = DrawLine;
        isLoaded = true;
    }
    public void Win()
    {
        gameplayHandler.enabled = false;
        DrawLine.Remove();
        GameManager.Ins.ChangeState(GameState.Win);
    }
    public void Lose()
    {
        gameplayHandler.enabled = false;
        DrawLine.Remove();
        GameManager.Ins.ChangeState(GameState.Lose);
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(curLevelIndex);
    }
    public void NextLevel()
    {
        curLevelIndex++;
    }
    public void DespawnCurLevel()
    {
        Destroy(containerTF.gameObject);
        DrawLine.Remove();
        gameplayHandler.enabled = false;

        if (navMeshDataInstance.valid)
        {
            navMeshDataInstance.Remove();
        }
    }
}
