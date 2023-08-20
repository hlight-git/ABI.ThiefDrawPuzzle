using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UICanvas
{
    public void ContinueButton()
    {
        Close(0);
        UIManager.Ins.OpenUI<UIGamePlay>();
    }
}
