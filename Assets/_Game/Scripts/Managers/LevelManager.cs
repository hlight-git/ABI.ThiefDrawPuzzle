using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeReference] PressHandler pressHandler;
    
    public void LoadLevel()
    {

    }
    public void OnStartPlay()
    {
        GameManager.Ins.ChangeState(GameState.Drawing);
        pressHandler.ChangeState(pressHandler.PressToDrawState);
    }
    public void OnWin()
    {
        GameManager.Ins.ChangeState(GameState.Win);
        UIManager.Ins.OpenUI<UIWin>();
    }

    public void OnLose()
    {
        GameManager.Ins.ChangeState(GameState.Lose);
        UIManager.Ins.OpenUI<UILose>();
    }
}
