using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.test
{
    [AddComponentMenu("")]
    public class NetworkManagerTest : NetworkManager
    {

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            testPlayerText.ResetPlayerNum();
            Debug.Log("SServer");
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            testPlayerText.ResetPlayerNum();
            Debug.Log("DServer");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("StartServer");
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            Debug.Log("StopServer");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("CClient");
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("DClient");
        }
    }
}