using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager GetInstance()
    {
        return instance;
    }

    public TextMeshProUGUI score_text;

    [SerializeField]
    int score = 0;

    void Awake()
    {
        instance = this;

        score_text.text = score.ToString();
    }

    public void AddScore(int n)
    {
        if (score + n < 0)
            score = 0;
        else
            score += n;

        score_text.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}
