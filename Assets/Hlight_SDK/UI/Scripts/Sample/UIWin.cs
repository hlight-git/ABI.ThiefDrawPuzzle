using UnityEngine.UI;

public class UIWin : UICanvas
{
    public Text score;

    public void NoThankButton()
    {
        Close(0);
        LevelManager.Ins.NextLevel();
        GameManager.Ins.ChangeState(GameState.Gameplay);
    }
    public void GetBonusButton()
    {
        Close(0);
        LevelManager.Ins.NextLevel();
        GameManager.Ins.ChangeState(GameState.Gameplay);
    }
}
