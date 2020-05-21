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
    public WHATHAND hand;

    SteamVR_Behaviour_Pose pose;

    void Awake()
    {
        if (hand == WHATHAND.LEFT)
             transform.parent = transform.parent.GetComponent<ViveManager>().leftHand.transform;
        if(hand == WHATHAND.RIGHT)
            transform.parent = transform.parent.GetComponent<ViveManager>().rightHand.transform;

        pose = GetComponentInParent<SteamVR_Behaviour_Pose>();

        gripAction[pose.inputSource].onChange += Grip;
        goodAction[pose.inputSource].onChange += Good;
    }

    void OnDestroy()
    {
        gripAction[pose.inputSource].onChange -= Grip;
        goodAction[pose.inputSource].onActiveChange -= Good;
    }

    void Grip(SteamVR_Action_Single action, SteamVR_Input_Sources source, float axis, float delta)
    {
        anim.SetFloat("GripBlend", axis);
    }

    void Good(SteamVR_Action_Boolean action, SteamVR_Input_Sources source, bool active )
    {
        float axis = 0;
        if (active)
            axis = Mathf.Lerp(0f, 1f, 0.5f);

        anim.SetFloat("GoodBlend", axis);
    }

}
