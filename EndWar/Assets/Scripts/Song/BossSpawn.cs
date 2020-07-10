using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossSpawn : MonoBehaviourPun
{
    private MonsterPooling m_pool;

    public List<GameObject> monster;


    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        m_pool.InitMontsers(monster[0].name, 1, MobLocation.Dungeon);
        m_pool.InitMontsers(monster[1].name, 2, MobLocation.Dungeon);
    }
}
