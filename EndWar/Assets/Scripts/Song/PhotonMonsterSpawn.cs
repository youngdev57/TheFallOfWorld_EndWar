﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonMonsterSpawn : MonoBehaviourPun
{
    public List<GameObject> nomalMonster;

    private MonsterPooling m_pool;

    private void Awake()
    {
        m_pool = GetComponent<MonsterPooling>();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        m_pool.InitMontsers(nomalMonster[0].name,2);
        MonsterPos(m_pool.montsers);
    }

    public void MonsterPos(List<GameObject> monster)
    {
        for (int x = 0; x < monster.Count; x++)
        {
            switch (x)
            {
                case 0:
                    monster[0].transform.position = new Vector3(0, 0, 5);
                    break;
                case 1:
                    monster[1].transform.position = new Vector3(10, 0, 15);
                    break;
            }
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        MonsterCheck(m_pool.montsers);
    }

    public void MonsterCheck(List<GameObject> obj)
    {
        for (int x = 0; x < obj.Count; x++)
        {
            if (obj[x].active == false)
            {
                StartCoroutine(MonsterSpawn(obj[x]));
            }
        }
    }

    IEnumerator MonsterSpawn(GameObject obj)
    {
        yield return new WaitForSeconds(10f);
        obj.SetActive(true);
    }
}   
