﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;



public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public Text nickname;   //머리위에 표시될 게임 아이디

    [SerializeField]
    internal PhotonManager photonManager;

    public GameObject dungeonAlert;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        nickname.text = photonView.Owner.NickName;

        if(GetComponent<PhotonView>().IsMine)
        {
            nickname.transform.parent.gameObject.SetActive(false);
        }
    }

    /** ??? **/

    void LeaveRoom()
    {
        photonManager.LeaveRoom();
    }

    [PunRPC]
    public void DungeonEnterAlert()
    {
        dungeonAlert.SetActive(true);
        Invoke("EnterAction", 3f);
    }

    void EnterAction()
    {
        GetComponent<PhotonView>().RPC("DungeonEnterAction", RpcTarget.All);
    }

    [PunRPC]
    public void DungeonEnterAction()
    {
        photonManager.destination = 3;
        photonManager.SendMessage("LeaveRoom");
    }

    [PunRPC]
    public void DungeonExitAction()
    {
        photonManager.destination = 2;
        photonManager.SendMessage("LeaveRoom");
    }

    [PunRPC]
    public void AddGold(int gold)
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            photonManager.KPM.inven.AddGold(gold, false);
        }
    }

    [PunRPC]
    public void ShowClear()
    {
        GetComponentInChildren<DungeonExit>().ShowClearUI();
    }

    //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //foreach(GameObject p in players)
    //{
    //    p.GetComponent<PhotonView>().RPC("AddGold", RpcTarget.All, 100);
    //}
}
