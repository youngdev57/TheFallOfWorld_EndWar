using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public GameObject player;

    void Start()
    {
        CharacterInit();
    }

    void CharacterInit()
    {
        GameObject punObj = PhotonNetwork.Instantiate("PunObject", Vector3.zero, Quaternion.identity);
        punObj.transform.parent = this.transform;

        punObj.transform.Find("Head").transform.parent =
            player.transform.Find("Camera (eye)").transform;

        punObj.transform.Find("LeftHand").transform.parent =
            player.transform.Find("Controller (left)").transform;

        punObj.transform.Find("RightHand").transform.parent =
            player.transform.Find("Controller (right)").transform;
        Debug.Log("asdsadsa");
    }
}
