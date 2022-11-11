using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public static PageType pageType;
    public GameObject chatRoom;
    public GameObject mainScreen;

    public enum PageType
    {
        Home,
        ChatRoom
    }

    void Start()
    {
        pageType = PageType.Home;
        chatRoom.SetActive(false);
    }


    private void Update()
    {
        checkBackSoftKey();
    }

    private void checkBackSoftKey(){
        //안드로이드 뒤로가기 상시 체크
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (pageType)
            {
                case PageType.Home:
                    //앱 종료할지 팝업
                    Application.Quit();
                    break;
                    
                case PageType.ChatRoom:
                    Navigator.pageType = PageType.Home;
                    chatRoom.SetActive(false);
                    mainScreen.SetActive(true);
                    break;
            }
        }
    }

    public void goToChatRoom(string chatRoomName)
    {
        chatRoom.SetActive(true);
        mainScreen.SetActive(false);
        Navigator.pageType = Navigator.PageType.ChatRoom;
    }
}
