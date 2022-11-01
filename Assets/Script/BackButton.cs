using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BackButton : MonoBehaviour
{
    Navigator navigator;

    public void goToMainScreen()
    {
        navigator = GameObject.Find("Navigator").GetComponent<Navigator>();
        navigator.chatRoom.SetActive(false);
        navigator.mainScreen.SetActive(true);
    }
}