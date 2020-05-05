﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class ControllerGrabObj : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;   //컨트롤러 정보
    public SteamVR_Action_Boolean grabAction;       //그랩 액션

    GameObject collidingObj; //현재 충돌중인 객체
    GameObject objectInHand; //플레이어가 잡은 객체

    void Update()
    {
        //잡는 버튼을 누를떄
        if (grabAction.GetLastState(handType))
        {
            if (collidingObj)
            {
                GrabObj();
            }
        }
        //잡는 버튼을 땔때
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObj();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObj(other);
    }
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObj(other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObj)
            return;

        collidingObj = null;
    }

    //충돌중인 객체로 설정
    void SetCollidingObj(Collider col)
    {
        if (collidingObj || !col.GetComponent<Rigidbody>())
            return;

        collidingObj = col.gameObject;
    }

    //객체를 잡음
    void GrabObj()
    {
        objectInHand = collidingObj; //잡은 객체로 설정
        collidingObj = null; //충돌 객체 해제

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    //FixedJoint => 객체들을 하나로 묶어 고정시겨줌
    //breakForce => 조인트가 제거되도록 하기 위한 필요한 힘의 크기
    //breakTorque => 조인트가 제거되도록 하기 위한 필요한 토크
    FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
    
    void ReleaseObj()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = 
                controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = 
                controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }
}
