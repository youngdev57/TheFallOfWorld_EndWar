using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string gmaeVersion = "1.0";
    public string nickName = "Y";
    public GameObject player;

    public override void OnJoinedRoom()
    {
        Invoke("CharacterInit", .5f);
    }

    void CharacterInit()
    {
        GameObject player = Instantiate(this.player);
        player.name = "Player";
        player.transform.position = Vector3.zero;

        GameObject punObj = PhotonNetwork.Instantiate("PunObject", Vector3.zero, Quaternion.identity);
        punObj.transform.parent = GameObject.Find("Player").transform;

        punObj.transform.Find("Head").transform.parent =
            player.transform.Find("Camera (eye)").transform;

        punObj.transform.Find("LeftHand").transform.parent =
            player.transform.Find("Controller (left)").transform;

        punObj.transform.Find("RightHand").transform.parent =
            player.transform.Find("Controller (right)").transform;
    }
}
