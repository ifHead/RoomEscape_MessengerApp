using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignChatRoomButton : MonoBehaviour
{
    Navigator navigator;
    string employeeName;

    public void goToChatRoom(){
        employeeName = transform.parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        navigator = GameObject.Find("Navigator").GetComponent<Navigator>();
        navigator.goToChatRoom(employeeName);        
    }
}
