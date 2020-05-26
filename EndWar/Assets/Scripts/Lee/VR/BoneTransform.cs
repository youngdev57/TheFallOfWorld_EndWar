using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneTransform : MonoBehaviour
{
    public Transform[] boneTr;

    public static Transform[] boneTransform;

    void Start()
    {
        boneTransform = boneTr;
    }

    public Transform[] GetBoneTr()
    {
        return boneTransform;
    }

    public void GetBone()
    {
        for (int i = 0; i < boneTr.Length; i++)
        {
            if (boneTr[i].GetChild(0))
            {
                

            }
        }
    }
}
