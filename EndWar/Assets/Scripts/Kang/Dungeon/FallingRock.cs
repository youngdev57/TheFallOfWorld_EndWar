using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FallingRock : MonoBehaviour
{
    Rigidbody rig;
    PhotonView pv;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            pv.RPC("GravityOn", RpcTarget.All);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            pv.RPC("GravityOn", RpcTarget.All);
            Debug.Log("돌을 맞췄다");
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Finish")
        {
            Debug.Log("돌 충돌 !!!!!!$$$$$$$$$$$$$$");
            pv.RPC("GravityOff", RpcTarget.All);
        }

        if (coll.gameObject.tag == "Monster")
        {
            Debug.Log("돌 충돌 !!!!!!$$$$$$$$$$$$$$");
            pv.RPC("GravityOff", RpcTarget.All);
            coll.gameObject.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, 70);
        }
    }

    [PunRPC]
    void GravityOn()
    {
        rig.useGravity = true;
        rig.constraints = RigidbodyConstraints.None;
    }

    [PunRPC]
    void GravityOff()
    {
        GetComponentInChildren<Collider>().isTrigger = true;
        StartCoroutine(DelayGravity());
    }

    IEnumerator DelayGravity()
    {
        yield return new WaitForSeconds(0.2f);
        rig.useGravity = false;
    }
}
