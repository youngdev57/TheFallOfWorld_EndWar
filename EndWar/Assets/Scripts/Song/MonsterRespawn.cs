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
        if (!monster.activeSelf && monster.GetComponent<Monster>().location == MobLocation.Field)   //게임오브젝트가 꺼져있고 장소가 필드일때만 리스폰
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10f);
        monster.SetActive(true);
    }
}
