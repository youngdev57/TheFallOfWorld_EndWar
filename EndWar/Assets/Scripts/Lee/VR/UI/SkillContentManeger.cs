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
    public SteamVR_Action_Boolean closeUi;
    public SteamVR_Action_Boolean touchPress;
    public SteamVR_Action_Vector2 touchValue;

    [Space(5)]
    public GameObject uiCanvas;
    public GameObject movementObj;
    SkillManager skillmanager;

    [Space(5)]
    public int select = 1;
    public int iconSize = 150;
    public int spacing = 30;
    public float speed = .3f;

    List<Transform> contents;
    RectTransform rectTr;
    Button button;
    Color color;
    int chooseSkill = 1;

    void OnEnable()
    {
        Init();
        SetSize();
    }

    private void Init()
    {
        SkillManager[] _skill = FindObjectsOfType<SkillManager>();
        for (int i = 0; i < _skill.Length; i++)
        {
            if (_skill[i].GetComponent<PhotonView>().IsMine)
            {
                skillmanager = _skill[i];
                break;
            }
        }
        contents = new List<Transform>();
        rectTr = GetComponent<RectTransform>();
        skillmanager.enabled = false;
    }

    void Update()
    {
        if (closeUi.GetStateDown(SteamVR_Input_Sources.LeftHand))
            OnObjActive();

        if (choose.GetStateDown(SteamVR_Input_Sources.LeftHand))
            PressedDown();

        if (choose.GetStateUp(SteamVR_Input_Sources.LeftHand))
            PressedUp();

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
        chooseSkill = select;

        skillmanager.skill = contents[select].GetComponent<AddPrefabs>().GetSkill();
        OnObjActive();
    }

    void OnObjActive()
    {
        skillmanager.enabled = true;
        movementObj.SetActive(true);
        uiCanvas.SetActive(false);
    }

    void SelectedSkill()
    {
        Vector3 obj = new Vector3(0f, contents[chooseSkill].localPosition.y, 0f);
        button = contents[select].GetComponent<Button>();
        obj.y -= contents[1].localPosition.y;

        float dixY = Mathf.Lerp(transform.localPosition.y, -obj.y, speed);

        transform.localPosition = new Vector3(0f, dixY, 0f);
    }
    void SetSize()
    {
        transform.localPosition = new Vector2(0f, 0f);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                contents.Add(transform.GetChild(i));
        }

        int size = (iconSize + spacing) * contents.Count - spacing;

        rectTr.sizeDelta = new Vector2(640, size);
    }
}
