using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeReference] PressHandler pressHandler;
    [SerializeReference] Player playerPrefab;

    [SerializeField] Level[] levels;
    public int curLevelIndex = 0;

    public void LoadLevel(int levelIndex)
    {
        if (InsManager.Ins.Level != null)
        {
            Destroy(InsManager.Ins.Level.gameObject);
            Destroy(InsManager.Ins.Player.gameObject);
        }
        InsManager.Ins.Level = Instantiate(levels[levelIndex]);

        InsManager.Ins.Player = Instantiate(playerPrefab);
        InsManager.Ins.Player.TF.position = InsManager.Ins.Level.StartPoint;
    }
    public void LoadCurrentLevel()
    {
        LoadLevel(curLevelIndex);
    }
    public void OnStartPlay()
    {
        GameManager.Ins.ChangeState(GameState.Drawing);
        pressHandler.ChangeState(pressHandler.PressToDrawState);
    }
    public void OnWin()
    {
        InsManager.Ins.PressHandler.ClearPoints();
        GameManager.Ins.ChangeState(GameState.Win);
        UIManager.Ins.OpenUI<UIWin>();
    }

    public void OnLose()
    {
        InsManager.Ins.PressHandler.ClearPoints();
        GameManager.Ins.ChangeState(GameState.Lose);
        UIManager.Ins.OpenUI<UILose>();
    }
}
