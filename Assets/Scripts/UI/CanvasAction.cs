using ProjectCamera;
using TMPro;
using UnityEngine;

namespace UI {
    public class CanvasAction : UICanvasWindowView {
        [field: SerializeField]
        private TextMeshProUGUI _levelText;
        
        public void Initialize(CameraView camera, int levelId) {
            SetCamera(camera);
            _levelText.text = $"Level {levelId + 1}";
        }
    }
}