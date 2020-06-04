using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;
using WebSocketSharp;
using System.Text;
using VRKeys;

public class PhotonTest : MonoBehaviourPunCallbacks
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))     //로그인 테스트용 ㅡㅡ
        {
            OnClickLogin("1", "1");
        }
    }

    /** 웹서버 통한 로그인 작업 **/
    IEnumerator LoginCheck(string email, string password) {
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("pwd", password);

        Debug.Log(email + " 메일주소");
        Debug.Log(password + " 비번");

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/gameAccess.do", form);

        yield return www;

        Debug.Log("LoginCheck : " + www.text);

        StartCoroutine(WaitForLogin(www, email));
    }

    IEnumerator WaitForLogin(WWW www, string tempEmail)
    {
        yield return www;

        string[] result = www.text.Split(',');

        Debug.Log(result[0]);
        Debug.Log(www.text + " 로그인 결과!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        switch(result[0])
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
                Debug.Log("로그인 실패");
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

        Debug.Log(www.text + " Gid 판단");

        if(text[0] == "1")
        {
            //ShowAlert("이미 존재하는 아이디입니다.");
            status = Status.ExistGid;
        } else
        {
            //ShowAlert("생성 완료!");
            //gidBox.SetActive(false);
            userId = text[1];
            status = Status.SuccessGid;
            //WebLogin();
        }
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
        if(CheckVaild(gid))
        {
            byte[] txtArr = Encoding.UTF8.GetBytes(gid);

            if(txtArr.Length > 18 || txtArr.Length < 4)
            {
                //ShowAlert("게임 아이디가 너무 짧거나 깁니다.");
                status = Status.InvaildGid;
            } 
            else
            {
                //닉네임 만들기
                StartCoroutine(GidCheck(gid));
            }

            Debug.Log("유효성 검사 완료, 바이트 수 : " + txtArr.Length);
        } else
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
        loginButton.interactable = false;  //마스터 서버에 연결하기 전까진 로그인 버튼 비활성화
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()  //마스터 서버에 연결
    {
        Debug.Log("Connect To Master ");

        if (isFirstConnection)
            loginButton.interactable = true;  //마스터 서버에 연결 완료되면 버튼 활성화
        else
        {
            PhotonNetwork.JoinLobby();
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

            case 2: //사막맵 씬 전용 룸
                PhotonNetwork.CreateRoom("Desert", new RoomOptions { MaxPlayers = this.maxPlayer });
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

        switch (destination)
        {
            case 0: //기지 씬 로드
                LoadBaseScene();
                break;

            case 1: //아이스맵 씬 로드
                LoadIceScene();
                break;

            case 2: //사막맵 씬 로드
                break;
            case 99:
                LoadAimScene();
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

        destination = 1;

        LeaveRoom();
        ChangeRoom(destination);
        isFirstConnection = false;
    }

    public void Login()
    {
        loginButton.interactable = false;  //로그인 버튼 여러번 누르는 것 방지
        userId = emailInput.text;
        PhotonNetwork.NickName = emailInput.text;

        destination = 1;

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

    void ChangeRoom(int destination)
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
            case 2:  //사막맵 룸 입장 시도
                break;
            case 99:
                PhotonNetwork.JoinRoom("AimTest");
                break;
        }
    }

    //아이스맵 씬 로드
    public void LoadIceScene()
    {
        if (destination == 1)
            StartCoroutine(IceSceneLoading());
    }

    IEnumerator IceSceneLoading()
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        AsyncOperation operation = SceneManager.LoadSceneAsync("SnowMountains");

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            //비동기 씬 로드 완료 시 까지 대기
            yield return null;
        }

        //대기 후 위치에 플레이어 생성
        pointsObj = PlayerPoints.GetInstance();
        playerSpawnPoints = pointsObj.points;
        CreatePlayer(destination);  //생성  1=아이스맵 생성용
    }

    //기지맵 씬 로드  
    public void LoadBaseScene()
    {
        if (destination == 0)
            StartCoroutine(BaseSceneLoading());
    }

    IEnumerator BaseSceneLoading()
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        AsyncOperation operation = SceneManager.LoadSceneAsync("Basement");

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            //비동기 씬 로드 완료 시 까지 대기
            yield return null;
        }

        //대기 후 위치에 플레이어 생성
        pointsObj = PlayerPoints.GetInstance();
        playerSpawnPoints = pointsObj.points;
        CreatePlayer(destination);  //생성  0=기지에 플레이어 생성용
    }

    //사격연습장 씬 로드  
    public void LoadAimScene()
    {
        if (destination == 99)
            StartCoroutine(AimSceneLoading());
    }

    IEnumerator AimSceneLoading()
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        AsyncOperation operation = SceneManager.LoadSceneAsync("AimTest");

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            //비동기 씬 로드 완료 시 까지 대기
            yield return null;
        }

        //대기 후 위치에 플레이어 생성
        pointsObj = PlayerPoints.GetInstance();
        playerSpawnPoints = pointsObj.points;
        CreatePlayer(destination);  //플레이어 생성
    }







    /** 아래는 생성이나 설정 함수들 **/


    //플레이어 생성

    void CreatePlayer(int destination)  //플레이어 프리팹 생성  destination (0 = 기지, 1 = 아이스맵, 2 = 사막맵 ...)
    {
        int idx;  //도착 위치 배열의 인덱스

        Debug.Log("플레이어 생성");

        GameObject tempObj = null;

        switch (destination)
        {
            case 0:
                idx = 0; //기지 소환 위치 하나뿐이라서 그냥 0
                tempObj = PhotonNetwork.Instantiate("Player", playerSpawnPoints[idx].position, Quaternion.identity, 0);
                tempObj.GetComponent<PlayerInfo>().photonManager = this;
                Debug.Log("기지맵에 소환됨");
                break;

            case 1:
                idx = Random.Range(1, playerSpawnPoints.Length);
                //포톤에서 프리팹을 소환하려면 최상위 루트의 Resources 폴더 안에 프리팹을 둬야만 함
                //그리고 프리팹의 이름을 문자열로 호출하여 Instantiate 함
                tempObj = PhotonNetwork.Instantiate("Player", playerSpawnPoints[idx].position, Quaternion.identity, 0);
                tempObj.GetComponent<PlayerInfo>().photonManager = this;
                Debug.Log("아이스맵에 소환됨");
                break;

            case 99:
                idx = 0; //소환 위치 하나뿐이라서 그냥 0
                tempObj = PhotonNetwork.Instantiate("Player", playerSpawnPoints[idx].position, Quaternion.identity, 0);
                tempObj.GetComponent<PlayerInfo>().photonManager = this;
                Debug.Log("사격연습장에 소환됨");
                break;
        }
    }

    public void SetDestination(int num)
    {
        if (photonView.IsMine)
        {
            destination = num;
        }
    }
}