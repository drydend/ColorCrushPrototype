using System.Threading;
using Cubes.Behaviour;
using Cysharp.Threading.Tasks;
using Levels.Models;
using UI;
using Utilities;

namespace States {
    public class WinState : LevelState {
        private readonly CanvasWinWindow _winWindow;
        private CancellationTokenSource _cancellation;
        
        public WinState(LevelSessionModel session, CanvasWinWindow winWindow) : base(session) {
            _winWindow = winWindow;
        }
        
        public override void Dispose() {
            base.Dispose();
            
            CancellationUtility.Release(ref _cancellation);
        }
        
        protected override void Enter() {
            _winWindow.Open();
        }
        
        protected override void Exit() {
            _winWindow.Close();
        }
        
        protected override bool IsCurrent(LevelSessionModel.State state) => state == LevelSessionModel.State.Win;
    }
}