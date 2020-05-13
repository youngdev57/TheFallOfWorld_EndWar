using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    private string gameVersion = "0.01";
    public string userId = "";
    public byte maxPlayer = 20;

    PlayerPoints pointsObj;
    public Transform[] playerSpawnPoints;
    public Transform basePoint;

    public Text nameInput;
    public Button loginButton;

    public int destination;

    //최초 입장인지 판단
    bool isFirstConnection = true;

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

    public void Login()
    {
        loginButton.interactable = false;  //로그인 버튼 여러번 누르는 것 방지
        userId = nameInput.text;
        PhotonNetwork.NickName = nameInput.text;

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