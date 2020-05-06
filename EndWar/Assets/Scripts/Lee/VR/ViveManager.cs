using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ViveManager : MonoBehaviourPun
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    public static ViveManager Instance;

    void Awake()
    {
        if (Instance == null && photonView.IsMine)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
