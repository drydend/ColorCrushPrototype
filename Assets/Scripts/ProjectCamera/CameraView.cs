using Progression;
using UnityEngine;

namespace ProjectCamera {
    public class CameraView : MonoBehaviour {
        [field: SerializeField]
        public Camera mainCamera { get; private set; }
        
        [field: SerializeField]
        public Camera uiCamera { get; private set; }
        
        [field: SerializeField]
        public Camera overlayUICamera { get; private set; }
    }
}