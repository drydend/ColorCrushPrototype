using System;
using ProjectCamera;
using VContainer.Unity;

namespace Loader {
    public class CurtainController : IStartable {
        private LoaderCurtain _curtain;
        private CameraView _camera;
        
        public CurtainController(CameraView camera, LoaderCurtain curtain) {
            _camera = camera;
            _curtain = curtain;
        }
        
        public void Start() => _curtain.Initialize(_camera);
    }
}