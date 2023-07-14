using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    public Text score;

    public void MainMenuButton()
    {
        Close(0);
        GameManager.Ins.OpenMainMenu();
    }
}
