using System;
using ProjectCamera;
using VContainer.Unity;

namespace Input {
    public class InputPanelController : IInitializable {
        private InputPanel _panel;
        private CameraView _camera;
        
        public InputPanelController(InputPanel panel, CameraView camera) {
            _panel = panel;
            _camera = camera;
        }
        
        public void Initialize() {
            _panel.Initialize(_camera);
        }
    }
}