using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    [AddComponentMenu("")]
    public class NetworkManagerPyCt: NetworkManager
    {
        private void Start()
        {
            
        }

        /// <summary>
        /// サーバー開始時、プレイヤーキャラクターのメッセージを登録
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}