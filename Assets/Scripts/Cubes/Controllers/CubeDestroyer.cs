using System;
using Cubes.Models;
using Extensions;
using Levels.Models;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using VContainer.Unity;
using CompositeDisposable = R3.CompositeDisposable;

namespace Cubes.Controllers {
    public class CubeDestroyer : IStartable, IDisposable {
        private readonly CubeGridModel _cubeGrid;
        private readonly CompositeDisposable _disposables;
        
        public CubeDestroyer(LevelSessionModel session) {
            _cubeGrid = session.cubeGrid;
            _disposables = new CompositeDisposable();
        }
        
        public void Start() {
            for (int i = 0; i < _cubeGrid.all.Length; i++) {
                CubeModel model = _cubeGrid.all[i];
                OnHealthChanged(model.currentHealth.Value, model);
                _disposables.Add(model.currentHealth.Subscribe(health => OnHealthChanged(health, model)));
            }
        }
        
        public void Dispose() => _disposables.Dispose();
        
        private void OnHealthChanged(int health, CubeModel model) {
            if (health <= 0 && !model.isDestroyed) {
                model.isDestroyed = true;
                _cubeGrid.aliveCubesLeft.Value = Mathf.Max(0, _cubeGrid.aliveCubesLeft.Value - 1);
                Vector3Int gridPosition = model.gridPosition;
                _cubeGrid.grid[gridPosition.x, gridPosition.y, gridPosition.z] = null;
            }
        }
    }
}