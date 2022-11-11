using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Reflection;


public class ChatDataController : MonoBehaviour
{
    public ChatData chatDataObj;
    FileInfo fileInfo;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            fileInfo = new FileInfo(Path.Combine(Application.persistentDataPath, "ChatDataJSON.json"));
        }
        else
        {
            fileInfo = new FileInfo(Path.Combine(Application.dataPath, "ChatDataJSON.json"));
        }

        if (!fileInfo.Exists) 
        {
            PlayerPrefs.SetInt("isInitRun", 1);
            InitialSetup();
        }
        else
        {
            PlayerPrefs.SetInt("isInitRun", 0);
        }

        chatDataObj = LoadJsonFile<ChatData>("ChatDataJSON.json");
    }

    private void InitialSetup()
    {
        // android이면서, 이미 persistentPath에 json 파일이 있으면 복제하지 않음 (최초 실행이 아니라는 의미)
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
        }

        PlayerPrefs.SetInt("isFirstQuizSolved", 0);
        PlayerPrefs.SetInt("isSecondQuizSolved", 0);
    }

    //Newtonsoft : 객체를 받아와서 Json 문자열을 반환
    //string jsonData = ObjectToJson(jtc); 과 같은 형태로 사용
    public string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    //Newtonsoft : Json 파일과 일치하는 클래스의 객체를 반환
    // var jtc2 = JsonToOject<JTestClass>(jsonData); 과 같은 형태로 사용
    T JsonToObject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(
            jsonData, 
            new JsonSerializerSettings() 
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace 
            }
        );
    }

    // Json 파일을 저장. Asset폴더에 fileName으로.
    public void CreateJsonFile(string fileName, string jsonData)
    {
        FileStream fileStream;
        if (Application.platform == RuntimePlatform.Android)
        {
            fileStream = new FileStream(
                Path.Combine(Application.persistentDataPath, fileName),
                FileMode.Create
            );
        }
        else
        {
            fileStream = new FileStream(
                Path.Combine(Application.dataPath, fileName),
                FileMode.Create
            );
        }

        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    //  Asset폴더에 fileName.Json 기반으로 오브젝트 생성
    public T LoadJsonFile<T>(string fileName)
    {
        FileStream fileStream;
        if(Application.platform == RuntimePlatform.Android)
        {
            fileStream = new FileStream(
                Path.Combine(Application.persistentDataPath, fileName),
                FileMode.Open
            );
        }
        else
        {
            fileStream = new FileStream(
                Path.Combine(Application.dataPath, fileName),
                FileMode.Open
            );
        }

        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
    }
}