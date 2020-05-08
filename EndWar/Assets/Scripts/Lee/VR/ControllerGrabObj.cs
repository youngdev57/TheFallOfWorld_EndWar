using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class ControllerGrabObj : MonoBehaviourPun
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;   //컨트롤러 정보
    public SteamVR_Action_Boolean grabAction;       //그랩 액션

    GameObject collidingObj; //현재 충돌중인 객체
    GameObject objectInHand; //플레이어가 잡은 객체
    PhotonView myPv;

    void Start()
    {
        myPv = transform.parent.GetComponent<PhotonView>();
        controllerPose = SteamVR_Render.Top().origin.Find("Controller (right)").GetComponent<SteamVR_Behaviour_Pose>();
    }

    void Update()
    {
        if (!myPv.IsMine)
            return;

        //잡는 버튼을 누를떄
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObj)
            {
                photonView.RPC("GrabObj", RpcTarget.AllBuffered, null);
            }
        }
        //잡는 버튼을 땔때
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                photonView.RPC("ReleaseObj", RpcTarget.AllBuffered, null);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //아이템 레이어로 바꿀것
            SetCollidingObj(other);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //아이템 레이어로 바꿀것
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
    [PunRPC]
    void GrabObj()
    {
        objectInHand = collidingObj; //잡은 객체로 설정
        collidingObj = null; //충돌 객체 해제

        objectInHand.GetComponent<BoxCollider>().isTrigger = true;
        objectInHand.GetComponent<Rigidbody>().useGravity = false;

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

    [PunRPC]
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
        objectInHand.GetComponent<BoxCollider>().isTrigger = false;
        objectInHand.GetComponent<Rigidbody>().useGravity = true;
        objectInHand = null;
    }
}
