using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPool : MonoBehaviour
{
    private static FloatingTextPool instance;
    public static FloatingTextPool GetInstance()
    {
        return instance;
    }

    public Queue<GameObject> pool;

    public GameObject fTextPrefab;

    private void Awake()
    {
        instance = this;
        pool = new Queue<GameObject>();
    }

    public GameObject GetFloatingText(Vector3 showPos)
    {
        GameObject fText;
        if (pool.Count > 0) {
            fText = pool.Dequeue();
            fText.GetComponentInChildren<ScoreText_Floating>().startPos = showPos;
            fText.SetActive(true);
            fText.GetComponentInChildren<ScoreText_Floating>().SetInit();
        }
        else
        {
            fText = Instantiate(fTextPrefab, showPos, Quaternion.Euler(0, 90f, 0));
            fText.GetComponentInChildren<ScoreText_Floating>().startPos = showPos;
            fText.GetComponentInChildren<ScoreText_Floating>().SetInit();
        }

        fText.transform.position = showPos;
        return fText;
    }

    public void Restore(GameObject fText)
    {
        pool.Enqueue(fText);
    }
}
