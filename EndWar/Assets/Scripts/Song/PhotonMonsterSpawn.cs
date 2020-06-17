using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonMonsterSpawn : MonoBehaviourPun
{
    public List<GameObject> nomalMonster;

    // 몬스터 무리 위치
    public Vector3 m_fir, m_sec;

    public MobLocation spawnLocation;       //씬안에 있는 몹스포너의 인스펙터에서 이걸 설정해줘야함 (필드면 필드, 던전이면 던전)

    private MonsterPooling m_pool;

    private void Awake()
    {
        m_pool = GetComponent<MonsterPooling>();
        // 실제 맵 위치
        /*
        m_fir = new Vector3(751,0,2050); 
        m_sec = new Vector3(0,0,0);
        */
        // 테스트 맵 위치
        //m_fir = new Vector3(40, 0, 40);
        //m_sec = new Vector3(-40, 0, 40);
        m_fir = new Vector3(111f, 26f, 99f);
        m_sec = new Vector3(108.5f, 26f, 99f);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        m_pool.InitMontsers(nomalMonster[0].name, 5, spawnLocation);
        m_pool.InitMontsers(nomalMonster[1].name, 5, spawnLocation);
        MonsterPos(m_pool.montsers);
    }

    public void MonsterPos(List<GameObject> monster)
    {
        for (int x = 0; x < monster.Count; x++)
        {
            switch ((x+1)%5)
            {
                case 0:
                    switch (monster[x].name)
                    {
                        case "Juggernaut(Clone)":
                            monster[x].transform.position = m_fir + new Vector3(20, 0, 10);
                            break;
                        case "Insect(Clone)":
                            monster[x].transform.position = m_sec + new Vector3(20, 0, 10);
                            break;
                    }
                    break;
                case 1:
                    switch (monster[x].name)
                    {
                        case "Juggernaut(Clone)":
                            monster[x].transform.position = m_fir + new Vector3(20, 0, -10);
                            break;
                        case "Insect(Clone)":
                            monster[x].transform.position = m_sec + new Vector3(20, 0, -10);
                            break;
                    }
                    break;
                case 2:
                    switch (monster[x].name)
                    {
                        case "Juggernaut(Clone)":
                            monster[x].transform.position = m_fir + new Vector3(-20, 0, 10);
                            break;
                        case "Insect(Clone)":
                            monster[x].transform.position = m_sec + new Vector3(-20, 0, 10);
                            break;
                    }
                    break;
                case 3:
                    switch (monster[x].name)
                    {
                        case "Juggernaut(Clone)":
                            monster[x].transform.position = m_fir + new Vector3(-5, 0, -20);
                            break;
                        case "Insect(Clone)":
                            monster[x].transform.position = m_sec + new Vector3(5, 0, 20);
                            break;
                    }
                    break;
                case 4:
                    switch (monster[x].name)
                    {
                        case "Juggernaut(Clone)":
                            monster[x].transform.position = m_fir + new Vector3(0, 0, 0);
                            break;
                        case "Insect(Clone)":
                            monster[x].transform.position = m_sec + new Vector3(0, 0, 0);
                            break;
                    }
                    break;
            }
        }
    }
}   
