using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Start_Dungeon : MonoBehaviour
{
    Button btn;
    public DungeonEnter dungeonEnter;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate () { Action(); });
    }

    public void Action()
    {
        dungeonEnter.EnterDungeon();
    }
}
