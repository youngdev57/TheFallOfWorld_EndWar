using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickedItem : MonoBehaviourPun
{
    public GameObject pickUpLine;
    public Gem gemType;

    int index;

    [PunRPC]
    public void SetPick(bool isPick)
    {
        if (isPick)
            index++;
        else
            index--;
    }

    void Update()
    {
        if (index <= 0)
            pickUpLine.SetActive(false);
        else
            pickUpLine.SetActive(true);
    }
}
