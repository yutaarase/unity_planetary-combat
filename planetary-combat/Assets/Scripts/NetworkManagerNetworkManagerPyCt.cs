using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class NetworkManagerPyCt: NetworkManager
    {
        NetworkManagerPyCt instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// サーバー開始時、プレイヤーキャラクターのメッセージを登録
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            //base.OnServerAddPlayer(conn);
            //string netID = conn.identity.netId.ToString();
            //Player player = conn.identity.gameObject.GetComponent<Player>();

            //GameManager.instance.RegisterPlayer(netID, player);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}