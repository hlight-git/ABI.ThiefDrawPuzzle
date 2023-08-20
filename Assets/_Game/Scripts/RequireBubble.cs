using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireBubble : GameUnit
{
    [SerializeReference] TextMeshProUGUI quantityText;
    [SerializeField] float floatingOffset = 10f;
    [SerializeField] float doActionDuration = 1f;
    [SerializeField] List<RectTransform> bubbleRTFList;
    [ReadOnly, SerializeField] Vector2[] anchorPosCache;
    bool isShowing = false;
    private void Awake()
    {
        TF.SetParent(UIManager.Ins.CanvasParentTF);
        for (int i = 0; i < bubbleRTFList.Count; i++)
        {
            bubbleRTFList[i].anchoredPosition = Vector2.zero;
            bubbleRTFList[i].localScale = Vector3.zero;
        }
    }
    public void OnInit(int quantity)
    {
        UpdateText("x" + quantity);
        Show();
    }
    public void UpdateText(string text)
    {
        quantityText.text = text;
    }
    public void Show()
    {
        isShowing = true;
        for (int i = 0; i < bubbleRTFList.Count; i++)
        {
            bubbleRTFList[i].DOAnchorPos(anchorPosCache[i], doActionDuration);
            bubbleRTFList[i].DOScale(Vector3.one, doActionDuration);
        }
        TF.DOMove(TF.position + Vector3.up * floatingOffset, doActionDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    public void Hide()
    {
        isShowing = false;
        TF.DOKill();
        for (int i = 0; i < bubbleRTFList.Count; i++)
        {
            bubbleRTFList[i].DOAnchorPos(Vector2.zero, doActionDuration);
            bubbleRTFList[i].DOScale(Vector3.zero, doActionDuration);
        }
    }
#if UNITY_EDITOR
    [Button]
    void UpdateCaches()
    {
        anchorPosCache = bubbleRTFList.Select(rtf => rtf.anchoredPosition).ToArray();
    }
#endif
}
