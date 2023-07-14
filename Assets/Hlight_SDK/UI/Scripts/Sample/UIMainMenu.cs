using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    public void PlayButton()
    {
        //UIManager.Ins.OpenUI<UIGamePlay>();
        LevelManager.Ins.OnStartPlay();
        Close(0);
    }
}
