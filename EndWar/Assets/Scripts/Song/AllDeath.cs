using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AllDeath : MonoBehaviour
{
    public int Damage = 999999999;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, Damage);
        }
    }
}
