using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Sender : MonoBehaviour
{
    private UdpClient m_Sender = new UdpClient();
    public string m_ReceiverIp = "192.168.0.255";
    public int m_Port = 50001;
    public string m_SendMessage;
    private byte[] m_SendBytes;

    void Start()
    {
        InitSender();
    }

    void Update()
    {
        SetSendPacket();
        DoBeginSend(m_SendBytes);
    }

    void OnApplicationQuit()
    {
        CloseSender();
    }

    void InitSender()
    {
        m_Sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        m_Sender.Connect(IPAddress.Parse(m_ReceiverIp), m_Port);
    }

    // 보내는 값
    void SetSendPacket()
    {
        m_SendMessage = "English 한글 漢字 !@#$%^&*()";
        m_SendBytes = Encoding.UTF8.GetBytes(m_SendMessage);
    }

    void DoBeginSend(byte[] packets)
    {
        m_Sender.BeginSend(packets, packets.Length, new AsyncCallback(SendCallback), m_Sender);
    }

    void SendCallback(IAsyncResult ar)
    {
        UdpClient udpClient = (UdpClient)ar.AsyncState;
    }

    void CloseSender()
    {
        if (m_Sender != null)
        {
            m_Sender.Close();
            m_Sender = null;
        }
    }
}