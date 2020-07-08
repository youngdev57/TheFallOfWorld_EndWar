using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public int damage = 0;
    public GameObject hitEffect;

    GameObject _hitEffect;
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

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(HitEffect(other));

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            other.gameObject.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, damage);
            Debug.Log("몬스터 맞음");
            OffObject();

        }
    }

    IEnumerator HitEffect(Collision co)
    {
        ContactPoint contact = co.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (_hitEffect == null)
        {
            _hitEffect = Instantiate(hitEffect, pos, rot);
        }
        else
        {
            _hitEffect.transform.position = pos;
            _hitEffect.transform.rotation = rot;
            _hitEffect.SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        _hitEffect.SetActive(false);
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
