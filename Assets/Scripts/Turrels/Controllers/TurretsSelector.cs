using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Input;
using Levels.Models;
using ProjectCamera;
using R3;
using Services.Audio.Players;
using Turrels.Models;
using Turrels.Parameters;
using Turrels.Views;
using UnityEngine;

namespace Turrels.Controllers {
    public class TurretsSelector : IDisposable {
        private readonly CameraView _camera;
        private readonly InputPanel _inputPanel;
        private readonly TurretsGridModel _grid;
        private readonly TurretsParameters _parameters;
        private CompositeDisposable _disposable;
        private CancellationTokenSource _cancellation;
        
        public TurretsSelector(InputPanel inputPanel, LevelSessionModel level, CameraView camera, TurretsParameters parameters) {
            _inputPanel = inputPanel;
            _grid = level.turretsGrid;
            _camera = camera;
            _parameters = parameters;
            _cancellation = new CancellationTokenSource();
            _disposable = new CompositeDisposable();
        }
        
        public void Activate() {
            _disposable = new CompositeDisposable();
            _disposable.Add(_inputPanel.onDown.Subscribe(OnPress));
        }
        
        public void Deactivate() {
            _disposable.Dispose();
        }
        
        public void Dispose() {
            _cancellation.Cancel();
            _cancellation.Dispose();
            _disposable.Dispose();
        }
        
        private void OnPress(Vector2 screenPos) {
            Ray ray = _camera.mainCamera.ScreenPointToRay(screenPos);
            
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider.TryGetComponent(out TurretView turret)) {
                    if (!IsAnyEmpty()) {
                        return;
                    }
                    
                    turret.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
                    
                    if (turret.model.state.Value != TurretModel.State.Available) {
                        return;
                    }
                    
                    AudioService.instance.player.Play(AudioService.instance.parameters.turretSelect, Vector3.zero);
                    MoveTurretToSpot(turret.model, turret);
                }
            }
        }
        
        private bool IsAnyEmpty() {
            for (int spotId = 0; spotId < _grid.spots.Length; spotId++) {
                if (_grid.spots[spotId].turret == null) {
                    return true;
                }
            }
            
            return false;
        }
        
        private TurretSpotModel GetFirstEmpty() {
            for (int spotId = 0; spotId < _grid.spots.Length; spotId++) {
                if (_grid.spots[spotId].turret == null) {
                    return _grid.spots[spotId];
                }
            }
            
            return null;
        }
        
        private void MoveTurretToSpot(TurretModel model, TurretView view) {
            TurretSpotModel spot = GetFirstEmpty();
            spot.turret = model;
            model.state.Value = TurretModel.State.Moved;
            _grid.grid[model.gridPosition.x, model.gridPosition.y] = null;
            
            MoveProcess(model, view, spot, _cancellation.Token).Forget();
        }
        
        private async UniTask MoveProcess(TurretModel model, TurretView view, TurretSpotModel spot, CancellationToken cancellation) {
            float moveSpeed = Vector3.Distance(view.transform.position, spot.position) / _parameters.moveSpeed;
            await view.transform.DOMove(spot.position, moveSpeed).AsyncWaitForCompletion(cancellation);
            
            model.state.Value = TurretModel.State.Active;
        }
    }
}