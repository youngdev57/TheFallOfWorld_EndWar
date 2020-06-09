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
        if (!monster.activeSelf)
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
