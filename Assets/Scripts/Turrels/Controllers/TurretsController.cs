using System.Collections.Generic;
using Levels.Models;
using Turrels.Models;
using Turrels.Parameters;
using Turrels.Views;
using UnityEngine;
using VContainer.Unity;

namespace Turrels.Controllers {
    public class TurretsController : IStartable, ITickable {
        private readonly TurretsGridView _gridView;
        private readonly TurretsParameters _parameters;
        private readonly LevelSessionModel _level;
        private readonly Dictionary<TurretModel, TurretView> _turretsMap;
        private readonly List<TurretPositionController> _controllers;
        
        public TurretsController(LevelSessionModel level, TurretsGridView turretsGrid, TurretsParameters parameters, Dictionary<TurretModel, TurretView> turretsMap) {
            _level = level;
            _gridView = turretsGrid;
            _parameters = parameters;
            _turretsMap = turretsMap;
            _controllers = new List<TurretPositionController>();
        }
        
        public void Start() {
            List<TurretView> views = _gridView.Initialize(_level.turretsGrid, _parameters);
            
            for (int turretId = 0; turretId < _level.turretsGrid.all.Length; turretId++) {
                TurretPositionController controller = new TurretPositionController(_level.turretsGrid.all[turretId], views[turretId], _gridView);
                _controllers.Add(controller);
                _turretsMap.Add(_level.turretsGrid.all[turretId], views[turretId]);
            }
        }
        
        public void Tick() {
            float delta = _parameters.moveSpeed * Time.deltaTime;
            
            for (int i = 0; i < _controllers.Count; i++) {
                _controllers[i].Tick(delta);
            }
            
            _gridView.Tick();
        }
    }
}