using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPClient : MonoBehaviour
{
    public string recvStr;
    private string UDPClientIP;
    string str = "From Unity!";
    Socket socket;
    EndPoint serverEnd;
    IPEndPoint ipEnd;

    byte[] recvData = new byte[1024];
    byte[] sendData = new byte[1024];
    int recvLen = 0;
    Thread connectThread;

    void Start()
    {
        UDPClientIP = "192.168.0.25";
        UDPClientIP = UDPClientIP.Trim();
        InitSocket();
    }

    void InitSocket()
    {
        ipEnd = new IPEndPoint(IPAddress.Parse(UDPClientIP), 25666);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        serverEnd = (EndPoint)sender;
        SocketSend(str);
        
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
        recvData = new byte[1024];
        try
        {
            recvLen = socket.ReceiveFrom(recvData, ref serverEnd);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        if (recvLen > 0)
        {
            recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            Debug.Log(recvStr);
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
    }
    
    void OnApplicationQuit()
    {
        SocketQuit();
    }
    
    private void Update()
    {
        // SocketReceive();
    }
}