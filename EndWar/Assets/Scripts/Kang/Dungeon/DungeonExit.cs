using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;


public class DungeonExit : MonoBehaviour
{
    internal PhotonTest photonManager;

    public GameObject dungeonQuit_UI;
    public GameObject dungeonClear_UI;

    public GameObject dungeonWelcome;



    public TextMeshProUGUI coreTxt;
    public TextMeshProUGUI soulGemTxt;
    public TextMeshProUGUI redStoneTxt;

    int reward_Core = 0;
    int reward_SoulGem = 0;
    int reward_RedStone = 0;

    bool isClear = false;

    void Start()
    {
        if(ScoreExtensions.GetScore(PhotonNetwork.LocalPlayer) == 1)
        {
            StartCoroutine(CheckDungeonEnd());
            StartCoroutine(WelcomePopUp());
        }
    }

    IEnumerator WelcomePopUp()
    {
        dungeonWelcome.SetActive(true);
        yield return new WaitForSeconds(3f);
        dungeonWelcome.SetActive(false);
    }

    public void ShowQuitUI()
    {
        dungeonQuit_UI.SetActive(true);
    }
    
    public void ShowClearUI()
    {
        dungeonClear_UI.SetActive(true);

        reward_Core = Random.Range(8, 12);
        reward_SoulGem = Random.Range(5, 10);
        reward_RedStone = Random.Range(3, 8);

        coreTxt.text = "코어\n" + reward_Core + "개";
        soulGemTxt.text = "소울젬\n" + reward_SoulGem + "개";
        redStoneTxt.text = "레드스톤\n" + reward_RedStone + "개";
    }

    public void HideQuitUI()
    {
        dungeonQuit_UI.SetActive(false);
    }

    public void Exit_Quit()
    {
        ScoreExtensions.SetScore(PhotonNetwork.LocalPlayer, 0);
        photonManager.destination = 2;
        photonManager.SendMessage("LeaveRoom");

        //클리어 못하고 도중 퇴장 (사망 아님)
    }

    public void Exit_Clear()
    {
        //보상 받고 밖으로
        photonManager.KPM.inven.gems[3] += reward_Core;
        photonManager.KPM.inven.gems[4] += reward_SoulGem;
        photonManager.KPM.inven.gems[5] += reward_RedStone;
        photonManager.KPM.inven.SaveInven(false);

        ScoreExtensions.SetScore(PhotonNetwork.LocalPlayer, 0);
        photonManager.destination = 2;
        photonManager.SendMessage("LeaveRoom");
    }

    IEnumerator CheckDungeonEnd()
    {
        yield return new WaitForSeconds(0.3f);

        if (ScoreExtensions.GetScore(PhotonNetwork.LocalPlayer) == 2 && !isClear)
        {
            //끝나는 UI 띄우기
            ShowClearUI();
            isClear = true;
        } else if(!isClear)
        {
            StartCoroutine(CheckDungeonEnd());
        }
    }
}
