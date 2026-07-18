using System;
using System.Collections.Generic;
using Cubes;
using Cubes.Controllers;
using Cubes.Views;
using Levels.Models;
using Levels.Views;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace Levels.Controllers {
    public class CubePositionController : IStartable, IDisposable, ITickable {
        private readonly LevelCubesGridView _gridView;
        private readonly CubeConfigsBase _cubeConfigs;
        private readonly LevelSessionModel _session;
        private readonly CompositeDisposable _disposables;
        private List<CubeController> _controllers;
        
        public CubePositionController(LevelSessionModel session, LevelCubesGridView gridView, CubeConfigsBase cubeConfigs) {
            _session = session;
            _gridView = gridView;
            _cubeConfigs = cubeConfigs;
            _disposables = new CompositeDisposable();
            _controllers = new List<CubeController>();
        }
        
        public void Start() {
            List<CubeView> views = _gridView.CreateViews(_session.cubeGrid, _cubeConfigs);
            
            for (int cubeId = 0; cubeId < _session.cubeGrid.all.Length; cubeId++) {
                CubeController controller = new CubeController(_session.cubeGrid.all[cubeId], views[cubeId], _gridView);
                _controllers.Add(controller);
                _disposables.Add(controller);
            }
        }
        
        public void Tick() {
            float delta = 1 / 0.2f * Time.deltaTime;
            
            for (int i = 0; i < _controllers.Count; i++) {
                _controllers[i].MoveCube(delta);
            }
        }
        
        public void Dispose() {
            _disposables.Dispose();
        }
    }
}