using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    //싱글톤
    private static RankManager instance;

    public static RankManager GetInstance()
    {
        if (instance == null)
            instance = new RankManager();

        return instance;
    }

    //랭킹 저장용
    public JsonRank jsonRank;
    public List<Rank> sortedRank;

    void Awake()
    {
        DontDestroyOnLoad(this);

        instance = this;
        jsonRank = new JsonRank();
        jsonRank.rankList = new List<Rank>();

        LoadFromJSON();     //시작하면서 JSON 랭킹 파일 불러옴
    }

    public void WriteToJSON()//json 랭킹 파일 생성
    {
        string jsonData = JsonUtility.ToJson(jsonRank, true); //1번째 인자의 내용을 저장, 2번째 인자는 읽기 쉽게 저장할지 말지를 정함.
        string path = Application.streamingAssetsPath + "/rank.json";//저장될 주소.
        File.WriteAllText(path, jsonData);// 파일생성
    }

    public void LoadFromJSON()//json 랭킹 파일 불러오기
    {
        try    //랭킹 파일 불러오기 시도
        {
            string path = Application.streamingAssetsPath + "/rank.json";
            string jsonData = File.ReadAllText(path);
            JsonRank loadRank = JsonUtility.FromJson<JsonRank>(jsonData);

            jsonRank = loadRank;

            GameObject rankCanvas = GameObject.Find("SingleGame_Canvas");
            GameObject rankBox = rankCanvas.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;

            GameObject[] nicknames = new GameObject[6];
            GameObject[] scores = new GameObject[6];

            for (int i = 0; i < nicknames.Length; i++)
            {
                nicknames[i] = rankBox.transform.GetChild(i + 6).gameObject;
                nicknames[i].GetComponent<TextMeshProUGUI>().text = "";

                scores[i] = rankBox.transform.GetChild(i + 12).gameObject;
                scores[i].GetComponent<TextMeshProUGUI>().text = "";
            }

            SortRank(nicknames, scores);
        }
        catch (FileNotFoundException e)     //로컬에 랭킹파일이 존재하지 않는 경우
        {
            Debug.Log(e.Message);

            WriteToJSON();      //빈 랭크 파일을 생성 후
            LoadFromJSON();     //다시 불러옴
        }
    }

    void SortRank(GameObject[] nicknames, GameObject[] scores)
    {
        List<Rank> list = jsonRank.rankList;

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].score < list[i].score)
                {
                    string tempName = list[j].name;
                    list[j].name = list[i].name;
                    list[i].name = tempName;

                    int tempScore = list[j].score;
                    list[j].score = list[i].score;
                    list[i].score = tempScore;
                }
            }
        }

        sortedRank = list;

        try
        {
            for (int i = 0; i < Mathf.Clamp(sortedRank.Count, 0, 6); i++)
            {
                nicknames[i].GetComponent<TextMeshProUGUI>().text = sortedRank[i].name;  //곡 이름
                scores[i].GetComponent<TextMeshProUGUI>().text = sortedRank[i].score.ToString();  //점수
            }
        }
        catch (Exception e)
        {
        }
    }

    public void AddRank(string name, int score)     //랭킹 정보 추가
    {
        jsonRank.rankList.Add(new Rank(name, score));

        WriteToJSON();
    }
}


[Serializable]
public class JsonRank   //json으로 저장될 클래스
{
    public List<Rank> rankList;
}

[Serializable]
public class Rank
{
    public string name;
    public int score;

    public Rank(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
