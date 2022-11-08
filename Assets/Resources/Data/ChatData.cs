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
    public struct ChatRoom {
        public string? chatRoomName {get; set;}
        public List<List<string>?>? chat {get; set;}
    }

    public List<ChatRoom> chatRooms;

    private Profile profileData;
    
    public ChatData()
    {
        profileData = Resources.Load<Profile>("Data/ProfileData");
        int profileNum = profileData.names.Length;

        initChatRooms(profileNum);
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

            if(profileData.names[i] == "여석현 팀장")
            {
                addChat(i, "Me", "넵 팀장님", "오후 04:20");
                addChat(i, "IgnoredChat", "", "");
                addChat(i, "Friend", "혹시 신팀장님 전화 왔었어요?", "오전 09:40");
                addChat(i, "Me", "앗 저는 아아요. 감사합니다,,", "오전 09:42");
                addChat(i, "Friend", "혹시 신팀장님 전화 왔었어요?", "오전 09:50");
                addChat(i, "Me", "아뇨 안왔습니다!", "오전 09:50");
                addChat(i, "Friend", "네~", "오전 09:55");
                addChat(i, "Me", "팀장님 혹시 질문이 있는데", "오후 05:20");
                addChat(i, "Me", "아 자리에 가서 말씀드리겠습니다.", "오후 05:20");
                addChat(i, "MyFile", "", "오후 06:50");
                addChat(i, "Friend", "네 좋아요. 마지막 장에 명단만 작성해서 최종파일 다시 보내주세요", "오후 07:15");
                addChat(i, "Me", "넵!", "오후 07:15");
                addChat(i, "Friend", "수고했어요", "오후 07:15");
            }

            if (profileData.names[i] == "신혜윤 팀장")
            {
                addChat(i, "IgnoredChat", "", "");
                addChat(i, "Friend", "저 지금 전화 못 받는데 무슨 일로", "오후 02:50");
                addChat(i, "Me", "아 사무실로 등기가 와서 연락드렸어요. 내일 다시 오신다고 합니다.", "오후 02:50");
                addChat(i, "Friend", "아 맞다 제가 연락할게요 죄송", "오후 02:52");
                addChat(i, "Friend", "감사합니다.", "오후 02:52");
                addChat(i, "Me", "넵!", "오후 02:53");
            }

            if (profileData.names[i] == "임채범 사원")
            {
                addChat(i, "Friend", "안녕하세요! 이번에 모로가도 기획2팀에 새로 입사하게 된 임채범 사원입니다! 많이 부족하지만 선배님들 보고 많이 배워 진행하는 프로젝트에 도움이 되도록 하겠습니다! 김사합니다!!", "오전 09:10");
                addChat(i, "Friend", "앗 오타 죄송합니다. 감사합니다!", "오전 09:10");
                addChat(i, "Me", "안녕하세요 기획1팀 남민서 주임입니다. 잘 부탁드리겠습니다.", "오전 10:11");
                addChat(i, "Friend", "넵 주임님 감사합니다!!", "오전 10:11");
            }

            if (profileData.names[i] == "손영정 주임")
            {
                addChat(i, "IgnoredChat", "", "");
            }

            if (profileData.names[i] == "정산 대리")
            {
                addChat(i, "IgnoredChat", "", "");
                addChat(i, "Friend", "이상하네요. 아깐 잘 됐었는데;", "오후 10:20");
                addChat(i, "Friend", "확인해볼게요.;", "오후 10:20");
                addChat(i, "Me", "넵!", "오후 10:21");
                addChat(i, "Friend", "아 죄송해요 괄호가 열려있어서", "오후 10:50");
                addChat(i, "Friend", "이제 괜찮을거예요. 확인해보세요.", "오후 10:50");
                addChat(i, "Me", "넵 대리님 이제 잘 구동됩니다", "오후 10:57");
                addChat(i, "Me", "감사합니다.", "오후 10:57");
                addChat(i, "Friend", "넵!", "오후 10:58");
            }

            if (profileData.names[i] == "정백민 사원")
            {
                addChat(i, "Friend", "주임님 정대리님이 찾으시는데 메신저 보시면 연락주세요!", "오후 04:07");
            }

            if (profileData.names[i] == "이주은 팀장")
            {
                addChat(i, "Friend", "현장 도착했는데 혹시 어디쯤이세요? 차가 막혀서 10분 정도 늦어요.", "오전 11:09");
                addChat(i, "Friend", "지금 택시.", "오전 11:10");
                addChat(i, "Me", "넵!", "오전 11:11");
            }
        }
    }

    public void addChat(int chatRoomNum, string meOrFriend, string text, string time){
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