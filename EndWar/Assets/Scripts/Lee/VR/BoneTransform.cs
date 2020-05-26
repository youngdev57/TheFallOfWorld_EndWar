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
}
