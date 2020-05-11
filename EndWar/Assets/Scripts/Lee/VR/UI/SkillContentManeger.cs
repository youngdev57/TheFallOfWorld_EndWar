using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Valve.VR;
using System;

public class SkillContentManeger : MonoBehaviourPun
{
    public SteamVR_Action_Boolean choose;
    public SteamVR_Action_Boolean touchPress;
    public SteamVR_Action_Vector2 touchValue;

    [Space(5)]
    public GameObject movementObj;
    public GameObject uiCanvas;

    [Space(5)]
    public int select = 1;
    public int iconSize = 150;
    public int spacing = 30;
    public float speed = .3f;

    List<Transform> contents;
    RectTransform rectTr;
    SkillManager skillmanager;
    Button button;
    Color color;

    void Start()
    {
        skillmanager = SteamVR_Render.Top().origin.FindChild("Player").FindChild("LeftHand(Clone)").GetComponent<SkillManager>();
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        contents = new List<Transform>();
        rectTr = GetComponent<RectTransform>();

        if (choose.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            PressedDown();
        }

        if (choose.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            PressedUp();
        }

        SetSize();
    }

    void Update()
    {
        if (touchPress.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            select += touchValue.axis.y > 0 ? -1 : 1;
            if (select <= 1)
                select = 1;
            else if (select >= contents.Count-2)
                select = contents.Count - 2;
        }

        SelectedSkill();
    }

    void PressedDown()
    {
        ColorBlock n_color = button.colors;
        color = n_color.normalColor;
        n_color.normalColor = new Color(214 / 255f, 214 / 255f, 214 / 255f);
    }

    void PressedUp()
    {
        ColorBlock n_color = button.colors;
        n_color.normalColor = color;

        skillmanager.skill = contents[select].GetComponent<AddPrefabs>().GetSkill();
        movementObj.SetActive(true);
        uiCanvas.SetActive(false);
    }

    void SelectedSkill()
    {
        Vector3 obj = new Vector3(0f, contents[select].localPosition.y, 0f);
        obj.y -= contents[1].localPosition.y;

        float dixY = Mathf.Lerp(transform.localPosition.y, -obj.y, speed);

        transform.localPosition = new Vector3(0f, dixY, 0f);
    }
    void SetSize()
    {
        transform.localPosition = new Vector2(-320f, 0f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                contents.Add(transform.GetChild(i));
            Debug.Log(i);
        }

        int size = (iconSize + spacing) * contents.Count - spacing;

        rectTr.sizeDelta = new Vector2(640, size);
    }
}
