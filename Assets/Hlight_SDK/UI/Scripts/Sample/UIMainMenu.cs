using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    public void PlayButton()
    {
        //UIManager.Ins.OpenUI<UIGamePlay>();
        //GameManager.Ins.ChangeState(GameState.Drawing);
        Close(0);
    }
}
