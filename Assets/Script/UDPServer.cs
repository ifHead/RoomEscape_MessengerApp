using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// TCPIP의 클라이언트는 서버가 가동되는 장치의 IP를 알아야 함

public class UDPServer : MonoBehaviour
{
    Socket socket;
    EndPoint clientEnd;
    IPEndPoint ipEnd;
    string recvStr;
    string sendStr;
    byte[] recvData = new byte[512];
    byte[] sendData = new byte[512];
    int recvLen;
    Thread connectThread;
    public int listeningPort;

    public string remoteIp = "192.168.0.255";

    public static int remoteRqst = 0;
    public static int remoteSupportMouseDownCnt = 0;
    public static int remoteSupportKeyboardDownCnt = 0;
    public static bool isRemoteKeyboardChanged = false;
    public static bool isRemoteMouseChanged = true;

    string tmpSendStr;

    void InitSocket()
    {
        ipEnd = new IPEndPoint(IPAddress.Any, listeningPort);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEnd);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, listeningPort);
        clientEnd = (EndPoint)sender;
        print("waiting for UDP dgram");
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketSend(string sendStr)
    {
        ipEnd = new IPEndPoint(IPAddress.Parse(remoteIp), listeningPort);
        sendData = new byte[1024];
        sendData = Encoding.UTF8.GetBytes(sendStr);
        try
        {
            socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
        }
        catch
        {
            //Invoke에 전달하기 위해 복사
            //될때까지 2초마다 재귀로 반복
            tmpSendStr = sendStr;
            Invoke("SocketSendDelay",2f);
        }
    }

    void SocketSendDelay()
    {
        SocketSend(tmpSendStr);
    }

    void SocketReceive()
    {
        print("Entering for Receiving");
        while (true)
        {
            recvData = new byte[512];
            recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            if(recvStr.Contains("#remotespt,run"))
            {
                ChatManager.isRemoteSupportOnline = true;
                print("message from: " + clientEnd.ToString());
                print(recvStr);
            }

            isRemoteKeyboardChanged = false;
            isRemoteMouseChanged = false;

            if(recvStr.Contains("mouse"))
            {
                remoteSupportMouseDownCnt++;
                isRemoteKeyboardChanged = false;
                isRemoteMouseChanged = true;
            }

            if(recvStr.Contains("keyboard"))
            {
                remoteSupportKeyboardDownCnt++;
                isRemoteKeyboardChanged = true;
                isRemoteMouseChanged = false;
            }
        }
    }

    void SocketQuit()
    {
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        if (socket != null)
            socket.Close();
        print("disconnect");
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        InitSocket();
        StartCoroutine(broadcastAppState());
    }

    IEnumerator broadcastAppState()
    {
        while(true)
        {
            SocketSend($"#messenger,remoteRqst:{remoteRqst},first:{PlayerPrefs.GetInt("isFirstQuizSolved")},second:{PlayerPrefs.GetInt("isSecondQuizSolved")},");
            yield return new WaitForSeconds(10);
        }
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}