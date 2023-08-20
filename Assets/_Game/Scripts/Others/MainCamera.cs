using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{
    public enum State
    {
        MainMenu = 0,
        Drawing = 1,
        Moving = 2,
    }

    [SerializeReference] Camera self;
    [SerializeReference] Raycaster3D raycaster;
    [SerializeField] Transform[] statePositions;

    [SerializeField] float slipSpeed = 5f;

    Vector3 targetOffset;
    Quaternion targetRotate;

    State curState;
    public bool isTesting;
    public Camera Camera => self;

    private void LateUpdate()
    {
        if (!isTesting)
        {
            //Vector3 targetOffset = this.targetOffset + (curState == State.Moving ? InsManager.Ins.Player.TF.position : Vector3.zero);
            TF.SetPositionAndRotation(
                Vector3.Lerp(TF.position, targetOffset, Time.deltaTime * slipSpeed),
                Quaternion.Lerp(TF.rotation, targetRotate, Time.deltaTime * slipSpeed)
            );
        }
    }
    public void ChangeState(State state)
    {
        curState = state;
        targetOffset = statePositions[(int)state].localPosition;
        targetRotate = statePositions[(int)state].localRotation;
        //if (curState == State.MainMenu)
        //{
        //    Transform playerTF = InsManager.Ins.Player.TF;
        //    targetOffset = playerTF.position + targetOffset.y * Vector3.up + targetOffset.z * Vector3.forward;
        //    targetRotate.eulerAngles += playerTF.rotation.eulerAngles;
        //    //TF.LookAt(InsManager.Ins.Player.TF);
        //    //targetOffset = TF.rotation.eulerAngles;
        //}
    }

    public RaycastHit? Raycast(int layerMask) => raycaster.Raycast(self.ScreenPointToRay(Input.mousePosition).direction, layerMask: layerMask);
}