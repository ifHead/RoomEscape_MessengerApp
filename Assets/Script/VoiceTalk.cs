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
        talkAudio.gameObject.SetActive(true);
        callAudio.gameObject.SetActive(false);
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
