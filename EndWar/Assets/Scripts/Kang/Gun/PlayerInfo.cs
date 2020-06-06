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

    void Start()
    {
        nickname.text = photonView.Owner.NickName;
    }

    /** ??? **/

    void LeaveRoom()
    {
        photonManager.LeaveRoom();
    }
}
