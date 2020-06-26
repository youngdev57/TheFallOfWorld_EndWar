using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_DungeonClear : MonoBehaviour
{
    Button btn;
    public DungeonExit dungeonExit;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate () { ClearBtn(); });
    }

    public void ClearBtn()
    {
        dungeonExit.Exit_Clear();
    }
}
