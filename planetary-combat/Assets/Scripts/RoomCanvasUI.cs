using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.PlanetaryCombat
{
    public class RoomCanvasUI : MonoBehaviour
    {
        public RectTransform mainPanel;

        public RectTransform playersPanel;

        // static instance that can be referenced directly from Player script
        public static RoomCanvasUI instance;

        void Awake()
        {
            instance = this;
        }
    }
}