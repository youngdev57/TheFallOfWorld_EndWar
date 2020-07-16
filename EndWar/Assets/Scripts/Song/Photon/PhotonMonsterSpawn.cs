using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhotonMonsterSpawn : MonoBehaviour
{
    public List<GameObject> nomalMonster;   //일반 몬스터

    //던전 몬스터
    public List<GameObject> dunMonster;
    public List<GameObject> bossMonster;

    // 몬스터 무리 위치
    public Vector3 m_fir, m_sec;

    public MobLocation spawnLocation;       //씬안에 있는 몹스포너의 인스펙터에서 이걸 설정해줘야함 (필드면 필드, 던전이면 던전)
    public List<Transform> spawnPos;

    private MonsterPooling m_pool;

    private void Awake()
    {
        m_pool = GetComponent<MonsterPooling>();
        // 테스트 맵 위치
        //m_fir = new Vector3(40, 0, 40);
        //m_sec = new Vector3(-40, 0, 40);
        m_fir = new Vector3(843f, 1f, 1906f);
        m_sec = new Vector3(710f, 1f, 2282f);
    }

    private void Start()
    {
        if(spawnLocation == MobLocation.Field)      //필드에 몬스터 소환할 때
        {
            m_pool.InitMontsers(nomalMonster[2], 7, MobLocation.Field);
            m_pool.InitMontsers(nomalMonster[3], 7, MobLocation.Field);
            m_pool.InitMontsers(nomalMonster[4], 7, MobLocation.Field);
            m_pool.InitMontsers(nomalMonster[0], 7, MobLocation.Field);
            m_pool.InitMontsers(nomalMonster[0], 7, MobLocation.Field);
            m_pool.InitMontsers(nomalMonster[1], 7, MobLocation.Field);
            FieldMonsterSpawn(m_pool.montsers);
        }
        else          //던전에 몬스터 소환할 때
        {
            m_pool.InitMontsers(dunMonster[0], 5, MobLocation.Dungeon);
            m_pool.InitMontsers(dunMonster[1], 5, MobLocation.Dungeon);
            m_pool.InitMontsers(bossMonster[0], 1, MobLocation.Dungeon);
            DungeonMonsterSpawn(m_pool.montsers);
        }
    }

    public void FieldMonsterSpawn(List<GameObject> monsters)      //필드 고정위치 소환..
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            //monsters[i].GetComponent<NavMeshAgent>().Warp(spawnPos[i].localPosition);
            monsters[i].transform.position = spawnPos[i].position;
        }
    }

    public void DungeonMonsterSpawn(List<GameObject> monsters)      //던전 고정위치 소환..
    {
        for(int i=0; i<monsters.Count; i++)
        {
            //monsters[i].GetComponent<NavMeshAgent>().Warp(spawnPos[i].localPosition);
            monsters[i].transform.position = spawnPos[i].position;
        }
    }
}   
