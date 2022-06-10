using UnityEngine;

namespace Mirror.test
{
    public class CanvasUI : MonoBehaviour
    {

        public RectTransform mainPanel;

        public Transform text;

        // static instance that can be referenced directly from Player script
        public static CanvasUI instance;

        void Awake()
        {
            instance = this;
        }
    }
}
