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
        public string localIPv4;
        public string remoteIPv4;
        public enum Role { host, client, server };
        public Role role;
        public int port;
        public GameObject noWifiWarn;
        bool wifiSetupFlag = true;
        IEnumerator clientStartCoroutine;
        IEnumerator hostStartCoroutine;
        IEnumerator serverStartCoroutine;
        NetworkManager manager;

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
                if(wifiSetupFlag)
                {
                    switch(role)






























































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































                    
                    {
                        case Role.client :
                            StartCoroutine(clientStartCoroutine);
                            break;
                            
                        case Role.host : 
                            StartCoroutine(hostStartCoroutine);
                            break;

                        case Role.server :
                            StartCoroutine(serverStartCoroutine);
                            break;
                    }

                    wifiSetupFlag = false;
                }
            }
        }

        public bool isWiFiConnected()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return true;
            }
            return false;
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
                        if(strIP[count].Contains("192.168"))
                        {
                            Debug.Log(strIP[count]);
                            return localIPv4;
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
            // while(!(NetworkServer.active && NetworkClient.active))
            {
                manager.StartHost();
                manager.networkAddress = localIPv4;
                Debug.Log($"Host attempt " + i);
                i++;
                yield return new WaitForSeconds(2);
            }
            Debug.Log("Host Activated");
            yield return null;
        }

        IEnumerator serverStart()
        {
            int i = 0;
            while (!NetworkServer.active)
            {
                manager.StartServer();
                manager.networkAddress = localIPv4;
                Debug.Log($"Server attempt " + i);
                i++;
                yield return new WaitForSeconds(2);
            }
            Debug.Log("Server Activated");
            yield return null;
        }

        IEnumerator clientStart()
        {
            int i = 0;
            while (!NetworkClient.isConnected)
            {
                manager.networkAddress = remoteIPv4;
                manager.StartClient();
                Debug.Log("Client attempt" + i);
                i++;
                yield return new WaitForSeconds(2);
            }
            Debug.Log("Client Activated");
            yield return null;
        }
    }
}