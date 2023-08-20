using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Vector3 x = Vector3.zero;
        T(x);
        print(x);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 vector3 = (Camera.main.ScreenPointToRay(Input.mousePosition).direction);
            //if (Physics.Raycast(Camera.main.transform.position, vector3, out RaycastHit hit, 100f))
            //{
            //    print(hit.collider.gameObject.name);
            //}
            print(MainCamera.Ins.Raycast(Util.LayerOf(Const.Layer.CATCH_RAY))?.point);
        }
    }
    void T(Vector3 t)
    {
        t += Vector3.one;
    }
}
