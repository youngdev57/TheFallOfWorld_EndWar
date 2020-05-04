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

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        OnLogin();
    }

    void OnLogin()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room");
        this.CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("gh");
        Invoke("CharacterInit", 1f);
    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    void CharacterInit()
    {
        GameObject player = Instantiate(this.player);
        player.name = "Player";
        player.transform.position = Vector3.zero;

        GameObject punObj = PhotonNetwork.Instantiate("PunObject", Vector3.zero, Quaternion.identity);
        punObj.transform.parent = GameObject.Find("Player").transform;

        punObj.transform.Find("Head").transform.parent =
            player.transform.Find("Camera").transform;

        punObj.transform.Find("LeftHand").transform.parent =
            player.transform.Find("Controller (left)").transform;

        punObj.transform.Find("RightHand").transform.parent =
            player.transform.Find("Controller (right)").transform;
    }
}
