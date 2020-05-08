using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gate : MonoBehaviour
{
    public int destination;

    private void Start()
    {
        pointManager = PlayerPoints.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("Player(Clone)"))
        {
            if(other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                PhotonTest myPhoton = other.gameObject.GetComponent<PlayerInfo>().photonManager;

                myPhoton.destination = destination;
                Debug.Log("어디로 가는가 : " + myPhoton.destination);
                myPhoton.SendMessage("LeaveRoom");
            }
        }
    }
}
