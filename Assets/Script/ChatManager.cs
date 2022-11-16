using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ChatManager : MonoBehaviour
{
    public GameObject myMessageBox, friendMessageBox, IgnoredChat, fileMessage;
    public Scrollbar scrollbar;
    public InputField inputField;
    public RectTransform chatRoomContent;
    public TextMeshProUGUI curChatRoomName;
    public ChatDataController chatDataController;
    private ChatData.ChatRoom curChatRoom;
    public ProfileSetter profileSetter;
    float time;
    TouchScreenKeyboard softKeyboard;
    public VoiceTalk voiceTalk;
    public GameObject networkManager;
    Mirror.NetworkSetup networkSetup;

    public bool isFriendReadMessage = false;
    public bool isFriendTyping = false;

    public string friendLastMessageTime = "";
    public string myLastMessageTime = "";
    public string previousMessageOwner = "";

    public bool isQuizSolutionRevealed = false;

    void Start()
    {
        networkSetup = networkManager.GetComponent<Mirror.NetworkSetup>();

        if(PlayerPrefs.GetInt("isInitRun") == 1)
        {
            PlayerPrefs.SetInt("isFirstQuizSolved", 0);
            PlayerPrefs.SetInt("isSecondQuizSolved", 0);
        }

        if(PlayerPrefs.GetInt("isSecondQuizSolved") == 1)
        {
            Invoke("call", 4f);
        }
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            time += Time.deltaTime;
        }
        else if (time > Time.deltaTime * 30)
        {
            sendButton();
        }
    }

    public void saveChat(string sender, string text)
    {
        // 스페이스나 개행만 입력했을 때는 메시지가 전송되지 않음
        if (text.Trim() == "") return;

        Invoke("bringScrollToBottom", 0.05f);
        List<ChatData.ChatRoom> allChatRooms = chatDataController.chatDataObj.chatRooms;

        for(int i = 0; i < allChatRooms.Count; i++)
        {
            if(allChatRooms[i].chatRoomName == curChatRoomName.text)
            {
                List<string> chatElems = new List<string>();
                chatElems.Add(sender);
                chatElems.Add(text);
                // 오른쪽처럼 하면 오류 나니 주의 : DateTime.Now.ToString("~~") + DateTime.Now.ToString("~~")
                string amOrPm = DateTime.Now.ToString("tt") == "AM" ? "오전 " : "오후 ";
                string time = DateTime.Now.ToString("hh:mm");
                chatElems.Add(amOrPm + time);
                //jsonObj에 메시지를 저장
                chatDataController.chatDataObj.chatRooms[i].chat.Add(chatElems);
                break;
            }
        }

        // 런타임 json 오브젝트를 업데이트하면서 로컬 json파일도 업데이트
        chatDataController.CreateJsonFile(
            "ChatDataJSON.json", 
            chatDataController.ObjectToJson(chatDataController.chatDataObj)
        );
        
        Invoke("bringScrollToBottom", 0.05f);
    }

    void bringScrollToBottom()
    {
        scrollbar.value = 0.0000001f;
        Canvas.ForceUpdateCanvases();
        scrollbar.value = - 0.01f;
        Canvas.ForceUpdateCanvases();
    }

    public void sendFriendMessage(string text, int delay)
    {
        StartCoroutine(waitForAnswer(text, delay));
    }

    public void sendButton()
    {
        if(inputField.text.Trim() != "")
        {
            string s = inputField.text;
            saveChat("Me", inputField.text);
            makeMessageBalloon("Me", inputField.text);

            if(curChatRoomName.text.Contains("정산"))
            {
                if(PlayerPrefs.GetInt("isFirstQuizSolved") != 1)
                {
                    if(s.Contains("오류") || s.Contains("에러") && !isQuizSolutionRevealed)
                    {
                        isQuizSolutionRevealed = true;
                        Invoke("messageReadDelay", 3);
                        sendFriendMessage("아 제가 지금 외부출장중이라...ㅠ", 6);
                        sendFriendMessage("잘 될지는 모르겠지만,, 코드 추가랑 수정을 부탁드려야 할 것 같아요ㅠ", 10);
                        sendFriendMessage("제 피씨 비번 2419치고 들어가서 바탕화면에 보시면", 15);
                        sendFriendMessage("code.c라는 파일이 있을 거예요!!!", 20);
                        sendFriendMessage("#define SOUND_FLAG_A 40\n#define SOUND_FLAG_B 20", 25);
                        sendFriendMessage("ㄴ 21, 22번째 줄에 추가해 주시구", 31);
                        sendFriendMessage("252번째 줄에 있는 sound;를\nSOUND_FLAG_A;로 바꾸고", 36);
                        sendFriendMessage("531번째 줄에도 sound;가 있는데", 42);
                        sendFriendMessage("SOUND_FLAG_B; 로 바꿔주세요!", 45);
                        sendFriendMessage("다 바꾸시고 나면", 48);
                        sendFriendMessage("바탕화면에 있는 Compiler를 실행시켜서 정상동작하는지 확인해주세요ㅜㅜㅠ", 53);
                        sendFriendMessage("아이고 결국 코드 수정까지 부탁드리게 됐네요... 이번만 부탁드려요ㅜㅠ!", 63);
                        sendFriendMessage("대소문자랑 ; 세미콜론 잘 보고 수정해주세요!!", 67);
                    }
                    Invoke("firstQuizSolveDelay", 67f);
                }
                else if(PlayerPrefs.GetInt("isSecondQuizSolved") != 1)
                {
                    if (s.Contains("오류") || s.Contains("에러")
                        || s.Contains("안돼") || s.Contains("안되")
                        || s.Contains("안 돼") || s.Contains("안 되"))
                    {
                        Invoke("messageReadDelay", 3);
                        sendFriendMessage("아 잠시만 기다려주시겠어요?", 6);
                        sendFriendMessage("자리 좀 옮겨서 보이스톡 걸게요!!", 12);
                        Invoke("call", 20f);
                    }
                    PlayerPrefs.SetInt("isSecondQuizSolved", 1);
                }

                if (s == "help")
                {
                    sendFriendMessage("log", 0);
                    sendFriendMessage("setiphint 192.168.", 0);
                }

                if(s == "log")
                {
                    sendFriendMessage("Role : " + networkSetup.role.ToString(), 0);
                    sendFriendMessage("WiFi : " + networkSetup.isWiFiConnected().ToString(), 0);
                    
                    if(networkSetup.role == Mirror.NetworkSetup.Role.host)
                    {
                        sendFriendMessage("Ready : " + (Mirror.NetworkServer.active && Mirror.NetworkClient.active).ToString(), 0);
                    }
                    else if(networkSetup.role == Mirror.NetworkSetup.Role.server)
                    {
                        sendFriendMessage("Ready : " + Mirror.NetworkServer.active.ToString(), 0);
                        sendFriendMessage("Remote IP Hint : " + networkSetup.ipCheckHint, 0);
                        sendFriendMessage("Local IP : " + networkSetup.getLocalIPAddress(), 0);
                        sendFriendMessage("All IP : ", 0);
                        string[] IPs = networkSetup.getAllIP();

                        for(int i = 0; i < networkSetup.countAllIP(); i++)
                        {
                            if(IPs[i] == null) break;
                            sendFriendMessage(i + ". " + IPs[i], 0);
                            Debug.Log(IPs[i]);
                        }                      
                    }
                    else if(networkSetup.role == Mirror.NetworkSetup.Role.client)
                    {
                        sendFriendMessage("Ready : " + Mirror.NetworkClient.isConnected.ToString(), 0);
                        sendFriendMessage("Remote Target IP : " + networkSetup.remoteIPv4.ToString(), 0);
                    }
                }

                if(s.Contains("setiphint"))
                {
                    string[] iphintarr = s.Split(' ');
                    networkSetup.ipCheckHint = iphintarr[1];
                }
            }
        }
    }

    public void messageReadDelay()
    {
        isFriendReadMessage = true;
    }

    public void call()
    {
        StartCoroutine(waitForCallAnswer());
    }

    public void firstQuizSolveDelay(){
        PlayerPrefs.SetInt("isFirstQuizSolved", 1);
        isFriendReadMessage = false;
        isFriendTyping = false;
    }

    public void makeMessageBalloon(string sender, string _text)
    {
        if(_text == "") return;

        //말풍선 생성 후 메시지 내용 설정
        GameObject messageBox = Instantiate(sender == "Me" ? myMessageBox : friendMessageBox);
        messageBox.GetComponent<AreaScript>().userText.text = _text;

        //시간 텍스트 설정
        AreaScript areaScript = messageBox.GetComponent<AreaScript>();
        areaScript.timeText.text = DateTime.Now.ToString("tt") == "AM" ? "오전 " : "오후 ";
        areaScript.timeText.text = areaScript.timeText.text + DateTime.Now.ToString("hh:mm");

        if ("" != (sender == "Me" ? myLastMessageTime : friendLastMessageTime)
        && areaScript.timeText.text == (sender == "Me" ? myLastMessageTime : friendLastMessageTime)
        && previousMessageOwner == sender)
        {
            if(sender == "Friend")
            {
                areaScript.timeText.gameObject.SetActive(false);
                areaScript.tail.SetActive(false);
                //프로필 사진, 이름 끄기
                areaScript.transform.GetChild(0).gameObject.SetActive(false);
                //프로필 사진과 이름이 빠진 만큼, 말풍선의 위치를 위로 올리기
                areaScript.GetComponent<HorizontalLayoutGroup>().padding.top = -5;
            }
            else if(sender == "Me")
            {
                areaScript.timeText.gameObject.SetActive(false);
                areaScript.tail.SetActive(false);
                //프로필 사진과 이름이 빠진 만큼, 말풍선의 위치를 위로 올리기
                areaScript.GetComponent<HorizontalLayoutGroup>().padding.top = -5;
            }
        }
        else if(sender == "Friend")
        {
            areaScript.friendName.text = curChatRoomName.text;

            //친구 프로필 색상 지정
            Image friendProfileImg = areaScript.transform.GetChild(0).GetComponent<Image>();
            string[] allNames = profileSetter.profileData.names;
            for (int i = 0; i < allNames.Length; i++)
            {
                if (allNames[i] == curChatRoomName.text)
                {
                    Transform mainPageProfile = profileSetter.profileInstances[i].transform.GetChild(0);
                    friendProfileImg.material = mainPageProfile.GetComponent<Image>().material;
                    break;
                }
            }
        }

        previousMessageOwner = sender;

        if(sender == "Me") { 
            myLastMessageTime = areaScript.timeText.text;
            inputField.text = "";
        }
        else 
        { 
            friendLastMessageTime = areaScript.timeText.text; 
        }

        //말풍선을 채팅창 vertical layout에 추가
        messageBox.transform.SetParent(chatRoomContent.transform);

        Canvas.ForceUpdateCanvases();

        if (areaScript.textRect.sizeDelta.x > 536)
        {
            Canvas.ForceUpdateCanvases();
            areaScript.balloonRect.transform.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
            Canvas.ForceUpdateCanvases();
            areaScript.textRect.sizeDelta = new Vector2(510, areaScript.textRect.sizeDelta.y);
            Canvas.ForceUpdateCanvases();
            messageBox.SetActive(false);
            messageBox.SetActive(true);
        }
    }

    public void setupChatRoom(string _reqChatRoomName)
    {
        int chatRoomContentNum = chatRoomContent.transform.childCount;

        for (int i = 0; i < chatRoomContentNum; i++)
        {
            Destroy(chatRoomContent.GetChild(i).gameObject);
        }

        curChatRoomName.text = _reqChatRoomName;

        //json 오브젝트로부터 제목이 같은 채팅방 구조체를 찾아옴
        List<ChatData.ChatRoom> allChatRooms = chatDataController.chatDataObj.chatRooms;
        for(int i = 0; i < allChatRooms.Count; i++)
        {
            if(allChatRooms[i].chatRoomName == _reqChatRoomName)
            {
                curChatRoom = allChatRooms[i];
            }
        }

        friendLastMessageTime = "";
        myLastMessageTime = "";

        for(int i = 0; i < curChatRoom.chat.Count; i++)
        {
            List<string> chatElem = curChatRoom.chat[i];
            setChatBalloons(chatElem);
        }
    }

    public void setChatBalloons(List<string> _chatElem)
    {
        GameObject messageBox;
        AreaScript areaScript;

        switch(_chatElem[0])
        {
            case "Me" :
            {
                messageBox = Instantiate(myMessageBox);
                areaScript = messageBox.GetComponent<AreaScript>();
                areaScript.userText.text = _chatElem[1];
                areaScript.timeText.text = _chatElem[2];

                areaScript.isReadIndicator.SetActive(false);

                if (myLastMessageTime != ""
                    && areaScript.timeText.text == myLastMessageTime
                    && previousMessageOwner == "Me")
                {
                    areaScript.timeText.gameObject.SetActive(false);
                    areaScript.tail.SetActive(false);
                    //프로필 사진과 이름이 빠진 만큼, 말풍선의 위치를 위로 올리기
                    areaScript.GetComponent<HorizontalLayoutGroup>().padding.top = -5;
                }
                
                myLastMessageTime = _chatElem[2];
                previousMessageOwner = "Me";
                break;
            }

            case "Friend" :
            {
                messageBox = Instantiate(friendMessageBox);
                areaScript = messageBox.GetComponent<AreaScript>();
                areaScript.userText.text = _chatElem[1];
                areaScript.timeText.text = _chatElem[2];

                if (friendLastMessageTime != "" 
                    && areaScript.timeText.text == friendLastMessageTime
                    && previousMessageOwner == "Friend")
                {
                    areaScript.timeText.gameObject.SetActive(false);
                    areaScript.tail.SetActive(false);
                    
                    //프로필 사진, 이름 끄기
                    areaScript.transform.GetChild(0).gameObject.SetActive(false);
                    //프로필 사진과 이름이 빠진 만큼, 말풍선의 위치를 위로 올리기
                    areaScript.GetComponent<HorizontalLayoutGroup>().padding.top = -5;
                }
                else
                {
                    areaScript.friendName.text = curChatRoomName.text;
                    
                    //친구 프로필 색상 지정
                    Image friendProfileImg = areaScript.transform.GetChild(0).GetComponent<Image>();
                    string[] allNames = profileSetter.profileData.names;
                    for (int i = 0; i < allNames.Length; i++)
                    {
                        if (allNames[i] == curChatRoomName.text)
                        {
                            Transform mainPageProfile = profileSetter.profileInstances[i].transform.GetChild(0);
                            friendProfileImg.material = mainPageProfile.GetComponent<Image>().material;
                            break;
                        }
                    }
                }

                friendLastMessageTime = _chatElem[2];
                previousMessageOwner = "Friend";
                break;
            }

            case "MyFile" :
            {
                messageBox = Instantiate(fileMessage);
                    messageBox.transform.SetParent(chatRoomContent.transform);
                return;
            }

            case "IgnoredChat" :
            {
                messageBox = Instantiate(IgnoredChat);
                messageBox.transform.SetParent(chatRoomContent.transform);
                return;
            }
            default :
                return;
        }

        //말풍선을 채팅창 vertical layout에 추가
        messageBox.transform.SetParent(chatRoomContent.transform);
        inputField.text = "";

        //말풍선 가로 사이즈 제한
        Canvas.ForceUpdateCanvases();
        if (areaScript.textRect.sizeDelta.x > 536)
        {
            Canvas.ForceUpdateCanvases();
            areaScript.balloonRect.transform.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
            Canvas.ForceUpdateCanvases();
            areaScript.textRect.sizeDelta = new Vector2(510, areaScript.textRect.sizeDelta.y);
            Canvas.ForceUpdateCanvases();
            messageBox.SetActive(false);
            messageBox.SetActive(true);
        }
    }

    IEnumerator waitForAnswer(string text, int delay)
    {
        yield return new WaitForSeconds(delay);
        
        isFriendTyping = true;
        isFriendReadMessage = true;

        while(!curChatRoomName.text.Contains("정산"))
        {
            yield return null;
        }

        saveChat("Friend", text);
        makeMessageBalloon("Friend", text);
        Invoke("bringScrollToBottom", 0.05f);
        StartCoroutine(leaveChatRoom());
    }

    IEnumerator leaveChatRoom()
    {
        isFriendTyping = true;
        isFriendReadMessage = true;
        yield return new WaitForSeconds(5);
        isFriendTyping = false;
        isFriendReadMessage = false;
    }

    IEnumerator waitForCallAnswer()
    {
        if(PlayerPrefs.GetInt("isCallAccepted") == 1)
        {
            yield break;
        }

        voiceTalk.call();

        while(PlayerPrefs.GetInt("isCallAccepted") == 0)
        {
            if(PlayerPrefs.GetInt("isCallRejected") == 1)
            {
                yield return new WaitForSeconds(5f);

                if (PlayerPrefs.GetInt("isCallAccepted") == 1)
                {
                    yield break;
                }

                voiceTalk.call();
            }

            yield return new WaitForSeconds(3f);

            if (PlayerPrefs.GetInt("isCallAccepted") == 1)
            {
                yield break;
            }
        }
        yield break;
    }
}
