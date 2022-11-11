using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AreaScript : MonoBehaviour
{
    public RectTransform fullAreaRect, balloonRect, textRect;
    public GameObject tail;
    public TextMeshProUGUI timeText, userText, friendName;
    public string time, user;
    public GameObject isReadIndicator;
    ChatManager chatManager;
    IEnumerator detectReadStateCoroutine;

    public void Start()
    {
        //상대가 읽었는지 확인
        if(isReadIndicator != null)
        {
            detectReadStateCoroutine = detectReadState();
            chatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
            StartCoroutine(detectReadStateCoroutine);
        }
    }

    IEnumerator detectReadState(){
        while(true)
        {
            if(chatManager.isFriendReadMessage)
            {
                isReadIndicator.SetActive(false);
                StopCoroutine(detectReadStateCoroutine);
            }
            yield return null;
        }
    }
}
