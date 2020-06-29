using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using TMPro;
using System.Runtime.CompilerServices;

public class MailPost : MonoBehaviour
{
    private static MailPost instance;
    public static MailPost GetInstance()
    {
        return instance;
    }

    internal Inventory inven;

    public List<PostContent> postList;

    public GameObject listView;
    public GameObject contentView;

    /** 리스트 뷰 구성체 **/
    public GameObject[] postPacks;
    public TextMeshProUGUI[] postPack_titles, postPack_senderName;
    public TextMeshProUGUI[] postPack_postNums;
    public TextMeshProUGUI page_txt;
    public Button prev_btn, next_btn;
    public TextMeshProUGUI postNotExist_txt;    //우편물 존재하지 않음 표시할 텍스트


    /** 콘텐트 뷰 구성체 **/
    public Image item_image;
    public TextMeshProUGUI sender_txt, power_txt, atk_txt, def_txt;
    public TextMeshProUGUI itemName_txt;
    public Button receive_btn, back_btn;

    public Sprite gold_spr;


    int viewMax = 1;
    int viewIndex = 1;

    int openItemCode = 0;
    int openPostNum = 0;
    int openGold = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OpenPack("Post_Pack^0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OpenPack("Post_Pack^1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OpenPack("Post_Pack^2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OpenPack("Post_Pack^3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OpenPack("Post_Pack^4");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OpenPack("Post_Pack^5");
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Prev();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Next();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReceiveItem();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ShowList();
        }
    }


    public void SettingPostBox()
    {
        SetOff_AllPack();   //우편물 오브젝트 일단 다 끔 (초기화)

        page_txt.text = viewIndex + " / " + viewMax;    //아래에 페이지 텍스트 갱신

        int begin = ((viewIndex - 1) * postPacks.Length);   //우편물 탐색 시작점 정해줌
        int end = 0;    //우편물 탐색 끝점 변수 초기화

        if(postList.Count == 0)
        {
            postNotExist_txt.gameObject.SetActive(true);
            Debug.Log("포스트가 없음");
            return;
        } 
        else
        {
            postNotExist_txt.gameObject.SetActive(false);
        }


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

            if(postList[i].price == 0)
            {
                string item_name = PlayerInven.allItemLists[postList[i].item_code].itemName;      //인벤의 아이템 정보를 통해 아이템 이름 가져옴
                postPack_titles[packIdx].text = item_name;      //가져온 아이템 이름을 우편물 제목으로..
            }
            else
            {
                postPack_titles[packIdx].text = "판매금 " + postList[i].price.ToString() + " 골드";      //돈이 들어있다면.. 판매금을 받는 상황임. 판매금 수령 우편으로 인식 ^^
            }
            
            postPack_senderName[packIdx].text = postList[i].sender;     //우측 보낸 사람 이름은 postList의 sender로
            postPack_postNums[packIdx].text = postList[i].post_num.ToString(); //우편 번호를 숨겨서 저장
        }
    }

    public void ShowList()
    {
        listView.SetActive(true);
        contentView.SetActive(false);

        viewIndex = 1;

        SettingPostBox();
    }

    void ShowContent(int postIdx)
    {
        listView.SetActive(false);
        contentView.SetActive(true);

        PostContent cont = postList[postIdx];

        openItemCode = cont.item_code;  //수령을 위해 아이템 코드 받아서 AddItem()함수에 사용할 예정
        openGold = cont.price;

        if(cont.item_code != -1)
        {
            item_image.sprite = inven.spriteList[cont.item_code];
            itemName_txt.text = PlayerInven.allItemLists[cont.item_code].itemName;
            power_txt.text = "전투력 : " + PlayerInven.allItemLists[cont.item_code].GetPower();
            atk_txt.text = "공격력 + " + PlayerInven.allItemLists[cont.item_code].GetAttackPower();
            def_txt.text = "방어력 + " + PlayerInven.allItemLists[cont.item_code].GetDefensePower();
        }
        else
        {
            item_image.sprite = gold_spr;
            itemName_txt.text = "판매금";
            power_txt.text = openGold + " 골드";
            atk_txt.text = "";
            def_txt.text = "";
        }
        
        sender_txt.text = cont.sender;
        
    }

    public void OpenPack(string objName)
    {
        string[] div = objName.Split('^');
        int hidden = int.Parse(postPack_postNums[int.Parse(div[1])].text);
        openPostNum = hidden;
        Debug.Log("오픈 postNum : " + openPostNum);
        ShowContent(int.Parse(div[1]) + ((viewIndex - 1) * postPacks.Length));
    }

    public void DeletePack()
    {
        Send_DeletePost(openPostNum);
    }

    public void ReceiveItem()
    {
        PlayerInven pInven = inven.pInven;

        if (openItemCode == -1)
        {
            pInven.AddGold(openGold, true);
            inven.invenGold = pInven.gold;
            inven.gold_Txt.text = inven.invenGold + " G";
            DeletePack();
            return;
        }

        if(inven.itemList.Count == 28)
        {
            //꽉 참
            Debug.Log("수령 불가");
            return;
        } else
        {
            inven.AddItem(openItemCode + 1);    //아이템 수령

            DeletePack();
            //아이템 저장
            pInven.BringAllItem();
            pInven.BringAllGem();
            pInven.SaveInven();
        }
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

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/receivePost", form);
        
        yield return www;

        Debug.Log(www.text);

        string[] list = www.text.Split('^');        //전체 우편 목록을 받아 우편마다 덩어리로 자름

        postList.Clear();       //우편 리스트 변수 비우기


        for(int i=0; i<list.Length; i++)        //받은 문자열 잘라서 PostContent 만들어 postList에 추가
        {
            string[] listFrag = list[i].Split(',');

            if(listFrag.Length <= 1)
            {

            } else
            {
                PostContent post = new PostContent();       //우편 객체 생성
                post.post_num = int.Parse(listFrag[0]);
                post.sender = listFrag[1];
                post.receiver = listFrag[2];
                post.item_code = int.Parse(listFrag[3]) - 1;
                post.price = int.Parse(listFrag[4]);        //값 다넣음

                postList.Add(post);     //리스트에 추가
            }
        }

        if (postList.Count != 0)
            viewMax = (int)Mathf.Ceil(postList.Count / (float)postPacks.Length);
        else
            viewMax = 1;

        ShowList();
    }

    public void Send_DeletePost(int post_num)     //우편 첨부 아이템을 수령하면, 우편 테이블에서 해당 우편을 삭제함
    {
        StartCoroutine(DeleteQuery(post_num));
    }

    IEnumerator DeleteQuery(int post_num)
    {
        WWWForm form = new WWWForm();

        Debug.Log("쿼리 postNum : " + post_num);

        form.AddField("post_num", post_num);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/removePost", form);

        yield return www;

        //다시 갱신
        LoadPostList(PhotonNetwork.NickName);
    }
}
