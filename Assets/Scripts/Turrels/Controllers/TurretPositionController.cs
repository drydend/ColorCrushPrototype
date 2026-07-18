using Turrels.Models;
using Turrels.Views;
using UnityEngine;

namespace Turrels.Controllers {
    public class TurretPositionController {
        private readonly TurretModel _model;
        private readonly TurretView _view;
        private readonly TurretsGridView _turretsGrid;
        
        public TurretPositionController(TurretModel model, TurretView view, TurretsGridView turretsGrid) {
            _model = model;
            _view = view;
            _turretsGrid = turretsGrid;
        }
        
        public void Tick(float moveDelta) {
            if (_model.state.Value != TurretModel.State.Inactive) {
                return;
            }
            
            Vector3 targetPosition = _turretsGrid.CalculateTurretPosition(_model.gridPosition);
            
            float distance = Vector3.Distance(_view.transform.position, targetPosition);
            
            if (distance > 0.05) {
                Vector3 position = Vector3.MoveTowards(_view.transform.position, targetPosition, moveDelta);
                _view.transform.position = position;
            } else {
                _view.transform.position = targetPosition;
                
                if (_model.gridPosition.y == 0) {
                    _model.state.Value = TurretModel.State.Available;
                }
            }
        }
    }
}