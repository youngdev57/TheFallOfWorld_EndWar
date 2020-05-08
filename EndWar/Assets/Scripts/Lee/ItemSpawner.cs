using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] Item;
    
    void Start()
    {
        for (int i = 0; i < Item.Length; i++)
        {
            Vector3 position;
            position.x = transform.position.x + Random.Range(-50f, 50f);
            position.y = 10f;
            position.z = transform.position.z + Random.Range(-50f, 50f);
            PhotonNetwork.Instantiate(Item[i].name, position, Quaternion.identity, 0);
        }
    }

}
