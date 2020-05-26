using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneTransform : MonoBehaviour
{
    public Transform wrist;

    List<Transform> boneTr;

    void Start()
    {
        boneTr = new List<Transform>();
        boneTr.Add(wrist);
        Init();
    }

    void Init()
    {
        for (int i = 0; i < wrist.childCount; i++)
        {
            Transform temp = wrist.GetChild(i);

            for (int j = 0; true; j++)
            {
                boneTr.Add(temp);
                if (temp.childCount != 0)
                    temp = temp.GetChild(0);
                else
                    break;
            }
        }
    }

    public List<Transform> GetBone()
    {
        return boneTr;
    }
}
