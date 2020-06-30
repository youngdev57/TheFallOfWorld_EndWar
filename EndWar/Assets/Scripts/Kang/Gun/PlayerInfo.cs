using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;



public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public Text nickname;   //머리위에 표시될 게임 아이디

    [SerializeField]
    internal PhotonTest photonManager;

    public GameObject dungeonAlert;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        nickname.text = photonView.Owner.NickName;
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
}
