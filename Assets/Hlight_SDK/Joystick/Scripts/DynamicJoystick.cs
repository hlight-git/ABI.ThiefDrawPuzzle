using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        renderRectTF.gameObject.SetActive(true);
        renderRectTF.position = eventData.position;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        renderRectTF.gameObject.SetActive(false);
    }
    protected override void OnInit()
    {
        base.OnInit();
    }
}
