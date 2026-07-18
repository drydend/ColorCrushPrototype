using System.Collections.Generic;
using Bullets.Controllers;
using Bullets.Parameters;
using Cubes.Models;
using Levels.Models;
using Services.Audio.Players;
using Turrels.Models;
using Turrels.Views;
using UnityEngine;
using VContainer.Unity;

namespace Turrels.Controllers {
    public class TurretShotController : ITickable {
        private readonly LevelSessionModel _session;
        private readonly BulletsController _bulletsController;
        private readonly Dictionary<TurretModel, TurretView> _turretsMap;
        private readonly BulletsParameters _bulletsParameters;
        
        public TurretShotController(LevelSessionModel session, BulletsController bulletsController, Dictionary<TurretModel, TurretView> turretsMap,
                                    BulletsParameters bulletsParameters) {
            _session = session;
            _bulletsController = bulletsController;
            _turretsMap = turretsMap;
            _bulletsParameters = bulletsParameters;
        }
        
        public void Tick() {
            for (int spotId = 0; spotId < _session.turretsGrid.spots.Length; spotId++) {
                UpdateTurret(_session.turretsGrid.spots[spotId].turret);
            }
        }
        
        private void UpdateTurret(TurretModel model) {
            if (model == null) {
                return;
            }
            
            if (model.state.Value != TurretModel.State.Active) {
                return;
            }
            
            float recharge = 1 / model.shotsPerSec;
            
            if (Time.time - model.lastShotTime < recharge) {
                return;
            }
            
            for (int x = model.lastTargetedRow; x - model.lastTargetedRow < _session.cubeGrid.width; x++) {
                int rowId = x % _session.cubeGrid.width;
                CubeModel cube = _session.cubeGrid.grid[rowId, 0, 0];
                
                if (cube == null) {
                    continue;
                }
                
                if (cube.color != model.color) {
                    continue;
                }
                
                if (cube.predictedHealth.Value <= 0) {
                    continue;
                }
                
                if (Mathf.Abs(cube.distanceToTargetLayer) > 0.05f) {
                    continue;
                }
                
                ShotBullet(model, cube);
                model.lastShotTime = Time.time;
                model.lastTargetedRow = rowId + 1;
                model.durationLeft.Value = Mathf.Max(model.durationLeft.Value - 1, 0);
                
                if (model.durationLeft.Value == 0) {
                    model.state.Value = TurretModel.State.Removed;
                }
                
                break;
            }
        }
        
        private void ShotBullet(TurretModel model, CubeModel cube) {
            if (_turretsMap.TryGetValue(model, out TurretView view)) {
                AudioService.instance.player.PlayLimitCooldown(AudioService.instance.parameters.shootSound, Vector3.zero, "Shoot", 0.07f);
                _bulletsController.CreateBullet(view.bulletSpawnPosition.position, cube, _bulletsParameters.defaultBullet);
                view.ApplyDirection((cube.worldPosition - view.transform.position).normalized);
                view.OnShoot();
            }
        }
    }
}