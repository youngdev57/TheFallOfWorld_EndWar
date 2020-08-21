using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;



public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public GameObject dungeonAlert;

    void Start()
    {

    }

    public void DungeonEnterAlert()
    {
        dungeonAlert.SetActive(true);
        Invoke("EnterAction", 3f);
    }

    void EnterAction()
    {

    }

    public void DungeonEnterAction()
    {
        //던전 들어가기
    }

    [PunRPC]
    public void DungeonExitAction()
    {
        //던전 나가기
    }

    public void ShowClear()
    {
        GetComponentInChildren<DungeonExit>().ShowClearUI();
    }
}
