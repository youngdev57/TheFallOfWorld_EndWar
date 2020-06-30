using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossMonsterSpwanPatten : MonoBehaviour
{
    internal List<GameObject> montsers = new List<GameObject>();
    private Transform pool;
    private GameObject obj;
    private MobLocation spawnLocation;

    public void InitMontsers(string name, int poolSize, MobLocation spawnLocation)
    {
        pool = transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject goMontsers = PhotonNetwork.Instantiate(name, Vector3.zero, Quaternion.identity) as GameObject;
            PushMontsers(goMontsers);
        }
        this.spawnLocation = spawnLocation;
    }

    public void PushMontsers(GameObject obj)
    {
        montsers.Add(obj);
        transform.parent.GetComponent<MonsterRespawn>().enabled = false;
        obj.transform.parent = pool;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.GetChild(0).gameObject.SetActive(false);
        obj.transform.GetChild(0).GetComponent<Monster>().location = spawnLocation;
    }
}
