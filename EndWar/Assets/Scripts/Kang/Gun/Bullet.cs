using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 0;
    public GameObject hitEffect;

    public GameObject _hitEffect = null;
    IEnumerator effectCoroutine;
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
        effectCoroutine = HitEffect(other);
        StartCoroutine(effectCoroutine);

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

        Invoke("OffEffect", 1f);
        OffObject();
        yield return null;
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
    
    void OffEffect()
    {
        _hitEffect.SetActive(false);
    }
}
