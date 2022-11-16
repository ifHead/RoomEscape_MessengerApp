using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

namespace Mirror{
    public class NetworkSetup : MonoBehaviour
    {
        public Role role;
        private string localIPv4;
        public string remoteIPv4;
        public string ipCheckHint;
        public enum Role { host, client, server };
        public GameObject noWifiWarn;
        bool wifiSetupFlag = true;
        IEnumerator clientStartCoroutine;
        IEnumerator hostStartCoroutine;
        IEnumerator serverStartCoroutine;
        NetworkManager manager;

        public bool isServerOn = false;
        public bool isClientOn = false;
        public bool isHostOn = false;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            localIPv4 = getLocalIPAddress();
            noWifiWarn.SetActive(false);

            clientStartCoroutine = clientStart();
            serverStartCoroutine = serverStart();
            hostStartCoroutine = hostStart();
        }

        private void Update()
        {
            if(isWiFiConnected()) 
            {
                localIPv4 = getLocalIPAddress();
                manager.networkAddress = localIPv4;

                //자신의 역할에 맞는 연결을 시도
                if(wifiSetupFlag)
                {
                    switch(role)
                    {
                        case Role.client :
                            StartCoroutine(clientStart());
                            break;
                            
                        case Role.host : 
                            StartCoroutine(hostStart());
                            break;

                        case Role.server :
                            StartCoroutine(serverStart());
                            break;
                    }

                    wifiSetupFlag = false;
                }
            }
            else
            {
                //와이파이가 도중에 끊어지면 재 setup 기회를 얻음
                wifiSetupFlag = true;
            }
        }

        public bool isWiFiConnected()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            return true;
        }

        public int countAllIP()
        {
            IPHostEntry HostEntry = Dns.GetHostEntry((Dns.GetHostName()));
            return HostEntry.AddressList.Length;
        }

        public string[] getAllIP()
        {
            string[] strIP = null;
            int count = 0;

            IPHostEntry HostEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HostEntry.AddressList.Length > 0)
            {
                strIP = new string[HostEntry.AddressList.Length];
                foreach (IPAddress ip in HostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        strIP[count] = ip.ToString();
                        count++;
                    }
                }
            }
            return strIP;
        }

        public string getLocalIPAddress()
        {
            string[] strIP = null;
            int count = 0;

            IPHostEntry HostEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HostEntry.AddressList.Length > 0)
            {
                strIP = new string[HostEntry.AddressList.Length];
                foreach (IPAddress ip in HostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        strIP[count] = ip.ToString();
                        if(strIP[count].Contains(ipCheckHint))
                        {
                            return localIPv4 = strIP[count];
                        }
                        count++;
                    }
                }
            }
            return "0.0.0.0";
        }

        IEnumerator hostStart()
        {
            int i = 0;
            while(!(NetworkServer.active && NetworkClient.active))
            {
                isHostOn = false;
                manager.StartHost();
                // Debug.Log($"Host attempt " + i);
                i++;
                yield return new WaitForSeconds(3);
            }
            isHostOn = true;
            yield return null;
        }

        IEnumerator serverStart()
        {
            int i = 0;
            while (!NetworkServer.active)
            {
                isServerOn = false;
                manager.StartServer();
                // Debug.Log($"Server attempt " + i);
                i++;
                yield return new WaitForSeconds(3);
            }
            isServerOn = true;
            yield return null;
        }

        IEnumerator clientStart()
        {
            int i = 0;
            while (!NetworkClient.isConnected)
            {
                isClientOn = false;
                manager.networkAddress = remoteIPv4;
                manager.StartClient();
                // Debug.Log("Client attempt" + i);
                i++;
                yield return new WaitForSeconds(3);
            }
            isClientOn = true;
            yield return null;
        }
    }
}
