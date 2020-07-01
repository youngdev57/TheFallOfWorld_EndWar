using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;
using System.Text;
using VRKeys;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0.01";
    public string userId = "";
    public byte maxPlayer = 20;

    PlayerPoints pointsObj;
    public Transform[] playerSpawnPoints;
    public Transform basePoint;

    //로그인 UI
    public InputField emailInput;
    public InputField passInput;
    public Button loginButton;
    //닉네임 설정 UI
    public GameObject gidBox;
    public InputField gidInput;
    public Button gidButton;
    //닉네임 설정 UI Alert창
    public GameObject alertBox;
    public Text alertText;
    public Button alertButton;


    public int destination;

    //최초 입장인지 판단
    bool isFirstConnection = true;

    public VRKeyManager vrKeyManager;

    public K_PlayerManager KPM;

    public GameObject player;

    public enum Status
    {
        WaitLogin,
        InvaildEmail,
        InvaildPassword,
        SuccessLogin,
        WaitGid,
        InvaildGid,
        NoSpaceOrSpecialGid,
        ExistGid,
        SuccessGid
    }

    public Status status = Status.WaitLogin;


    /** 웹서버 통한 로그인 작업 **/
    IEnumerator LoginCheck(string email, string password)
    {
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("pwd", password);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/gameAccess.do", form);

        yield return www;

        StartCoroutine(WaitForLogin(www, email));
    }

    IEnumerator WaitForLogin(WWW www, string tempEmail)
    {
        yield return www;

        string[] result = www.text.Split(',');

        switch (result[0])
        {
            case "-1":
                //ShowAlert("회원 없음");
                status = Status.InvaildEmail;
                break;
            case "0":
                //ShowAlert("비밀번호 틀림");
                status = Status.InvaildPassword;
                break;
            case "1":   // GID 없음
                userId = tempEmail;
                status = Status.WaitGid;
                //ShowGid();
                break;
            case "2":   // GID 있음
                //게임 접속 진행
                userId = result[1];
                status = Status.SuccessLogin;
                //WebLogin();
                break;
            case "":
                status = Status.InvaildEmail;
                break;
        }
    }

    IEnumerator GidCheck(string gid)
    {
        WWWForm form = new WWWForm();

        form.AddField("email", userId);
        form.AddField("gid", gid);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/createGid.do", form);

        yield return www;

        StartCoroutine(WaitForGid(www));
    }

    IEnumerator WaitForGid(WWW www)
    {
        yield return www;

        string[] text = www.text.Split(',');

        if (text[0] == "1")
        {
            status = Status.ExistGid;
        }
        else
        {
            userId = text[1];
            status = Status.SuccessGid;
        }

        Debug.Log(www.text);
    }

    void ShowGid()
    {
        gidBox.SetActive(true);
    }

    void ShowAlert(string txt)
    {
        alertText.text = txt;
        alertBox.SetActive(true);
    }

    public void OnClickGidButton(string gid)
    {
        if (CheckVaild(gid))
        {
            byte[] txtArr = Encoding.UTF8.GetBytes(gid);

            if (txtArr.Length > 18 || txtArr.Length < 4)
            {
                //ShowAlert("게임 아이디가 너무 짧거나 깁니다.");
                status = Status.InvaildGid;
            }
            else
            {
                //닉네임 만들기
                StartCoroutine(GidCheck(gid));
            }

        }
        else
        {
            //ShowAlert("잘못된 게임 아이디입니다.\n특수문자, 공백 불가");
            status = Status.NoSpaceOrSpecialGid;
        }
    }

    public void OnClickAlertButton()
    {
        alertBox.SetActive(false);
    }

    public bool CheckVaild(string txt)
    {
        bool isMatch = false;
        Regex rx;

        rx = new Regex(@"^[a-zA-Z0-9가-힣]*$", RegexOptions.None);

        isMatch = (string.IsNullOrEmpty(txt)) ? false : rx.IsMatch(txt);

        return isMatch;
    }
    /** 로그인 작업 문단 끝 **/

    private void Awake()
    {
        try
        {
            loginButton.interactable = false;  //마스터 서버에 연결하기 전까진 로그인 버튼 비활성화
            DontDestroyOnLoad(this.gameObject);
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        catch (Exception e)
        {

        }
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()  //마스터 서버에 연결
    {
        Debug.Log("Connect To Master ");

        try
        {
            if (isFirstConnection)
                loginButton.interactable = true;  //마스터 서버에 연결 완료되면 버튼 활성화
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }
        catch (Exception e)
        {

        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)  //랜덤룸 입장 실패
    {
        // 미사용
    }

    public override void OnJoinRoomFailed(short returnCode, string message) //입장 실패 시 방을 만듬
    {
        switch (destination)
        {
            case 0: //기지 씬 전용 룸
                PhotonNetwork.CreateRoom("Base", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;

            case 1: //아이스맵 씬 전용 룸
                PhotonNetwork.CreateRoom("Ice", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;

            case 2: //아이스맵 씬 - 던전 앞
                PhotonNetwork.CreateRoom("Ice", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;

            case 3: //지하 던전
                PhotonNetwork.CreateRoom("Dungeon_Underground", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;

            case 98: //전투 테스트
                PhotonNetwork.CreateRoom("BattleTest", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;
            case 99: //사격연습장
                PhotonNetwork.CreateRoom("AimTest", new RoomOptions { MaxPlayers = this.maxPlayer });
                break;
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("On Left Room");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("On Joined Lobby");
        ChangeRoom(destination);
    }

    public override void OnJoinedRoom()  //입장 성공 시 씬 로드
    {
        Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.IsMessageQueueRunning = false;

        switch (destination)
        {
            case 0: //기지 씬 로드
                LoadingManager.LoadScene("Basement");
                StartCoroutine(BaseSettingWait());
                break;

            case 1: //아이스맵 씬 로드
                LoadingManager.LoadScene("SnowMountains");
                StartCoroutine(SceneSettingWait());
                break;

            case 2: //아이스맵 씬 로드 - 던전앞
                LoadingManager.LoadScene("SnowMountains");
                StartCoroutine(SceneSettingWait());
                break;

            case 3: //지하 던전 씬 로드
                LoadingManager.LoadScene("Dungeon_Underground");
                StartCoroutine(SceneSettingWait());
                break;
            case 98:
                LoadingManager.LoadScene("BattleTest");
                StartCoroutine(SceneSettingWait());
                break;
            case 99:
                LoadingManager.LoadScene("AimTest");
                StartCoroutine(SceneSettingWait());
                break;
        }
    }

    public void OnClickLogin(string email, string password)
    {
        StartCoroutine(LoginCheck(email, password));
    }

    public void WebLogin()
    {
        loginButton.interactable = false;  //로그인 버튼 여러번 누르는 것 방지
        PhotonNetwork.NickName = userId;
        Debug.Log(PhotonNetwork.NickName + " <- 이름");

        LeaveRoom();
        ChangeRoom(destination);
        isFirstConnection = false;
    }

    public void Login()
    {
        loginButton.interactable = false;  //로그인 버튼 여러번 누르는 것 방지
        userId = emailInput.text;
        PhotonNetwork.NickName = emailInput.text;

        LeaveRoom();
        ChangeRoom(destination);
        isFirstConnection = false;
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("룸에서 나감");
        }
    }

    public void ChangeRoom(int destination)
    {
        this.destination = destination;

        switch (destination)
        {
            case 0:  //기지맵 룸 입장 시도
                PhotonNetwork.JoinRoom("Base");
                break;
            case 1:  //아이스맵 룸 입장 시도
                PhotonNetwork.JoinRoom("Ice");
                break;
            case 2:  //아이스맵 룸 입장 시도 - 던전 앞
                PhotonNetwork.JoinRoom("Ice");
                break;
            case 3:  //지하 던전 룸 입장 시도
                PhotonNetwork.JoinRoom("Dungeon_Underground");
                break;
            case 98:
                PhotonNetwork.JoinRoom("BattleTest");
                break;
            case 99:
                PhotonNetwork.JoinRoom("AimTest");
                break;
        }
    }

    /** 씬 로드 부분 ------------------------------------------------------------------------------------------- **/
    

    internal IEnumerator BaseSettingWait()
    {
        yield return new WaitForSeconds(2f);
        if (LoadingManager.loadEnd)
        {
            BaseSetting();
            LoadingManager.loadEnd = false;
        }
        else
            StartCoroutine(BaseSettingWait());
    }

    public void BaseSetting()
    {
        //대기 후 위치에 플레이어 생성
        CreatePlayer(0);  //생성

        KPM.inven.OnBaseCamp();
    }

    IEnumerator SceneSettingWait()
    {
        yield return new WaitForSeconds(2f);
        if (LoadingManager.loadEnd)
        {
            yield return new WaitForSeconds(2f);
            SceneSetting();
            LoadingManager.loadEnd = false;
        }
        else
            StartCoroutine(SceneSettingWait());
    }

    public void SceneSetting()
    {
        //대기 후 위치에 플레이어 생성
        CreatePlayer(destination);  //생성
    }


    /** 아래는 생성이나 설정 함수들 **/


    //플레이어 생성

    void CreatePlayer(int destination)  //플레이어 프리팹 생성  destination (0 = 기지, 1 = 아이스맵, 2 = 사막맵 ...)
    {
        Debug.Log("플레이어 생성");

        switch (destination)
        {
            case 0:     //기지
                SpawnPlayer_Base();
                break;

            case 1:     //스노우맵
                SpawnPlayer(new Vector3(269.65f, 1.5f, 1166.47f));
                break;
            case 2:     //스노우맵 - 던전앞
                SpawnPlayer(new Vector3(3500f, 2f, 1726f));
                break;
            case 3:     //던전
                float randomZ = UnityEngine.Random.Range(-8f, 8f);
                Vector3 spawnPos = new Vector3(124.9847f, 30.566f, 99.78168f + randomZ);
                SpawnPlayer(spawnPos);
                break;
            case 98:    //배틀테스트
                SpawnPlayer(new Vector3(0.7f, 1f, 0.2f));
                break;
            case 99:    //사격연습장
                break;
        }
    }

    void SpawnPlayer_Base()
    {
        GameObject tempObj = PhotonNetwork.Instantiate("PlayerOnBase", new Vector3(107.37f, 1.21f, 174.25f), Quaternion.identity, 0);
        tempObj.GetComponent<PlayerInfo>().photonManager = this;
        player = tempObj;
        tempObj.GetComponent<PlayerManager>().photonManager = this;
        tempObj.GetComponentsInChildren<UI_Laser>()[0].enabled = true;
        tempObj.GetComponentsInChildren<UI_Laser>()[1].enabled = true;

        tempObj.GetComponentsInChildren<UI_Laser>()[0].pInven = GetComponent<PlayerInven>();
        tempObj.GetComponentsInChildren<UI_Laser>()[0].kPm = GetComponent<K_PlayerManager>();
        tempObj.GetComponentsInChildren<UI_Laser>()[1].pInven = GetComponent<PlayerInven>();
        tempObj.GetComponentsInChildren<UI_Laser>()[1].kPm = GetComponent<K_PlayerManager>();

        GameObject guns = tempObj.GetComponentInChildren<ChangeGunManager>().transform.GetChild(2).gameObject;
        guns.SetActive(false);
        tempObj.GetComponentInChildren<ChangeGunManager>().enabled = false;
    }

    void SpawnPlayer(Vector3 pos)
    {
        GameObject tempObj;
        tempObj = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity, 0);
        player = tempObj;
        tempObj.GetComponent<PlayerInfo>().photonManager = this;
        tempObj.GetComponent<PlayerManager>().photonManager = this;
        tempObj.GetComponent<PlayerItem>().pInven = GetComponent<PlayerInven>();
        tempObj.GetComponent<PlayerItem>().LoadGemsLocal();     //PlayerInven의 재료 개수를 PlayerItem에 적용하는 함수
        tempObj.GetComponentInChildren<DungeonExit>().photonManager = this;


        int zeroDetect_main = player.GetComponentInChildren<ChangeGunManager>().mainWeapon = GetComponent<PlayerInven>().mainPistol;
        int zeroDetect_sub = player.GetComponentInChildren<ChangeGunManager>().secondaryWeapon = GetComponent<PlayerInven>().subPistol;

        if(zeroDetect_main == 0 && zeroDetect_sub == 0)
        {
            GetComponent<PlayerInven>().mainPistol = 3;
        }

        player.GetComponentInChildren<ChangeGunManager>().mainWeapon = GetComponent<PlayerInven>().mainPistol;
        player.GetComponentInChildren<ChangeGunManager>().secondaryWeapon = GetComponent<PlayerInven>().subPistol;
        player.GetComponent<PhotonView>().RPC("ChangeGun", RpcTarget.AllBuffered, 0);
    }
}