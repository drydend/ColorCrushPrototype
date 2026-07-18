using System;
using Cubes.Models;
using Cubes.Views;
using Levels.Views;
using R3;
using UnityEngine;

namespace Cubes.Controllers {
    public class CubeController : IDisposable {
        private readonly LevelCubesGridView _gridView;
        private readonly CubeView _view;
        private readonly CubeModel _model;
        
        private readonly CompositeDisposable _disposables;
        
        public CubeController(CubeModel model, CubeView view, LevelCubesGridView gridView) {
            _view = view;
            _gridView = gridView;
            _model = model;
            _disposables = new CompositeDisposable();
        }
        
        public void Dispose() => _disposables.Dispose();
        
        public void MoveCube(float delta) {
            if (_model.currentHealth.Value <= 0) {
                return;
            }
            
            Vector3 targetPosition = _gridView.GetTargetPositionForCubeOnGrid(_model.gridPosition);
            _view.transform.position = Vector3.MoveTowards(_view.transform.position, targetPosition, delta);
            _model.worldPosition = _view.transform.position;
            _model.distanceToTargetLayer = targetPosition.y - _view.transform.position.y;
        }
    }
}