using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlameThrower : MonoBehaviour
{
    public GameObject flameEffect;
    internal PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            pv.RPC("Blast", RpcTarget.All);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Debug.Log("불 발사");
            pv.RPC("Blast", RpcTarget.All);
        }
    }

    [PunRPC]
    void Blast()
    {
        flameEffect.SetActive(true);

        StartCoroutine(WaitFlame());
    }

    IEnumerator WaitFlame()
    {
        yield return new WaitForSeconds(2f);

        pv.RPC("BlastOff", RpcTarget.All);
    }

    [PunRPC]
    void BlastOff()
    {
        flameEffect.SetActive(true);
    }
}
