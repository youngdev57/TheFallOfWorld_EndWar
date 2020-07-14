using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public int Damage;
    private float Timer = 0f;
    private float dealTimer = 0f;
    private bool attack = false;

    private void Update()
    {
        Timer += Time.deltaTime;
        dealTimer += Time.deltaTime;
        if (Timer >= 10f)
        {
            Timer = 0f;
            gameObject.SetActive(false);
        }
        if (dealTimer >= 2f)
        {
            attack = true;
            dealTimer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && attack)
        {
            attack = false;
            //   other.transform.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, Damage);
            other.GetComponent<PlayerManager>().GetDamage(Damage);
        }
    }

    public void setDamage(int damage)
    {
        Damage = damage;
    }
}
