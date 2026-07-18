using DG.Tweening;
using ProjectCamera;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class CanvasLoseWindow : UICanvasWindowView {
        [field: SerializeField]
        private RectTransform _body;
        
        [field: SerializeField]
        private CanvasGroup _canvasGroup;
        
        [field: SerializeField]
        private Button _restartButton;
        
        public void Initialize(CameraView camera, LevelLoader levelLoader) {
            SetCamera(camera);
            _restartButton.onClick.AddListener(levelLoader.RestartCurrentLevel);
        }
        
        public override void Open() {
            base.Open();
            _body.localScale = Vector3.one * 0.7f;
            _body.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.2f).SetUpdate(true);
        }
    }
}