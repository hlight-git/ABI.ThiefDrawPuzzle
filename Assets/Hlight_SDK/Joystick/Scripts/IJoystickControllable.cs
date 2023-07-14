using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJoystickControllable
{
    public void OnDrag(Vector3 direction, float dragOffsetCoefficient);
    public void OnRelease();
}
