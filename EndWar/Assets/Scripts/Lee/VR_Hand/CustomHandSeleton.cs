﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CustomHandSeleton : MonoBehaviour
{
    public SteamVR_Action_Single gripAction;
    public SteamVR_Action_Boolean goodAction;
    [Space(10)]
    public Animator anim;
    public WHATHAND hand;

    SteamVR_Behaviour_Pose pose;

    void Awake()
    {
        pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        gripAction[pose.inputSource].onChange += Grip;
        goodAction[pose.inputSource].onChange += Good;
    }

    void Start(){}

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
