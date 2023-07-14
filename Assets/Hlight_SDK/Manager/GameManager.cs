using UnityEngine;

public enum GameState
{
    MainMenu = 0,
    Drawing = 1,
    Moving = 2,
    Lose = 3,
    Win = 4,
}

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private static GameState gameState;
    public bool openUIWhenPlay;

    protected void Awake()
    {
        GameSetup();
        LoadData();

        OpenMainMenu();
        ChangeState(GameState.MainMenu);
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
        //csv.OnInit();
    }
    public void OpenMainMenu()
    {
        if (openUIWhenPlay)
        {
            UIManager.Ins.OpenUI<UIMainMenu>();
        }
        LevelManager.Ins.LoadCurrentLevel();
    }
    public void ChangeState(GameState state)
    {
        if ((int) state < 3)
        {
            InsManager.Ins.PlayerLineOfSight.ChangeState((PlayerLineOfSight.State)state);
        }
        gameState = state;
    }

    public bool IsState(GameState state) => gameState == state;
    public bool IsPlaying => IsState(GameState.Drawing) || IsState(GameState.Moving);
}
