using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerName : MonoBehaviourPunCallbacks
{
    public Text nickname;

    void Start()
    {
        nickname.text = photonView.Owner.NickName;
    }
}
