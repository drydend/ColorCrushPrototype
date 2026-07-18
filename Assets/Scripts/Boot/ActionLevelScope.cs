using System.Collections.Generic;
using Bullets;
using Bullets.Controllers;
using Cubes.Behaviour;
using Cubes.Controllers;
using Input;
using Levels.Controllers;
using Levels.Models;
using Levels.Views;
using ProjectCamera;
using States;
using Turrels.Controllers;
using Turrels.Models;
using Turrels.Views;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Boot {
    public class ActionLevelScope : LifetimeScope {
        [field: SerializeField]
        private CanvasWinWindow _winWindow;
        
        [field: SerializeField]
        private CanvasLoseWindow _loseWindow;
        
        [field: SerializeField]
        private CanvasAction _actionCanvas;
        
        [field: SerializeField]
        private TurretSpotsPlaceholderView _turretSpot;
        
        [field: SerializeField]
        private InputPanel _inputPanel;
        
        [field: SerializeField]
        private BulletsFactoryView _bullets;
        
        [field: SerializeField]
        private TurretsGridView _turretsGrid;
        
        [field: SerializeField]
        private LevelCubesGridView _gridView;
        
        [field: SerializeField]
        private CameraConfigurationView _cameraConfig;
        
        protected override void Configure(IContainerBuilder builder) {
            builder.RegisterComponent(_loseWindow);
            builder.RegisterComponent(_winWindow);
            builder.RegisterComponent(_actionCanvas);
            
            builder.RegisterComponent(_gridView);
            builder.RegisterComponent(_inputPanel);
            builder.RegisterComponent(_turretSpot);
            builder.RegisterComponent(_bullets);
            builder.RegisterComponent(_cameraConfig);
            builder.RegisterComponent(_turretsGrid);
            
            builder.RegisterInstance(new Dictionary<TurretModel, TurretView>());
            
            builder.Register<BulletsModel>(Lifetime.Scoped);
            builder.Register<LevelSessionModel>(Lifetime.Scoped);
            builder.Register<BulletsController>(Lifetime.Scoped);
            
            builder.Register<CubesGridMover>(Lifetime.Scoped);
            builder.Register<TurretsGridMover>(Lifetime.Scoped);
            builder.Register<TurretsSelector>(Lifetime.Scoped);
            
            builder.Register<LevelWindController>(Lifetime.Scoped);
            builder.Register<LevelLoseController>(Lifetime.Scoped);
            
            builder.RegisterEntryPoint<TurretsRemoveControllers>(Lifetime.Scoped);
            builder.RegisterEntryPoint<InputPanelController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<CameraConfigurationController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<CubeDestroyer>(Lifetime.Scoped);
            builder.RegisterEntryPoint<CubePositionController>(Lifetime.Scoped);
            
            builder.RegisterEntryPoint<TurretsController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<TurretShotController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<TurretSpotsPlaceholderController>(Lifetime.Scoped);
            
            builder.RegisterEntryPoint<ActionWindowController>(Lifetime.Scoped);
            
            builder.RegisterEntryPoint<LevelEnterState>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameplayState>(Lifetime.Scoped);
            builder.RegisterEntryPoint<WinState>(Lifetime.Scoped);
            builder.RegisterEntryPoint<LoseState>(Lifetime.Scoped);
        }
    }
}