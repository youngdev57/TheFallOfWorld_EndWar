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
    public GameObject pickUp;
    public VR_Player movementObj;
    public SkillManager skillmanager;

    [Space(5)]
    public int select = 1;
    public int iconSize = 150;
    public int spacing = 30;
    public float speed = .3f;

    List<Transform> contents;
    RectTransform rectTr;
    Button button;
    int chooseSkill = 1;

    void OnEnable()
    {
        Init();
        SetSize();
    }

    private void Init()
    {
        contents = new List<Transform>();
        rectTr = GetComponent<RectTransform>();
        skillmanager.gameObject.SetActive(false);
        select = chooseSkill;
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
            else if (select >= contents.Count - 2)
                select = contents.Count - 2;
        }

        SelectedSkill();
    }

    void PressedDown()
    {
        ColorBlock n_color = button.colors;
        n_color.normalColor = new Color(214 / 255f, 214 / 255f, 214 / 255f);
        button.colors = n_color;
    }

    void PressedUp()
    {
        ColorBlock n_color = button.colors;
        n_color.normalColor = Color.white;
        button.colors = n_color;

        chooseSkill = select;

        if (contents[select].GetComponent<AddPrefabs>())
        {
            skillmanager.skill = contents[select].GetComponent<AddPrefabs>().GetSkill();
            pickUp.SetActive(false);
        }
        else
        {
            contents[select].GetComponent<PickUpType>().OffObj();
            movementObj.enabled = true;
            return;
        }

        OnObjActive();
    }

    void OnObjActive()
    {
        skillmanager.gameObject.SetActive(true);
        movementObj.enabled = true;
        uiCanvas.SetActive(false);
    }

    void SelectedSkill()
    {
        Vector3 obj = new Vector3(0f, contents[select].localPosition.y, 0f);
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
