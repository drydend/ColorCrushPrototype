using Cubes.Models;
using DG.Tweening;
using R3;
using UnityEngine;

namespace Cubes.Views {
    public class CubeView : MonoBehaviour {
        private CubeModel _model;
        private CompositeDisposable _disposables;
        
        public void Initialize(CubeModel model) {
            _model = model;
            _disposables = new CompositeDisposable();
            
            OnHealthChanged(_model.currentHealth.Value);
            _disposables.Add(_model.currentHealth.Subscribe(OnHealthChanged));
        }
        
        public void Dispose() => _disposables.Dispose();
        
        private void OnHealthChanged(int health) {
            if (health <= 0) {
                PlayDestroyAnimation();
            }
        }
        
        private void PlayDestroyAnimation() {
            transform.DOScale(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}