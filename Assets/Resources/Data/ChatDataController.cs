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

    void Start()
    {
        // string chatJsonString = ObjectToJson(chatData);
        // CreateJsonFile("ChatData.json", chatJsonString);
        chatDataObj = JsonToObject<ChatData>(
            Resources.Load<TextAsset>("Data/ChatDataJSON").ToString()
        );
    }

    //Newtonsoft : 객체를 받아와서 Json 문자열을 반환
    //string jsonData = ObjectToJson(jtc); 과 같은 형태로 사용
    string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    //Newtonsoft : Json 파일과 일치하는 클래스의 객체를 반환
    // var jtc2 = JsonToOject<JTestClass>(jsonData); 과 같은 형태로 사용
    T JsonToObject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    // Json 파일을 저장. Asset폴더에 fileName으로.
    void CreateJsonFile(string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(
            Path.Combine(Application.dataPath, fileName),
            FileMode.Create
        );

        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    //  Asset폴더에 fileName.Json 기반으로 오브젝트 생성
    T LoadJsonFile<T>(string fileName)
    {
        FileStream fileStream = new FileStream(
            Path.Combine(Application.dataPath, fileName),
            FileMode.Open
        );

        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }
}