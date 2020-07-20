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

    public Queue<ScoreText_Floating> pool;

    public GameObject fTextPrefab;

    private void Awake()
    {
        instance = this;
        pool = new Queue<ScoreText_Floating>();
    }

    public ScoreText_Floating GetFloatingText()
    {
        ScoreText_Floating fText;
        if (pool.Count > 0)
        {
            fText = pool.Dequeue();
            return fText;
        }
        else
        {
            fText = Instantiate(fTextPrefab, new Vector3(-100f, -200f, -100f), Quaternion.identity).GetComponent<ScoreText_Floating>();
            return fText;
        }
    }

    public void Restore(ScoreText_Floating fText)
    {
        pool.Enqueue(fText);
    }
}
