// using System.Collections;
// using System.Collections.Generic;

// asdfafsd
// //https://wergia.tistory.com/163

// //채팅을 저장/불러오기 하기 위해 Newtonsoft Json을 사용해야 함
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;


public class ChatData
{
    public List<ChatRoom> chatRooms;
    private Profile profileData;

    public ChatData()
    {
        profileData = Resources.Load<Profile>("Data/ProfileData");
        int profileNum = profileData.names.Length;

        initChatRooms(profileNum);
    }

    public struct ChatRoom 
    {
        public string? chatRoomName {get; set;}
        public List<List<string>?>? chat {get; set;}
    }
    

    public void initChatRooms(int n)
    {
        ChatRoom emptyChatRoom = new ChatRoom();
        chatRooms = new List<ChatRoom>();

        for(int i = 0; i < n; i++)
        {
            chatRooms.Add(emptyChatRoom);
        }

        for(int i = 0; i < n; i++)
        {
            if (profileData.names[i] != "-")
            {
                emptyChatRoom.chatRoomName = profileData.names[i];
                emptyChatRoom.chat = new List<List<string>>();
                chatRooms[i] = emptyChatRoom;
            }
        }
    }

    public void addChat(int chatRoomNum, string meOrFriend, string text, string time)
    {
        chatRooms[chatRoomNum].chat.Add(new List<string> {meOrFriend, text, time}); 
    }

    public void Print()
    {
        foreach (var data in chatRooms)
        {
            Debug.Log(string.Format("ChatRoomStruct[{0}] = {1}", data.chatRoomName, data.chat));
        }
    }
}