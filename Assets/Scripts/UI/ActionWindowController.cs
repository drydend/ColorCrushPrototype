using System;
using Levels.Models;
using Progression.Models;
using ProjectCamera;
using Services;
using VContainer.Unity;

namespace UI {
    public class ActionWindowController : IStartable, IDisposable {
        private readonly CanvasLoseWindow _loseWindow;
        private readonly CanvasWinWindow _winWindow;
        private readonly CanvasAction _action;
        
        private readonly CameraView _cameraView;
        private readonly LevelLoader _levelLoader;
        private readonly GameModel _game;
        
        public ActionWindowController(CanvasAction action, CanvasWinWindow winWindow, CanvasLoseWindow loseWindow, CameraView cameraView, LevelLoader levelLoader, GameModel game) {
            _action = action;
            _winWindow = winWindow;
            _loseWindow = loseWindow;
            _cameraView = cameraView;
            _levelLoader = levelLoader;
            _game = game;
        }
        
        public void Start() {
            _loseWindow.Initialize(_cameraView, _levelLoader);
            _winWindow.Initialize(_cameraView, _levelLoader);
            _action.Initialize(_cameraView, _game.currentLevelId);
            
            _loseWindow.Close();
            _winWindow.Close();
            _action.Close();
        }
        
        public void Dispose() { }
    }
}