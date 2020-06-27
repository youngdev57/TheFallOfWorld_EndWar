using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawn : MonoBehaviour
{
    private GameObject monster;

    void Start()
    {
        monster = transform.GetChild(0).gameObject;
    }
    
    void Update()
    {
        if (!monster.activeSelf)   //게임오브젝트가 꺼져있을 때
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10f);
        monster.SetActive(true);
        // 해당 몬스터가 던전 몬스터 일 때
        if (monster.GetComponent<Monster>().location == MobLocation.Dungeon)
        {
            this.enabled = false;
        }
    }

    private void OnEnable()
    {
        this.enabled = true;
    }
}
