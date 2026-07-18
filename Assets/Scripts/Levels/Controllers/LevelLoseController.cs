using Bullets;
using Bullets.Controllers;
using Levels.Models;
using Services.Audio.Players;
using Turrels.Extensions;
using Turrels.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels.Controllers {
    public class LevelLoseController {
        private readonly BulletsModel _bullets;
        private readonly LevelSessionModel _level;
        
        public LevelLoseController(LevelSessionModel level, BulletsModel bullets) {
            _level = level;
            _bullets = bullets;
        }
        
        public void Update() {
            if (IsLose()) {
                AudioService.instance.player.Play(AudioService.instance.parameters.lose, Vector3.zero);
                _level.state.Value = LevelSessionModel.State.Lose;
            }
        }
        
        private bool IsLose() {
            if (IsAllRowEmpty()) {
                return false;
            }
            
            if (_bullets.all.Count > 0) {
                return false;
            }
            
            if (_level.cubeGrid.aliveCubesLeft.Value <= 0) {
                return false;
            }
            
            for (int spotId = 0; spotId < _level.turretsGrid.spots.Length; spotId++) {
                if (_level.turretsGrid.spots[spotId].IsEmpty() && !IsQueueEmpty()) {
                    return false;
                }
                
                if (_level.turretsGrid.spots[spotId].turret == null) {
                    continue;
                }
                
                if (!IsStuck(_level.turretsGrid.spots[spotId].turret)) {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool IsAllRowEmpty() {
            for (int x = 0; x < _level.cubeGrid.width; x++) {
                if (_level.cubeGrid.grid[x, 0, 0] != null) {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool IsStuck(TurretModel model) {
            for (int x = 0; x < _level.cubeGrid.width; x++) {
                if (_level.cubeGrid.grid[x, 0, 0] == null) {
                    continue;
                }
                
                if (_level.cubeGrid.grid[x, 0, 0].color == model.color) {
                    return false;
                }
            }
            
            return true;
        }
        
        private bool IsQueueEmpty() {
            for (int x = 0; x < _level.turretsGrid.width; x++) {
                for (int y = 0; y < _level.turretsGrid.height; y++) {
                    if (_level.turretsGrid.grid[x, y] != null) {
                        return false;
                    }
                }   
            }
            
            return true;
        }
    }
}