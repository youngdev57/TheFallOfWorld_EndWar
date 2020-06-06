using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CapsuleCollider))]
public class NonTargetTrigger : MonoBehaviour
{
    public int speed;
    public int damage;
    public float seconds;

    void Start()
    {
        Destroy(gameObject, seconds);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            other.GetComponent<PhotonView>().RPC("GetDamager", RpcTarget.AllBuffered, damage);
            Debug.Log("몬스타!");
        }
    }
}
