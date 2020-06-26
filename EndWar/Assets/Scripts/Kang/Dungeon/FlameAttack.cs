using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlameAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("********************** 보스 맞음");
        }

        if(other.tag == "Monster")
        {
            other.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, 50);
        }
    }
}
