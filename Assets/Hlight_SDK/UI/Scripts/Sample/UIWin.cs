using UnityEngine.UI;

public class UIWin : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        //UIManager.Ins.OpenUI<UIMainMenu>();
        GameManager.Ins.OpenMainMenu();
        Close(0);
    }
}
