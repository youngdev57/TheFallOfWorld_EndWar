using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickedItem : MonoBehaviourPun
{
    public GameObject pickUpLine;
    public Gem gemType;

    int index;

    float pickCooltime = 10f;
    float canPickTimer = 0f;

    BoxCollider coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

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
        if(gameObject.layer == 0)
            canPickTimer += Time.deltaTime;

        if(canPickTimer >= pickCooltime)
        {
            canPickTimer = 0;
            PickOn();       //주울 수 있게
        }

        if (index <= 0)
            pickUpLine.SetActive(false);
        else
            pickUpLine.SetActive(true);
    }

    public void PickOff()      //재료를 줍지 못하게 만듬
    {
        gameObject.layer = 0;   //레이어를 11이 아닌 0으로.. (11이 주울 수 있는 레이어임)
        coll.enabled = false;

        for(int i=1; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void PickOn()       //재료를 주울 수 있게 함
    {
        gameObject.layer = 11;      //주울 수 있는 레이어
        coll.enabled = true;

        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
