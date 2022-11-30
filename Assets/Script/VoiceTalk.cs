using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceTalk : MonoBehaviour
{
    public GameObject callingImage;
    public GameObject acceptedImage;
    public Button acceptBtn, rejectBtn;
    public GameObject talkAudio, callAudio;
    public bool isCallAccepted, isCallRejected;

    public GameObject voiceTalkScreen;

    public AudioSource awayLaunch, launchWait_1, 
            launchWait_2, launchComplete, mouse_1, 
            mouse_2, mouse_3, keyboard_1, keyboard_2, 
            keyboard_3, devRunAway;

    bool isFirstAccept = true;

    void Start()
    {
        isCallAccepted = PlayerPrefs.GetInt("isCallAccepted") == 1 ? true : false;
        isCallRejected = PlayerPrefs.GetInt("isCallRejected") == 1 ? true : false;
        callingImage.gameObject.SetActive(false);
        acceptedImage.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        talkAudio.gameObject.SetActive(false);
        callAudio.gameObject.SetActive(false);
    }

    public void call()
    {
        callingImage.SetActive(true);
        acceptedImage.SetActive(false);
        callAudio.gameObject.SetActive(true);
        acceptBtn.gameObject.SetActive(true);
        rejectBtn.gameObject.SetActive(true);
    }

    public void accept()
    {
        acceptedImage.SetActive(true);
        callingImage.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        talkAudio.SetActive(true);
        if(isFirstAccept)
        {
            StartCoroutine(talkOnContext());
            isFirstAccept = false;
        }
        callAudio.SetActive(false);
        PlayerPrefs.SetInt("isCallAccepted", 1);
    }

    public void reject()
    {
        acceptedImage.SetActive(false);
        callingImage.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        callAudio.gameObject.SetActive(false);
        PlayerPrefs.SetInt("isCallRejected", 1);
    }

    IEnumerator talkOnContext()
    {
        bool awayLaunchFlag = true;
        bool wait_1_flag = true;
        bool wait_2_flag = true;
        bool devFlag = true;
        bool isRemoteSupportOnlineFlag = true;
        float timer = 0;
        float devRunAwayTimer = 0;

        while(true)
        {
            timer += Time.unscaledDeltaTime;
            if(awayLaunchFlag)
            {
                awayLaunch.gameObject.SetActive(true);
                awayLaunchFlag = false;

                launchWait_1.gameObject.SetActive(false);
                launchWait_2.gameObject.SetActive(false);
                launchComplete.gameObject.SetActive(false);
                mouse_1.gameObject.SetActive(false);
                mouse_2.gameObject.SetActive(false);
                mouse_3.gameObject.SetActive(false);
                keyboard_1.gameObject.SetActive(false);
                keyboard_2.gameObject.SetActive(false);
                keyboard_3.gameObject.SetActive(false);
                devRunAway.gameObject.SetActive(false);
                yield return null;
            }
        
            if(timer > 75 && !ChatManager.isRemoteSupportOnline && wait_1_flag)
            {
                wait_1_flag = false;
                launchWait_1.gameObject.SetActive(true);
                yield return null;
            }
            else if(timer > 75 + 16 && !ChatManager.isRemoteSupportOnline && wait_2_flag)
            {
                wait_2_flag = false;
                launchWait_2.gameObject.SetActive(true);
                yield return null;
            }

            if(ChatManager.isRemoteSupportOnline && isRemoteSupportOnlineFlag)
            {
                isRemoteSupportOnlineFlag = false;
                launchWait_1.gameObject.SetActive(false);
                launchWait_2.gameObject.SetActive(false);
                launchComplete.gameObject.SetActive(true);
            }

            if(ChatManager.isRemoteSupportOnline)
            {
                devRunAwayTimer += Time.unscaledDeltaTime;
            }

            if(ChatManager.isRemoteSupportOnline && devRunAwayTimer > 20
                && UDPServer.isRemoteMouseChanged && devRunAwayTimer < 70)
            {
                if(UDPServer.remoteSupportMouseDownCnt == 1)
                {
                    mouse_1.gameObject.SetActive(true);
                }

                if (UDPServer.remoteSupportMouseDownCnt == 2)
                {
                    mouse_2.gameObject.SetActive(true);
                }

                if (UDPServer.remoteSupportMouseDownCnt == 3)
                {
                    mouse_3.gameObject.SetActive(true);
                }
                yield return null;
            }

            if (ChatManager.isRemoteSupportOnline && devRunAwayTimer > 20
                && UDPServer.isRemoteKeyboardChanged && devRunAwayTimer < 70)
            {
                if (UDPServer.remoteSupportKeyboardDownCnt == 1)
                {
                    keyboard_1.gameObject.SetActive(true);
                }

                if (UDPServer.remoteSupportKeyboardDownCnt == 2)
                {
                    keyboard_2.gameObject.SetActive(true);
                }

                if (UDPServer.remoteSupportKeyboardDownCnt == 3)
                {
                    keyboard_3.gameObject.SetActive(true);
                }
                yield return null;
            }

            if(devFlag && devRunAwayTimer > 80)
            {
                devRunAway.gameObject.SetActive(true);
                devFlag = false;
            }

            if(devRunAwayTimer > 95)
            {
                voiceTalkScreen.SetActive(false);
                break;
            }

            yield return null;
        }
        yield return null;
    }
}
