using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    ChatDataController chatDataController;
    public int resetIntentionCnt = 0;

    public void Start()
    {
        chatDataController = GameObject.Find("DataManager").GetComponent<ChatDataController>();
        Debug.Log(PlayerPrefs.GetInt("isFirstQuizSolved"));
        Debug.Log(PlayerPrefs.GetInt("isSecondQuizSolved"));
        Debug.Log(PlayerPrefs.GetInt("isCallAccepted"));
        Debug.Log(PlayerPrefs.GetInt("isCallRejected"));
        Debug.Log(PlayerPrefs.GetInt("isInitRun"));
    }

    public void resetBtn()
    {
        resetIntentionCnt++;
        
        if(resetIntentionCnt > 10)
        {
            resetIntentionCnt = 0;

            PlayerPrefs.SetInt("isFirstQuizSolved", 0);
            PlayerPrefs.SetInt("isSecondQuizSolved", 0);
            PlayerPrefs.SetInt("isCallAccepted", 0);
            PlayerPrefs.SetInt("isCallRejected", 0);
            PlayerPrefs.SetInt("isInitRun", 1);

            Debug.Log(PlayerPrefs.GetInt("isFirstQuizSolved"));
            Debug.Log(PlayerPrefs.GetInt("isSecondQuizSolved"));
            Debug.Log(PlayerPrefs.GetInt("isCallAccepted"));
            Debug.Log(PlayerPrefs.GetInt("isCallRejected"));
            Debug.Log(PlayerPrefs.GetInt("isInitRun"));

            //json 오리지널 복제해오기
            if (Application.platform == RuntimePlatform.Android)
            {
                TextAsset originalJsonFile = Resources.Load<TextAsset>("Data/OriginalChatJSON");
                FileStream fileStream;
                fileStream = new FileStream(
                    Path.Combine(Application.persistentDataPath, "ChatDataJSON.json"),
                    FileMode.Create
                );

                byte[] data = Encoding.UTF8.GetBytes(originalJsonFile.text);
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();

                chatDataController.chatDataObj = chatDataController.LoadJsonFile<ChatData>("ChatDataJSON.json");
            }
            else
            {
                TextAsset originalJsonFile = Resources.Load<TextAsset>("Data/OriginalChatJSON");
                FileStream fileStream;
                fileStream = new FileStream(
                    Path.Combine(Application.dataPath, "ChatDataJSON.json"),
                    FileMode.Create
                );

                byte[] data = Encoding.UTF8.GetBytes(originalJsonFile.text);
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();

                chatDataController.chatDataObj = chatDataController.LoadJsonFile<ChatData>("ChatDataJSON.json");
            }
        }
    }
}
