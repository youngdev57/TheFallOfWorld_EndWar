using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterPooling : MonoBehaviourPun
{
    internal List<GameObject> montsers;
    private Transform pool;
    private GameObject obj;
    private int indexMonster = 0;

    public void InitMontsers(string name, int poolSize)
    {
        montsers = new List<GameObject>();
        pool = transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject goMontsers = PhotonNetwork.Instantiate(name, Vector3.zero, Quaternion.identity) as GameObject;
            PushMontsers(goMontsers);
        }
    }

    public GameObject PopMontsers(GameObject temp)
    {
        if (montsers.Count > 0)
        {
            indexMonster++;
            if (indexMonster >= montsers.Count)
            {
                indexMonster = 0;
            }
            GameObject obj = temp;
        }
        obj.SetActive(true);
        return obj;
    }

    public void PushMontsers(GameObject obj)
    {
        montsers.Add(obj);
        obj.transform.parent = pool;
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);
    }
}
