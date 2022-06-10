using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.test
{
    public class testPlayerText : NetworkBehaviour
    {
        public GameObject uI;

        Text pText;

        [SyncVar]
        int playerNum;

        static readonly List<testPlayerText> playersList = new List<testPlayerText>();


        public override void OnStartServer()
        {
            base.OnStartServer();
            
            playersList.Add(this);

        }

        // Start is called before the first frame update
        void Start()
        {
            pText.text = "none";
        }

        // Update is called once per frame
        void Update()
        {
            pText.text = playerNum.ToString();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            pText.text = "none";
            playersList.Remove(this);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            pText = CanvasUI.instance.text.GetComponent<Text>();
            
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            pText.text = "none";
        }

        [ServerCallback]
        internal static void ResetPlayerNum()
        {
            int playerNum = 0;
            foreach (var player in playersList)
            {
                player.playerNum = ++playerNum;
            }
        }
    }
}
