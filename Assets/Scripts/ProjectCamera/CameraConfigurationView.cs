using UnityEngine;

namespace ProjectCamera {
    public class CameraConfigurationView : MonoBehaviour {
        [field: SerializeField]
        public float size { get; private set; }
    }
}