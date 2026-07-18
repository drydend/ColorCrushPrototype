using System.Collections.Generic;
using DG.Tweening;
using Levels.Models;
using Turrels.Models;
using Turrels.Parameters;
using Turrels.Views;
using UnityEngine;
using VContainer.Unity;

namespace Turrels.Controllers {
    public class TurretsRemoveControllers : ITickable {
        private TurretsParameters _parameters;
        private TurretsGridModel _model;
        private TurretSpotsPlaceholderView _spotsView;
        private Dictionary<TurretModel, TurretView> _turretsMap;
        
        public TurretsRemoveControllers(LevelSessionModel session, Dictionary<TurretModel, TurretView> turretsMap, TurretsParameters parameters,
                                        TurretSpotsPlaceholderView spotsView) {
            _turretsMap = turretsMap;
            _parameters = parameters;
            _spotsView = spotsView;
            _model = session.turretsGrid;
        }
        
        
        public void Tick() {
            for (int spotId = 0; spotId < _model.spots.Length; spotId++) {
                TurretSpotModel spot = _model.spots[spotId];
                
                if (spot.turret == null) {
                    continue;
                }
                
                if (spot.turret.state.Value == TurretModel.State.Removed) {
                    RemoveTurret(spotId, spot.turret);
                    spot.turret = null;
                }
            }
        }
        
        private void RemoveTurret(int spotId, TurretModel model) {
            int removeDirection = spotId + 1 <= _model.spots.Length / 2f ? -1 : 1;
            
            if (HasNeighbourInDirection(spotId, removeDirection)) {
                RemoveUpAndStraight(model, removeDirection);
            } else {
                RemoveStraight(model, removeDirection);
            }
        }
        
        private void RemoveStraight(TurretModel model, int direction) {
            if (_turretsMap.TryGetValue(model, out TurretView view)) {
                Vector3 targetPosition = view.transform.position + Vector3.right * direction * 50;
                float distance = Vector3.Distance(view.transform.position, targetPosition);
                view.transform.DOMove(targetPosition, distance / _parameters.moveSpeed).SetEase(Ease.Linear).OnComplete(view.Hide);
            }
        }
        
        private void RemoveUpAndStraight(TurretModel model, int direction) {
            if (_turretsMap.TryGetValue(model, out TurretView view)) {
                float verticalOffset = _spotsView.verticalSpacing;
                Vector3 verticalPosition = view.transform.position;
                verticalPosition.z += verticalOffset;
                
                Vector3 targetPosition = view.transform.position + Vector3.right * direction * 50;
                targetPosition.z += verticalOffset;
                
                Sequence moveSequence = DOTween.Sequence();
                
                float distance = Vector3.Distance(view.transform.position, verticalPosition);
                moveSequence.Append(view.transform.DOMove(verticalPosition, distance / _parameters.moveSpeed).SetEase(Ease.Linear));
                distance = Vector3.Distance(verticalPosition, targetPosition);
                moveSequence.Append(view.transform.DOMove(targetPosition, distance / _parameters.moveSpeed).SetEase(Ease.Linear));
                
                moveSequence.Play();
                moveSequence.OnComplete(view.Hide);
            }
        }
        
        private bool HasNeighbourInDirection(int startIndex, int direction) {
            for (int i = startIndex + direction; i >= 0 && i < _model.spots.Length; i += direction) {
                if (_model.spots[i] != null) {
                    return true;
                }
            }
            
            return false;
        }
    }
}