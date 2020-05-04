using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gate : MonoBehaviour
{
    [SerializeField]
    PlayerPoints pointManager;

    public int destination;

    private void Start()
    {
        pointManager = PlayerPoints.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("TestPlayer(Clone)"))
        {
            if(other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                pointManager.photonManager.SetDestination(destination);
                pointManager.SendMessage("LoadScene", destination);
            }
        }
    }
}
