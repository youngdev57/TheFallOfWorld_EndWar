using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerPoints : MonoBehaviourPunCallbacks
{
    private static PlayerPoints instance;
    public static PlayerPoints GetInstance()
    {
        return instance;
    }

    internal PhotonTest photonManager;

    [SerializeField]
    internal Transform[] points;

    public GameObject outGate;

    void Awake()
    {
        instance = this;
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void LoadScene(int destination)
    {
        switch(destination)
        {
            case 0:
                photonManager.LoadBaseScene();
                break;
            case 1:
                photonManager.LoadIceScene();
                break;
        }
    }
}
