using Levels.Models;
using Progression.Models;
using R3;
using Services.Audio.Players;
using UnityEngine;

namespace Levels.Controllers {
    public class LevelWindController {
        private readonly GameModel _game;
        private readonly LevelSessionModel _level;
        private CompositeDisposable _disposables;
        
        public LevelWindController(LevelSessionModel level, GameModel game) {
            _level = level;
            _game = game;
        }
        
        public void Activate() {
            if (_disposables != null) {
                _disposables.Dispose();
            }
            
            _disposables = new CompositeDisposable();
            _disposables.Add(_level.cubeGrid.aliveCubesLeft.Subscribe(OnAliveCubesCountChanged));
        }
        
        public void Deactivate() {
            _disposables.Dispose();
            _disposables = null;
        }
        
        private void OnAliveCubesCountChanged(int aliveLeft) {
            if (aliveLeft == 0) {
                AudioService.instance.player.Play(AudioService.instance.parameters.win, Vector3.zero);
                _level.state.Value = LevelSessionModel.State.Win;
            }
        }
    }
}