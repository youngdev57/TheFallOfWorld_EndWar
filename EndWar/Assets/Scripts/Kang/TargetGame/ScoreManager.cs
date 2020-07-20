using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;
using System.Threading;

public enum SenderType
{
    Monster_Near,
    Monster_Far,
    Ball_Hor_Near,
    Ball_Hor_Far,
    Ball_Ver_Near,
    Ball_Ver_Far,
    Monster_VeryFar,
    Ball_Hor_VeryFar
}

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager GetInstance()
    {
        return instance;
    }

    public TextMeshProUGUI score_text;
    public TextMeshProUGUI stage_text;

    [SerializeField]
    int score = 0;

    public int stage = 0;   // 0~7 STAGE3
    public int remainTarget = 0;    //현재 스테이지의 타겟 수
    public int requireSendCnt = 0;    //활성화 해야하는 타겟의 수

    public List<Target_Sender> senderList;
    public List<Target_Monster> mTargetList;

    int[] targetLevel = { 0, 1, 2, 3, 4, 5, 6, 7 };
    int[] stageEventCnt = { 5, 7, 9, 11, 12, 13, 14, 15 };


    void Awake()
    {
        instance = this;

        score_text.text = score.ToString();
    }

    void Start()
    {
        foreach (Target_Monster mTarget in mTargetList)
        {
            mTarget.StopAllCoroutines();
            mTarget.isFold = true;
            mTarget.anim.SetTrigger("Fold");
        }
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

    ////////스테이지
    
    public void Play()
    {
        StopAllCoroutines();

        remainTarget = stageEventCnt[stage];
        requireSendCnt = stageEventCnt[stage];

        StartCoroutine(StageWait());
    }

    IEnumerator StageWait()
    {
        yield return new WaitForSeconds(3f);

        StartCoroutine(GameStep());
    }

    IEnumerator GameStep()
    {
        int rnd = UnityEngine.Random.Range(7, 10);
        yield return new WaitForSeconds(rnd);

        rnd = UnityEngine.Random.Range(0, targetLevel[stage] + 1);

        switch((SenderType) rnd)
        {
            case SenderType.Monster_Near:
                mTargetList[0].Stand();
                break;

            case SenderType.Monster_Far:
                mTargetList[1].Stand();
                break;

            case SenderType.Ball_Hor_Near:
                senderList[0].SendBall();
                break;

            case SenderType.Ball_Hor_Far:
                senderList[1].SendBall();
                break;

            case SenderType.Ball_Ver_Near:
                senderList[2].SendBall();
                break;

            case SenderType.Ball_Ver_Far:
                senderList[3].SendBall();
                break;

            case SenderType.Monster_VeryFar:
                mTargetList[2].Stand();
                break;

            case SenderType.Ball_Hor_VeryFar:
                senderList[4].SendBall();
                break;
        }

        requireSendCnt--;

        if(requireSendCnt > 0)
            StartCoroutine(GameStep());
        else
        {

        }
            //다 내보냈으면 대기
    }

    public void NextStage()
    {
        stage++;
        stage_text.text = "STAGE " + stage;

        Play();
    }

    public void TargetRestore()
    {
        if(remainTarget - 1 == 0)
        {
            foreach (Target_Sender snd in senderList)
            {
                snd.PauseGame();
            }

            TargetMove[] allBalls = FindObjectsOfType<TargetMove>();

            foreach (TargetMove ball in allBalls)
            {
                if (ball.isActiveAndEnabled)
                {
                    ball.sender.Restore(ball.gameObject);
                }
            }

            foreach(Target_Monster mTarget in mTargetList)
            {
                mTarget.StopAllCoroutines();
                mTarget.isFold = true;
                mTarget.anim.SetTrigger("Fold");
            }

            if (stage == 7)
                GameEnd();
            else
                NextStage();
        } else
        {
            remainTarget--;
        }
    }

    public void GameEnd()
    {
        StopAllCoroutines();
        UnityEngine.Debug.Log("게임 끝");

        TargetMove[] allBalls = FindObjectsOfType<TargetMove>();

        foreach (TargetMove ball in allBalls)
        {
            if (ball.isActiveAndEnabled)
            {
                ball.sender.Restore(ball.gameObject);
            }
        }

        foreach (Target_Monster mTarget in mTargetList)
        {
            mTarget.StopAllCoroutines();
            mTarget.isFold = true;
            mTarget.anim.SetTrigger("Fold");
        }
    }

}
