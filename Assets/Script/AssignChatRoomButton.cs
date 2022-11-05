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
    ChatManager chatManager;

    public void goToChatRoom(){
        navigator = GameObject.Find("Navigator").GetComponent<Navigator>();
        navigator.goToChatRoom(employeeName);        
        chatManager.setupChatRoom();
    }
}
