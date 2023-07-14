using UnityEngine.UI;

public class UIWin : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        UIManager.Ins.OpenUI<UIMainMenu>();
        Close(0);
    }
}
