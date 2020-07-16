using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecognize : MonoBehaviour
{
    GameObject obj;
    Monster monster;

    private void Start()
    {
        obj = transform.parent.gameObject;
        monster = obj.GetComponent<Monster>();
    }

    // 플레이어 인식
    public void OnTriggerEnter(Collider other)
    {
        if (!monster.canAttack && other.gameObject.tag == "Player" && !monster.notDie)
        {
            monster.target = other.gameObject.transform;
            monster.mNav.stoppingDistance = monster.moveStopDir;
            monster.mNav.speed = monster.moveSpeedRun * monster.speed;
            monster.canAttack = true;
            monster.StopAllCoroutines();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (monster.canAttack && other.gameObject.tag == "Player" && !monster.notDie)
        {
            monster.mNav.stoppingDistance = 0;
            if (!monster.STUN)
            {
                monster.monster_Staus = Monster.Staus.walk;
            }
            monster.target = monster.noneTarget;
            monster.mNav.speed = monster.moveSpeedWalk * monster.speed;
            monster.canAttack = false;
            monster.idleMode = true;
        }
    }
}
