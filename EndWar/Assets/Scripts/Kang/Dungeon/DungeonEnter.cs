﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class DungeonEnter : MonoBehaviour
{
    GameObject playerObj;

    public GameObject invalidStart;

    public Button start_btn, ready_btn, cancel_btn, invalidStartOK_btn;

    public PhotonManager myPhoton;

    GameObject[] players;

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            start_btn.gameObject.SetActive(true);
        }
        //else
        //{
        //    ready_btn.gameObject.SetActive(true);
        //}
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            EnterDungeon();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player" && other.GetComponent<PhotonView>() != null)
        //{
        //    if (other.GetComponent<PhotonView>().IsMine)
        //    {
        //        playerObj = other.gameObject;
        //        myPhoton = playerObj.GetComponent<PlayerInfo>().photonManager;

        //        playerObj.GetComponentsInChildren<UI_Laser>()[0].enabled = true;
        //        playerObj.GetComponentsInChildren<UI_Laser>()[1].enabled = true;
        //        playerObj.GetComponentsInChildren<UI_Laser>()[0].LaserOn();
        //        playerObj.GetComponentsInChildren<UI_Laser>()[1].LaserOn();
        //    }
        //}
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>() != null)
        {
            if (other.gameObject == playerObj)
            {
                playerObj.GetComponentsInChildren<UI_Laser>()[0].LaserOff();
                playerObj.GetComponentsInChildren<UI_Laser>()[1].LaserOff();
                playerObj.GetComponentsInChildren<UI_Laser>()[0].enabled = false;
                playerObj.GetComponentsInChildren<UI_Laser>()[1].enabled = false;

                ScoreExtensions.SetScore(PhotonNetwork.LocalPlayer, 0);

                if(PhotonNetwork.IsMasterClient)
                {
                    //
                }
                else
                {
                    ready_btn.gameObject.SetActive(true);
                    cancel_btn.gameObject.SetActive(false);
                }

                playerObj = null;
            }
        }
            
    }

    public void EnterDungeon()
    {
        //if(PhotonNetwork.PlayerList.Length == 1)
        //{
        //    //혼자는 출발 불가능 하다는걸 알려주자
        //    ShowInvalidStart();
        //    return;
        //}

        //int successCnt = 0;

        //foreach(Player player in PhotonNetwork.PlayerList)
        //{
        //    if(player != PhotonNetwork.LocalPlayer)
        //    {
        //        if(ScoreExtensions.GetScore(player) == 1)
        //        {
        //            successCnt++;
        //        }
        //    }
        //}

        //if(successCnt == PhotonNetwork.PlayerList.Length - 1)
        //{
        //    //출발
        //    ScoreExtensions.SetScore(PhotonNetwork.MasterClient, 1);

        //    DoEnter();
        //    Debug.Log("★★★★★ 출발 준비 됨");
        //}
        //else
        //{
        //    //아직 준비 안된 플레이어가 있다
        //    ShowInvalidStart();
        //}

        players = GameObject.FindGameObjectsWithTag("Player");

        DungeonAlert();
    }

    void DungeonAlert()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PhotonView>().RPC("DungeonEnterAlert", RpcTarget.All);    //던전 입장 메소드를 PlayerInfo 에서 실행함
        }
    }

    void ShowInvalidStart()
    {
        invalidStart.SetActive(true);
    }

    public void HideInvalidStart()
    {
        invalidStart.SetActive(false);
    }

    //public void ReadyDungeon()
    //{
    //    Player myPlayer = PhotonNetwork.LocalPlayer;

    //    if (ScoreExtensions.GetScore(myPlayer) == 0)
    //    {
    //        ScoreExtensions.SetScore(myPlayer, 1);      //준비 해주고
    //        //버튼은 준비 상태로
    //        StartCoroutine(TryEnter());
    //        ready_btn.gameObject.SetActive(false);
    //        cancel_btn.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        ScoreExtensions.SetScore(myPlayer, 0);      //준비 취소 하고
    //        //버튼은 원래 상태로
    //        StopCoroutine(TryEnter());
    //        ready_btn.gameObject.SetActive(true);
    //        cancel_btn.gameObject.SetActive(false);
    //    }
        
    //}

    //IEnumerator TryEnter()
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    if(ScoreExtensions.GetScore(PhotonNetwork.MasterClient) == 1)
    //    {
    //        //출발
    //        DoEnter();
    //        Debug.Log("손님 입장 성공");
            
    //    } else
    //    {
    //        StartCoroutine(TryEnter());
    //        Debug.Log("손님 대기중 : " + ScoreExtensions.GetScore(PhotonNetwork.MasterClient));
    //    }
    //}

    
}
