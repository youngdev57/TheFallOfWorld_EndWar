using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class CustomHandSeleton : MonoBehaviourPun
{
    public SteamVR_Action_Single gripAction;
    public SteamVR_Action_Boolean goodAction;
    [Space(10)]
    public Animator anim;
    public PhotonView myPv;
    public WHATHAND hand;

    SteamVR_Behaviour_Pose pose;
    BoneTransform boneTransform;

    void Awake()
    {
        if (myPv == null)
            myPv = GetComponent<PhotonView>();

        if (!myPv.IsMine)
            return;

        pose = GetComponentInParent<SteamVR_Behaviour_Pose>();

        gripAction[pose.inputSource].onChange += Grip;
        goodAction[pose.inputSource].onChange += Good;
    }

    void Start()
    {
        boneTransform = GetComponent<BoneTransform>();
    }

    /*void OnDisable()
    {
        if (myPv.IsMine)
        { 
            gripAction[pose.inputSource].onChange -= Grip;
            goodAction[pose.inputSource].onActiveChange -= Good;
        }
    }*/

    public void SetBone(BoneTransform bone, int index)
    {
        if (index == 0)
        {
            anim.enabled = true;
        }
        else if (index == 1)
        {
            anim.enabled = false;

            List<Transform> temp = bone.GetBone();
            List<Transform> myBone = boneTransform.GetBone();

            for (int i = 0; i < temp.Count; i++)
            {
                myBone[i].position = temp[i].position;
                myBone[i].rotation = temp[i].rotation;
            }
        }
    }

    void Grip(SteamVR_Action_Single action, SteamVR_Input_Sources source, float axis, float delta)
    {
        anim.SetFloat("GripBlend", axis);
    }

    void Good(SteamVR_Action_Boolean action, SteamVR_Input_Sources source, bool active )
    {
        if (active)
            anim.SetFloat("TriggerBlend", 1f);
        else
            anim.SetFloat("TriggerBlend", 0.9f);
    }
}
