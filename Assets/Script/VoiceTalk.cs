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

    public AudioSource awayLaunch, launchWait_1, 
            launchWait_2, launchComplete, mouse_1, 
            mouse_2, mouse_3, keyboard_1, keyboard_2, 
            keyboard_3;

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
        awayLaunch.gameObject.SetActive(true);
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
}
