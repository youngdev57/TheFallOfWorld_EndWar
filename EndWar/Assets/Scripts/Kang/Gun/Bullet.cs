using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 0;
    public GameObject hitEffect;

    GameObject _hitEffect = null;
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
            other.gameObject.GetComponent<Monster>().GetDamage(damage);
        }
    }

    IEnumerator HitEffect(Collision co)
    {
        ContactPoint contactPoint = co.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        Vector3 pos = contactPoint.point;

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

        OffObject();

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
