using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class ChatManager : MonoBehaviour
{
    public GameObject MyMessageBox, FriendMessageBox;
    public RectTransform contentRect;
    public Scrollbar scrollbar;
    public InputField inputField;
    public GameObject myMessageBox;
    public GameObject chatRoomContent;
    public string lastMessageTime = "";
    TouchScreenKeyboard softKeyboard;
    float time;
    
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
        if (text.Trim() =="") return; // 스페이스나 개행만 입력했을 때는 메시지가 전송되지 않음

        scrollbar.value = 0.00001f;
    }

    public void sendButton()
    {
        if(inputField.text.Trim() != "")
        {
            chat("me", inputField.text);

            //말풍선 생성 후 메시지 내용 설정
            GameObject messageBox = Instantiate(myMessageBox);
            messageBox.GetComponent<AreaScript>().userText.text = inputField.text;
            
            //시간 텍스트 설정
            AreaScript areaScript = messageBox.GetComponent<AreaScript>();
            areaScript.timeText.text = DateTime.Now.ToString("tt") == "AM" ? "오전 " : "오후 ";
            areaScript.timeText.text = areaScript.timeText.text + DateTime.Now.ToString("hh:mm");
            
            if(lastMessageTime != "" && areaScript.timeText.text == lastMessageTime)
            {
                areaScript.timeText.gameObject.SetActive(false);
            }
            
            lastMessageTime = areaScript.timeText.text;
            
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

    public void setupChatRoom()
    {
        int chatRoomContentNum = chatRoomContent.transform.childCount;

        for (int i = 0; i < chatRoomContentNum; i++)
        {
            Destroy(chatRoomContent.transform.GetChild(i));
        }

        if (inputField.text.Trim() != "")
        {
            chat("me", inputField.text);

            //말풍선 생성 후 메시지 내용 설정
            GameObject messageBox = Instantiate(myMessageBox);
            messageBox.GetComponent<AreaScript>().userText.text = inputField.text;

            //시간 텍스트 설정
            AreaScript areaScript = messageBox.GetComponent<AreaScript>();
            areaScript.timeText.text = DateTime.Now.ToString("tt") == "AM" ? "오전 " : "오후 ";
            areaScript.timeText.text = areaScript.timeText.text + DateTime.Now.ToString("hh:mm");

            if (lastMessageTime != "" && areaScript.timeText.text == lastMessageTime)
            {
                areaScript.timeText.gameObject.SetActive(false);
            }

            lastMessageTime = areaScript.timeText.text;

            //말풍선을 채팅창 vertical layout에 추가
            messageBox.transform.SetParent(chatRoomContent.transform);
            inputField.text = "";

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
}

