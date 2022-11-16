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
    string str = "From Unity!";
    string recvStr;
    string sendStr;
    byte[] recvData = new byte[512];
    byte[] sendData = new byte[512];
    int recvLen;
    Thread connectThread;

    void InitSocket()
    {
        ipEnd = new IPEndPoint(IPAddress.Any, 25666);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEnd);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 25666);
        clientEnd = (EndPoint)sender;
        print("waiting for UDP dgram");
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketSend(string sendStr)
    {
        sendData = new byte[1024];
        sendData = Encoding.UTF8.GetBytes(sendStr);
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
        Debug.Log("UDP Send : " + str);
    }

    void SocketReceive()
    {
        print("Entering for Receiving");
        while (true)
        {
            recvData = new byte[512];
            recvLen = socket.ReceiveFrom(recvData, ref clientEnd);
            print("message from: " + clientEnd.ToString());
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print(recvStr);
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
    }

    void Update()
    {
        Debug.Log(recvStr);
    }

    void OnApplicationQuit()
    {
        SocketQuit();
    }
}