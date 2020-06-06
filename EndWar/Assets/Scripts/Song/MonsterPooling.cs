using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPooling : MonoBehaviour
{
    internal List<GameObject> montsers;
    private Transform pool;
    private GameObject obj;
    private int indexMonster = 0;

    public void InitMontsers(GameObject obj, int poolSize)
    {
        montsers = new List<GameObject>();
        pool = transform;
        this.obj = obj;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject goMontsers = Instantiate(obj) as GameObject;
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
            GameObject obj = montsers[indexMonster];
        }
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
