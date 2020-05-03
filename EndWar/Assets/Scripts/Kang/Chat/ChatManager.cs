using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;

public class ChatManager : MonoBehaviourPunCallbacks, IChatClientListener
{
    string userName, currentChannelName;
    ChatClient chatClient;  //채팅 클라이언트

    public GameObject chatObject;  //다른 사람들 채팅 캔버스는 안보이게 하기 위한 오브젝트
    public Text chatBox;        //채팅 내용 표시 텍스트
    public InputField input;    //채팅 입력하는 필드

    AuthenticationValues auth;  //인증 정보 (닉네임 통해서 생성함)

    void Start()
    {
        if(!photonView.IsMine)
        {
            chatObject.SetActive(false);  //내 플레이어가 아니면 캔버스를 꺼버림
        }

        chatClient = new ChatClient(this);
        Application.runInBackground = true;  //백 그라운드에서 실행

        userName = PhotonNetwork.LocalPlayer.NickName;  //유저의 닉네임

        auth = new AuthenticationValues(userName);  //유저 닉네임으로 인증 정보 생성 (채팅에 필요)

        currentChannelName = "ch1";      //채널 이름
        chatClient.ChatRegion = "asia";  //채팅 클라이언트 지역 서버
                           
                                  //Photon Chat App Id       /   Version  / 인증정보
        chatClient.Connect("feef8c13-8f07-4053-a672-4ce74ea27fb0", "0.01", auth);  //☆☆연결☆☆ (중요)

    }

    void Update()
    {
        if (!photonView.IsMine)
            return;   //본인 것이 아니면 실행 안함

        if (Input.GetKeyDown(KeyCode.Return))  //엔터키 입력 시
        {
            if (input.text != "")  //뭐라도 입력 했을 때
            {
                OnSubmit();  //입력 이벤트
            }

            if (input.isFocused == false)
                input.Select();  //인풋 필드에 마우스 커서를 눌러놓지 않았을 때 포커스를 줌
        }

        if (chatClient != null)  //채팅 클라이언트가 접속해 있을 때
        {
            chatClient.Service();   //☆☆ 채팅 서비스를 지속적으로 불러 채팅 목록을 갱신함 ☆☆ (중요)
        }
    }

    public void OnSubmit()  //입력 이벤트
    {
        Chat(input.text);  //Chat 함수에 인풋 필드 내용을 전달
        input.text = "";  //전달 했으니 인풋 필드는 비워줌
    }

    public void Chat(string text)  //채팅 함수
    {
        chatClient.PublishMessage(currentChannelName, text);  //☆☆ 공개 메시지로 현재 채팅 채널에 전달 ☆☆ (중요)
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)  //오류 떴을 때 Debug.Log에 출력하는 부분 (중요하지않음)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        //채팅 환경이 변했을 때 (미사용)
    }

    public void OnConnected()
    {
        //연결 시
        
        chatClient.Subscribe(new string[] { currentChannelName, "Channel 002" }, 10);  // ☆☆ 채널 구독 ☆☆ (중요)

        chatClient.SetOnlineStatus(ChatUserStatus.Online);

        AuthenticationValues toSystemMessageAuth = new AuthenticationValues("- System Message -");
        chatClient.AuthValues = toSystemMessageAuth;

        StringBuilder tempSb = new StringBuilder();
        tempSb.Append("반갑습니다 여러분.");

        chatClient.PublishMessage(currentChannelName, tempSb.ToString());

    }

    public void OnDisconnected()
    {
        //연결 끊겼을 시 (미사용 : 어차피 연결 끊겼을 때는 게임 껐을 때 뿐이라서..)
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //메시지를 수신했을 때
        ChatChannel channel = null;  //아래의 TryGetChannel이 channel을 반환함
        bool found = chatClient.TryGetChannel(currentChannelName, out channel); // ☆☆ 채널 얻어오기 ☆☆ (중요)
        chatBox.text = channel.ToStringMessages(); // ☆☆ 받은 채널의 채팅 내역을 받아와 텍스트 GUI에 표시함 ☆☆ (중요)
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //개인 메시지 수신 (미사용)
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //상태 변경 시 (미사용)
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //채널 입장 시 (미사용)
    }

    public void OnUnsubscribed(string[] channels)
    {
        //채널 퇴장 시 (미사용)
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //미사용
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //미사용
    }
}
