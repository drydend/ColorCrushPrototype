using System.Threading;
using Cubes.Behaviour;
using Cysharp.Threading.Tasks;
using Levels.Models;
using UI;
using Utilities;

namespace States {
    public class LoseState : LevelState {
        private CanvasLoseWindow _loseWindow;
        
        private CancellationTokenSource _cancellation;
        
        public LoseState(LevelSessionModel session, CanvasLoseWindow loseWindow) : base(session) {
            _loseWindow = loseWindow;
        }
        
        public override void Dispose() {
            base.Dispose();
            
            CancellationUtility.Release(ref _cancellation);
        }
        
        protected override void Enter() {
            _loseWindow.Open();
        }
        
        protected override void Exit() {
            _loseWindow.Close();
        }
        
        
        protected override bool IsCurrent(LevelSessionModel.State state) => state == LevelSessionModel.State.Lose;
    }
}