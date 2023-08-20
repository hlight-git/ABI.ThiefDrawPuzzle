using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Gameplay = 0,
    Setting = 1,
    Win = 2,
    Lose = 3,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] bool loadUserDataWhenPlay;

    GameState gameState;
    void Awake()
    {
        GameSetup();
        LoadData();
    }
    void Start()
    {
        ChangeState(GameState.Gameplay);
    }
    void GameSetup()
    {
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }
    void LoadData()
    {
        if (!loadUserDataWhenPlay)
        {
            return;
        }
    }
    void OnChangeToGameplayState(bool isTemporary)
    {
        UIManager.Ins.OpenUI<UIGamePlay>();
        if (!isTemporary)
        {
            LevelManager.Ins.LoadCurrentLevel();
        }
    }

    void OnChangeToSettingState(bool isTemporary)
    {
        UIManager.Ins.CloseUI<UIGamePlay>();
        UIManager.Ins.OpenUI<UISetting>();
    }

    IEnumerator OnChangeToWinState(bool isTemporary)
    {
        UIManager.Ins.CloseUI<UIGamePlay>();
        yield return Wait.ForSec(1);
        UIManager.Ins.OpenUI<UIWin>();
    }

    IEnumerator OnChangeToLoseState(bool isTemporary)
    {
        UIManager.Ins.CloseUI<UIGamePlay>();
        yield return Wait.ForSec(1);
        UIManager.Ins.OpenUI<UILose>();
    }

    public bool IsState(GameState state) => gameState == state;
    public bool IsPlaying => IsState(GameState.Gameplay);
    public void ChangeState(GameState state, bool isTemporary = false)
    {
        gameState = state;
        switch (gameState)
        {
            case GameState.Gameplay: OnChangeToGameplayState(isTemporary); break;
            case GameState.Setting: OnChangeToSettingState(isTemporary); break;
            case GameState.Win: StartCoroutine(OnChangeToWinState(isTemporary)); break;
            case GameState.Lose: StartCoroutine(OnChangeToLoseState(isTemporary)); break;
        }
    }
}
