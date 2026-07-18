using System.Threading;
using Bullets.Controllers;
using Cubes.Behaviour;
using Cysharp.Threading.Tasks;
using Levels.Controllers;
using Levels.Models;
using Loader;
using Turrels.Controllers;
using Utilities;

namespace States {
    public class GameplayState : LevelState {
        private readonly LoaderCurtain _loader;
        private readonly CubesGridMover _cubesGridMove;
        private readonly TurretsGridMover _turretsGridMover;
        private readonly TurretsSelector _selector;
        private readonly BulletsController _bulletsController;
        
        private readonly LevelWindController _windController;
        private readonly LevelLoseController _loseController;
        
        private CancellationTokenSource _cancellation;
        
        public GameplayState(LevelSessionModel session, CubesGridMover cubesGridMove, LoaderCurtain loader, TurretsSelector selector, TurretsGridMover turretsGridMover, BulletsController bulletsController, LevelLoseController loseController, LevelWindController windController) : base(session) {
            _cubesGridMove = cubesGridMove;
            _loader = loader;
            _selector = selector;
            _turretsGridMover = turretsGridMover;
            _bulletsController = bulletsController;
            _loseController = loseController;
            _windController = windController;
        }
        
        public override void Dispose() {
            base.Dispose();
            
            CancellationUtility.Release(ref _cancellation);
        }
        
        protected override void Enter() {
            _selector.Activate();
            _windController.Activate();
            
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            _loader.Close(_cancellation.Token).Forget();
            
            UpdateProcess(_cancellation.Token).Forget();
        }
        
        protected override void Exit() {
            _selector.Deactivate();
            _windController.Deactivate();
            
            CancellationUtility.Release(ref _cancellation);
        }
        
        private async UniTask UpdateProcess(CancellationToken cancellation) {
            while (!cancellation.IsCancellationRequested) {
                _cubesGridMove.Update();
                _turretsGridMover.Update();
                _bulletsController.Update();
                
                _loseController.Update();
                
                await UniTask.Yield();
            }
        }
        
        protected override bool IsCurrent(LevelSessionModel.State state) => state == LevelSessionModel.State.Action;
    }
}