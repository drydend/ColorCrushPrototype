using System;
using VContainer.Unity;

namespace ProjectCamera {
    public class CameraConfigurationController : IStartable {
        private readonly CameraConfigurationView _data;
        private readonly CameraView _camera;
        
        public CameraConfigurationController(CameraView camera, CameraConfigurationView data) {
            _camera = camera;
            _data = data;
            
        }
        
        public void Start() {
            _camera.transform.position = _data.transform.position;
            _camera.transform.rotation = _data.transform.rotation;
            _camera.mainCamera.orthographicSize = _data.size;
            _camera.uiCamera.orthographicSize = _data.size;
            _camera.overlayUICamera.orthographicSize = _data.size;
        }
    }
}