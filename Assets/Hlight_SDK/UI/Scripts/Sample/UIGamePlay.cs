using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    [SerializeReference] TextMeshProUGUI lvText;
    [SerializeReference] RectTransform guideLabelRTF;
    [SerializeReference] RectTransform shopBut1RTF;
    [SerializeReference] RectTransform shopBut2RTF;
    [SerializeField] float hiddenDuration = .5f;
    [SerializeField] float slipWidth = 150;
    bool isHidingShopButtons;

    public override void Open()
    {
        base.Open();
        lvText.text = "Level " + (LevelManager.Ins.curLevelIndex + 1);
        ShowShopButtons(true);
    }

    public void ShowShopButtons(bool value)
    {
        if (value == false && isHidingShopButtons)
        {
            return;
        }
        guideLabelRTF.gameObject.SetActive(value);
        shopBut1RTF.DOAnchorPosX((value ? 1 : -1) * slipWidth, hiddenDuration);
        shopBut2RTF.DOAnchorPosX((value ? -1 : 1) * slipWidth, hiddenDuration);
    }
    public void RestartButton()
    {
        ShowShopButtons(true);
        LevelManager.Ins.LoadCurrentLevel();
    }
    public void SettingButton()
    {
        Close(0);
        UIManager.Ins.OpenUI<UISetting>();
    }
}
