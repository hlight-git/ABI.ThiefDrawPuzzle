using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeReference] Transform speechPointTF;
    [SerializeReference] RequireBubble requireBubblePrefab;
    [SerializeReference] Transform leftDoorTF;
    [SerializeReference] Transform rightDoorTF;
    [SerializeField] float doActionDuration = 1f;
    [SerializeField] int requireQuantity;
    RequireBubble requireBubble;
    Vector3 SpeechPoint => MainCamera.Ins.Camera.WorldToScreenPoint(speechPointTF.position);
    private void Start()
    {
        requireBubble = Instantiate(requireBubblePrefab, SpeechPoint, Quaternion.identity);
        requireBubble.OnInit(requireQuantity);
    }
    void Open()
    {
        requireBubble.Hide();
        leftDoorTF.DOLocalRotate(180 * Vector3.back, doActionDuration);
        rightDoorTF.DOLocalRotate(Vector3.zero, doActionDuration);
    }
}
