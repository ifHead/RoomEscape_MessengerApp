using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public GameObject myMessageBox, friendMessageBox, IgnoredChat;
    public Scrollbar scrollbar;
    public InputField inputField;
    public RectTransform chatRoomContent;
    public TextMeshProUGUI curChatRoomName;
    public ChatDataController chatDataController;
    private ChatData.ChatRoom curChatRoom;
    public ProfileSetter profileSetter;
    float time; 
    TouchScreenKeyboard softKeyboard;

    public string friendLastMessageTime = "";
    public string myLastMessageTime = "";
    public string previousMessageOwner = "";

    void Start()
    {
        
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            time += Time.deltaTime;
        }
        else if (time > Time.deltaTime * 100)
        {
            sendButton();
        }
    }

    public void chat(string sender, string text)
    {
        if (text.Trim() =="") return; // 스페이스나 개행만 입력했을 때는 메시지가 전송되지 않음1
        scrollbar.value = 0.00001f;
    }

    public void sendButton()
    {
        if(inputField.text.Trim() != "")
        {
            chat("Me", inputField.text);

            //말풍선 생성 후 메시지 내용 설정
            GameObject messageBox = Instantiate(myMessageBox);
            messageBox.GetComponent<AreaScript>().userText.text = inputField.text;
            
            //시간 텍스트 설정
            AreaScript areaScript = messageBox.GetComponent<AreaScript>();
            areaScript.timeText.text = DateTime.Now.ToString("tt") == "AM" ? "오전 " : "오후 ";
            areaScript.timeText.text = areaScript.timeText.text + DateTime.Now.ToString("hh:mm");
            
            if(myLastMessageTime != "" && areaScript.timeText.text == myLastMessageTime)
            {
                areaScript.timeText.gameObject.SetActive(false);
            }
            
            myLastMessageTime = areaScript.timeText.text;
            
            //말풍선을 채팅창 vertical layout에 추가
            messageBox.transform.SetParent(chatRoomContent.transform);
            inputField.text ="";

            Canvas.ForceUpdateCanvases();

            if(areaScript.textRect.sizeDelta.x > 536)
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
            setChatBalloon(chatElem);
        }
    }

    public void setChatBalloon(List<string> _chatElem)
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
}