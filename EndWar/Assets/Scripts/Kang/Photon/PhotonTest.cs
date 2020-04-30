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

    public Text nameInput;
    public Button loginButton;

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
        Debug.Log("Connect To Master");
        loginButton.interactable = true;  //마스터 서버에 연결 완료되면 버튼 활성화
    }

    public override void OnJoinRandomFailed(short returnCode, string message)  //입장 실패
    {
        Debug.Log("Failed Join Room");
        //열려있는 방이 없거나 모종의 이유로 입장 불가 시 직접 방만들고 입장
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayer });
    }

    public override void OnJoinedRoom()  //입장 성공
    {
        Debug.Log("Joined Room !!!");
        LoadIceScene();  //입장 성공 시 설원맵 로드함
    }

    

    public void Login()
    {
        loginButton.interactable = false;  //로그인 버튼 여러번 누르는 것 방지
        userId = nameInput.text;
        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.JoinRandomRoom();  //성공 시 OnJoinedRoom 호출 - 실패 시 OnJoinRandomFailed 호출
    }

    void LoadIceScene()
    {
        StartCoroutine(IceSceneLoading());
    }

    IEnumerator IceSceneLoading()
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        AsyncOperation operation = SceneManager.LoadSceneAsync("SnowMountains");

        operation.allowSceneActivation = true;

        while(!operation.isDone)
        {
            //비동기 씬 로드 완료 시 까지 대기
            yield return null;
        }

        //대기 후 위치에 플레이어 생성
        pointsObj = PlayerPoints.GetInstance();
        Debug.Log(pointsObj.name);
        playerSpawnPoints = pointsObj.icePoints;
        CreatePlayer();  //생성
    }

    void CreatePlayer()  //플레이어 프리팹 생성
    {
        int idx = Random.Range(1, playerSpawnPoints.Length);
        //포톤에서 프리팹을 소환하려면 최상위 루트의 Resources 폴더 안에 프리팹을 둬야만 함
        //그리고 프리팹의 이름을 문자열로 호출하여 Instantiate 함
        PhotonNetwork.Instantiate("TestPlayer", playerSpawnPoints[idx].position, Quaternion.identity, 0);
        Debug.Log("소환됨");
    }
}
