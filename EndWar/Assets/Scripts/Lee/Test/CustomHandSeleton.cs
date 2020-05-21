using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class CustomHandSeleton : SteamVR_Behaviour_Skeleton
{
    PhotonView myPv;
  
    new void Awake()
    {
        myPv = GetComponent<PhotonView>();
        if (myPv.IsMine)
            base.Awake();
    }

    new void OnEnable()
    {
        if (myPv.IsMine)
            base.OnEnable();
    }

    new void OnDisable()
    {
        if (myPv.IsMine)
            base.OnDisable();
    }

    public void UseGetBonePositions()
    {
        myPv.RPC("C_GetBonePositions", RpcTarget.AllBuffered, null);
    }

    public void UseGetBoneRotations()
    {
        myPv.RPC("C_GetBoneRotations", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public Quaternion[] C_GetBoneRotations()
    {
        if (skeletonAvailable)
        {
            Quaternion[] rawSkeleton = skeletonAction.GetBoneRotations();
            if (mirroring == MirrorType.LeftToRight || mirroring == MirrorType.RightToLeft)
            {
                for (int boneIndex = 0; boneIndex < rawSkeleton.Length; boneIndex++)
                {
                    rawSkeleton[boneIndex] = MirrorRotation(boneIndex, rawSkeleton[boneIndex]);
                }
            }

            return rawSkeleton;

        }
        else
        {
            //fallback to getting skeleton pose from skeletonPoser
            if (fallbackPoser != null)
            {
                return fallbackPoser.GetBlendedPose(skeletonAction, inputSource).boneRotations;
            }
            else
            {
                Debug.LogError("Skeleton Action is not bound, and you have not provided a fallback SkeletonPoser. Please create one to drive hand animation when no skeleton data is available.", this);
                return null;
            }
        }
    }    

    [PunRPC]
    public Vector3[] C_GetBonePositions()
    {
        if (skeletonAvailable)
        {
            Vector3[] rawSkeleton = skeletonAction.GetBonePositions();
            if (mirroring == MirrorType.LeftToRight || mirroring == MirrorType.RightToLeft)
            {
                for (int boneIndex = 0; boneIndex < rawSkeleton.Length; boneIndex++)
                {
                    rawSkeleton[boneIndex] = MirrorPosition(boneIndex, rawSkeleton[boneIndex]);
                }
            }

            return rawSkeleton;
        }
        else
        {
            //fallback to getting skeleton pose from skeletonPoser
            if (fallbackPoser != null)
            {
                return fallbackPoser.GetBlendedPose(skeletonAction, inputSource).bonePositions;
            }
            else
            {
                Debug.LogError("Skeleton Action is not bound, and you have not provided a fallback SkeletonPoser. Please create one to drive hand animation when no skeleton data is available.", this);
                return null;
            }
        }
    }
}
