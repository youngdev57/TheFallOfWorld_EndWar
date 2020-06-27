using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_InvalidStartOK : MonoBehaviour
{
    Button btn;
    public DungeonEnter dungeonEnter;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate () { OKBtn(); });
    }

    public void OKBtn()
    {
        dungeonEnter.HideInvalidStart();
    }
}
