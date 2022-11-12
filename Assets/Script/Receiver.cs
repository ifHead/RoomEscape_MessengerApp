using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Receiver : MonoBehaviour
{
    private UdpClient m_Receiver;
    public int m_Port = 50001;
    public string m_ReceiveMessage;

    void Awake()
    {
        InitReceiver();
    }

    void OnApplicationQuit()
    {
        CloseReceiver();
    }

    void InitReceiver()
    {
        try
        {
            if (m_Receiver == null)
            {
                m_Receiver = new UdpClient(m_Port);
                m_Receiver.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, m_Port);

        byte[] received;
        if (m_Receiver != null)
        {
            received = m_Receiver.EndReceive(ar, ref ipEndPoint);
        }

        else
        {
            return;
        }

        m_Receiver.BeginReceive(new AsyncCallback(ReceiveCallback), null);

        m_ReceiveMessage = Encoding.Default.GetString(received);
        m_ReceiveMessage = m_ReceiveMessage.Trim();

        // 받은 값 처리 ...  
        DoReceive();
    }

    void DoReceive()
    {
        Debug.Log(m_ReceiveMessage); // 출력 : English 한글 漢字 !@#$%^&*() 
    }

    void CloseReceiver()
    {
        if (m_Receiver != null)
        {
            m_Receiver.Close();
            m_Receiver = null;
        }
    }
}