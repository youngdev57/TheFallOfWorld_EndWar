using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickManager : MonoBehaviour
{
    public Transform[] flameThrowerPos;
    public Transform[] rockPos;

    void Start()
    {
        foreach(Transform tr in flameThrowerPos)
        {
            PhotonNetwork.Instantiate("FlameThrower", tr.position, tr.rotation);
        }

        foreach (Transform tr in rockPos)
        {
            PhotonNetwork.Instantiate("FallingRock", tr.position, tr.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
