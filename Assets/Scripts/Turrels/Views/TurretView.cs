using System;
using DG.Tweening;
using R3;
using TMPro;
using Turrels.Models;
using UnityEngine;

namespace Turrels.Views {
    public class TurretView : MonoBehaviour {
        public TurretModel model { get; private set; }
        
        [field: SerializeField]
        public Transform bulletSpawnPosition { get; private set; }
        
        [field: SerializeField]
        public float rotationSpeed { get; private set; } = 180;
        
        [field: SerializeField]
        public Vector3 defaultRotation { get; private set; }
        
        [field: SerializeField]
        private TextMeshProUGUI _durationLeftCounter;
        
        private IDisposable _disposable;
        private Vector3 _defaultScale;
        private Tween _onShootTween;
        
        public void Initialize(TurretModel turret) {
            this.model = turret;
            _defaultScale = transform.localScale;
            UpdateCounter(turret.durationLeft.Value);
            _disposable = turret.durationLeft.Subscribe(UpdateCounter);
        }
        
        private void UpdateCounter(int value) => _durationLeftCounter.text = value.ToString();
        
        private void Dispose() {
            if (_disposable != null) {
                _disposable.Dispose();
                _disposable = null;
            }
        }
        
        private void OnDestroy() => Dispose();
        
        public void Tick() {
            float maxDegreesDelta = rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(defaultRotation), maxDegreesDelta);
        }
        
        public void ApplyDirection(Vector3 direction) {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        
        public void OnShoot() {
            if (_onShootTween != null) {
                _onShootTween.Kill();
            }
            
            transform.localScale = _defaultScale;
            _onShootTween = transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}