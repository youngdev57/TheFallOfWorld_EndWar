using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OffUIActive : MonoBehaviourPun
{
    public PhotonView player;

    void Start()
    {
        if (!player.IsMine)
            gameObject.SetActive(false);
    }
}
