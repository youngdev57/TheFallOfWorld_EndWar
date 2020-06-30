using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fog : MonoBehaviour
{
    public int Damage;
    private float Timer = 0f;

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 10f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, Damage);
        }
    }

    public void setDamage(int damage)
    {
        Damage = damage;
    }
}
