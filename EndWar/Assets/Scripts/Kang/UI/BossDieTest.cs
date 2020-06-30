using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;


public class BossDieTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            //foreach(Player player in PhotonNetwork.PlayerList)
            //{
            //    ScoreExtensions.SetScore(player, 2);
            //}

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach(GameObject player in players)
            {
                player.GetComponent<PhotonView>().RPC("ShowClearUI", RpcTarget.All);
            }

            gameObject.SetActive(false);
        }
    }
}
