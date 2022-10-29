using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public enum PageType
    {
        Home,
        ChatRoom
    }

    PageType pageType = PageType.Home;

    public void setPage(GameObject pageLayout, PageType pt)
    {
        SetActive(pageLayout, true);
        this.pageType = pt;
    }

    private void Update()
    {
        //안드로이드 뒤로가기 상시 체크
        if( Input.GetKeyDown( KeyCode.Escape ) )
        {
            switch(pageType){
                case PageType.Home :
                    //앱 종료할지 체크
                    break;
                case PageType.ChatRoom :
                    SetActive(채팅방 요소들, false);
                    SetActive(홈페이지 요소들, true);
                    this.pageType = PageType.Home;
                    break;
            }
        }
    }
}
