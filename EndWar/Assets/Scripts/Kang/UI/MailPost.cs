using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using TMPro;

public class MailPost : MonoBehaviour
{
    Inventory inven;

    public List<PostContent> postList;

    public GameObject listView;
    public GameObject contentView;

    /** 리스트 뷰 구성체 **/
    public GameObject[] postPacks;
    public TextMeshProUGUI[] postPack_titles, postPack_senderName;
    public TextMeshProUGUI page_txt;
    public Button prev_btn, next_btn;


    /** 콘텐트 뷰 구성체 **/
    public Image item_image;
    public TextMeshProUGUI sender_txt, power_txt, atk_txt, def_txt;
    public Button receive_btn, back_btn;


    int viewMax = 1;
    int viewIndex = 1;

    int openItemCode = 0;


    public void SettingPostBox()
    {
        SetOff_AllPack();   //우편물 오브젝트 일단 다 끔 (초기화)

        page_txt.text = viewIndex + " / " + viewMax;    //아래에 페이지 텍스트 갱신

        int begin = ((viewIndex - 1) * postPacks.Length);   //우편물 탐색 시작점 정해줌
        int end = 0;    //우편물 탐색 끝점 변수 초기화


        if((postList.Count - begin) > 6)    //시작점과 전체 우편물 수의 차를 구함
        {
            end = begin + 5;
        }
        else
        {
            end = postList.Count;
        }

        for (int i = begin; i < end; i++)
        {
            int packIdx = i - begin;

            postPacks[packIdx].SetActive(true);     //우편물 뷰 켬
            string item_name = inven.GetNth(postList[i].item_code).Value.itemName;      //인벤의 아이템 정보를 통해 아이템 이름 가져옴
            postPack_titles[packIdx].text = item_name;      //가져온 아이템 이름을 우편물 제목으로..
            postPack_senderName[packIdx].text = postList[i].sender;     //우측 보낸 사람 이름은 postList의 sender로
        }
    }

    void ShowList()
    {
        listView.SetActive(true);
        contentView.SetActive(false);

        SettingPostBox();
    }

    void ShowContent(int postIdx)
    {
        listView.SetActive(false);
        contentView.SetActive(true);

        PostContent cont = postList[postIdx];

        item_image.sprite = inven.spriteList[cont.item_code];
        sender_txt.text = cont.sender;
        power_txt.text = "전투력 : " + inven.GetNth(cont.item_code).Value.GetPower();
        atk_txt.text = "공격력 + " + inven.GetNth(cont.item_code).Value.GetAttackPower();
        def_txt.text = "방어력 + " + inven.GetNth(cont.item_code).Value.GetDefensePower();
    }

    public void OpenPack(string objName)
    {
        string[] div = objName.Split('^');
        ShowContent(int.Parse(div[1]));
    }

    public void ReceiveItem()
    {

    }

    public void Prev()
    {
        if (viewIndex == 1)
        {
            viewIndex = viewMax;
        }
        else
            viewIndex--;

        SettingPostBox();
    }

    public void Next()
    {
        if (viewIndex == viewMax)
        {
            viewIndex = 1;
        }
        else
            viewIndex++;

        SettingPostBox();
    }

    void SetOff_AllPack()
    {
        foreach(GameObject pack in postPacks)
        {
            pack.SetActive(false);
        }
    }

    
    public void LoadPostList(string gid)    //자신의 gid에 해당하는 메일 전부 가져오기
    {
        StartCoroutine(SelectQuery(gid));
    }

    IEnumerator SelectQuery(string gid)
    {
        WWWForm form = new WWWForm();

        form.AddField("receiver", gid);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/우편가져오는쿼리.do", form);
        
        yield return www;

        string[] list = www.text.Split('^');        //전체 우편 목록을 받아 우편마다 덩어리로 자름

        postList.Clear();       //우편 리스트 변수 비우기

        for(int i=0; i<list.Length; i++)        //받은 문자열 잘라서 PostContent 만들어 postList에 추가
        {
            string[] listFrag = list[i].Split(',');

            PostContent post = new PostContent();       //우편 객체 생성
            post.post_num = int.Parse(listFrag[0]);
            post.sender = listFrag[1];
            post.receiver = listFrag[2];
            post.item_code = int.Parse(listFrag[3]);
            post.price = int.Parse(listFrag[4]);        //값 다넣음

            postList.Add(post);     //리스트에 추가
        }
        
        viewMax = (int)Mathf.Ceil(postList.Count / (float)postPacks.Length);

        ShowList();
    }

    public void Send_DeletePost(int post_num)     //우편 첨부 아이템을 수령하면, 우편 테이블에서 해당 우편을 삭제함
    {
        StartCoroutine(DeleteQuery(post_num));
    }

    IEnumerator DeleteQuery(int post_num)
    {
        WWWForm form = new WWWForm();

        form.AddField("post_num", post_num);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/우편삭제하는쿼리.do", form);

        yield return www;

        //다시 갱신
        LoadPostList(PhotonNetwork.NickName);
    }
}
