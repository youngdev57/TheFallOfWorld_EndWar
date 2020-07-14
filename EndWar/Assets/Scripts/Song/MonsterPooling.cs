using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterPooling : MonoBehaviour
{
    internal List<GameObject> montsers = new List<GameObject>();
    private Transform pool;
    private GameObject obj;
    private int indexMonster = 0;
    private MobLocation spawnLocation;

    public void InitMontsers(GameObject obj, int poolSize, MobLocation spawnLocation)
    {
        pool = transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject goMontsers = Instantiate(obj,Vector3.zero, Quaternion.identity) as GameObject;
            PushMontsers(goMontsers);
        }

        this.spawnLocation = spawnLocation;
    }
    /*
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
    */

    public void PushMontsers(GameObject obj)
    {
        montsers.Add(obj);
        obj.transform.parent = pool;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.GetChild(0).gameObject.SetActive(false);
        obj.transform.GetChild(0).GetComponent<Monster>().location = spawnLocation;
    }
}
