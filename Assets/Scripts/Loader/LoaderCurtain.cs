using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Progression;
using ProjectCamera;
using UnityEngine;
using UnityEngine.UI;

namespace Loader {
    public class LoaderCurtain : MonoBehaviour {
        [field: SerializeField]
        private Canvas _thisCanvas;
        
        [field: SerializeField]
        private Image _image;
        
        [field: SerializeField]
        private float _fadeDuration;
        
        [field: SerializeField]
        private Ease _openEase;
        
        [field: SerializeField]
        private Ease _closeEase;
        
        private bool _isOpen;
        
        public void Initialize(CameraView camera) {
            _thisCanvas.worldCamera = camera.uiCamera;
        }
        
        public void ForceOpen() {
            _isOpen = true;
            gameObject.SetActive(true);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        }
        
        public async UniTask Open(CancellationToken cancellation) {
            if (_isOpen) {
                return;
            }
            
            _isOpen = true;
            gameObject.SetActive(true);
            await _image.DOFade(1, _fadeDuration).SetEase(_openEase).SetUpdate(true).AsyncWaitForCompletion(cancellation);
        }
        
        
        public async UniTask Close(CancellationToken cancellation) {
            if (!_isOpen) {
                return;
            }
            
            _isOpen = false;
            await _image.DOFade(0, 1).SetEase(_closeEase).SetUpdate(true).AsyncWaitForCompletion(cancellation);
            gameObject.SetActive(false);
        }
    }
}