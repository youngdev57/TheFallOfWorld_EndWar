using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonMonsterSpawn : MonoBehaviourPun
{
    public List<GameObject> nomalMonster;

    // 몬스터 무리 위치
    public Vector3 m_fir;

    private MonsterPooling m_pool;

    private void Awake()
    {
        m_pool = GetComponent<MonsterPooling>();
        m_fir = new Vector3(20,0,20);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        m_pool.InitMontsers(nomalMonster[0].name,5);
        MonsterPos(m_pool.montsers);
    }

    public void MonsterPos(List<GameObject> monster)
    {
        for (int x = 0; x < monster.Count; x++)
        {
            switch (x)
            {
                case 0:
                    monster[0].transform.position = m_fir + new Vector3(30, 0, 30);
                    break;
                case 1:
                    monster[1].transform.position = m_fir + new Vector3(20, 0, -10);
                    break;
                case 2:
                    monster[2].transform.position = m_fir + new Vector3(-20, 0, 10);
                    break;
                case 3:
                    monster[3].transform.position = m_fir + new Vector3(-5, 0, -30);
                    break;
                case 4:
                    monster[4].transform.position = m_fir + new Vector3(0, 0, 0);
                    break;
            }
        }
    }
    /*
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
            if (obj[x].activeSelf == false)
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
    */
}   
