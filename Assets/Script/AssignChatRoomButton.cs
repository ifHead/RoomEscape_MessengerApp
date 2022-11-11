using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignChatRoomButton : MonoBehaviour
{
    Navigator navigator;
    string employeeName;
    public TextMeshProUGUI chatRoomName;
    public ChatManager chatManager;
    
    private void Start()
    {
        chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
    }

    public void goToChatRoom(){
        navigator = GameObject.Find("Navigator").GetComponent<Navigator>();
        navigator.goToChatRoom(employeeName);        
        chatManager.setupChatRoom(chatRoomName.text);
        Invoke("bringScrollToBottom", 0.05f);
    }

    void bringScrollToBottom()
    {
        chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        Canvas.ForceUpdateCanvases();
        chatManager.scrollbar.value = 0.0001f;
        Canvas.ForceUpdateCanvases();
    }
}
