using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick :
    MonoBehaviour,
    IDragHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] protected RectTransform renderRectTF;
    [SerializeField] protected Image coverImage;
    [SerializeField] protected Image handleImage;

    protected IJoystickControllable controlTarget;
    protected Vector2 recentOffset;

    private void Awake() => OnInit();
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2? inputPosition = GetInputPosition(eventData);
        if (inputPosition == null)
        {
            return;
        }

        Vector2 offset = Normalized(inputPosition.Value);
        MoveHandle(offset);

        Vector3 direction = offset.x * Vector3.right + offset.y * Vector3.forward;
        if (direction != Vector3.zero)
        {
            controlTarget.OnDrag(direction, offset.magnitude);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        handleImage.rectTransform.anchoredPosition = Vector2.zero;

        controlTarget.OnRelease();
    }
    protected virtual void OnInit()
    {
        recentOffset = Vector2.zero;
    }
    Vector2? GetInputPosition(PointerEventData eventData)
    {
        if (
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                renderRectTF,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 inputPosition
            )
        )
        {
            return inputPosition;
        }
        return null;
    }
    Vector2 Normalized(Vector2 direction)
    {
        direction.x /= coverImage.rectTransform.sizeDelta.x;
        direction.y /= coverImage.rectTransform.sizeDelta.y;
        if (direction.sqrMagnitude > 1f)
        {
            return direction.normalized;
        }
        return direction;
    }
    protected virtual void MoveHandle(Vector2 offset)
    {
        handleImage.rectTransform.anchoredPosition = new Vector2(
            offset.x * coverImage.rectTransform.sizeDelta.x,
            offset.y * coverImage.rectTransform.sizeDelta.y
        );
    }

    public void SetTarget(IJoystickControllable joystickControlTarget)
    {
        controlTarget = joystickControlTarget;
    }
}
