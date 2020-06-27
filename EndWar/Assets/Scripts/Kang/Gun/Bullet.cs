using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public int damage = 0;

    Rigidbody rigid;
    void Start()
    {
        OnEnable();
        rigid = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        StartCoroutine(OffObjectCoroutine());
    }

    void Update()
    {
        transform.forward = rigid.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                OffObject();
                other.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, damage);
            }
        }
    }

    IEnumerator OffObjectCoroutine()
    {
        yield return new WaitForSeconds(1f);
        OffObject();
    }

    void OffObject()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
