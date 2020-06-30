using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }
    void OnJoinedLobby()
    {
        // 방 입장
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        // 방이 없을 경우 방 생성
        PhotonNetwork.CreateRoom("MyRoom");
    }

    void OnJoinedRoom()
    {
        CreatPlayer();
    }

    void CreatPlayer()
    {
        // 방장
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("unitychan", new Vector3(-4, 0.5f, 0), Quaternion.AngleAxis(90f, Vector3.up), 0);
        }
        // 게스트
        else
        {
            PhotonNetwork.Instantiate("unitychan", new Vector3(4, 0.5f, 0), Quaternion.AngleAxis(-90f, Vector3.up), 0);
        }
    }
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
