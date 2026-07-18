using System.Threading;
using Cysharp.Threading.Tasks;
using Levels.Models;
using Loader;
using UI;
using Utilities;

namespace States {
    public class LevelEnterState : LevelState {
        private readonly LoaderCurtain _loader;
        private readonly LevelSessionModel _session;
        private readonly CanvasAction _action;
        
        private CancellationTokenSource _cancellation;
        
        public LevelEnterState(LevelSessionModel session, LoaderCurtain loader, CanvasAction action) : base(session) {
            _loader = loader;
            _action = action;
            _session = session;
        }
        
        public override void Dispose() {
            base.Dispose();
            
            CancellationUtility.Release(ref _cancellation);
        }
        
        protected override void Enter() {
            _action.Open();
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            StartProcess(_cancellation.Token).Forget();
        }
        
        private async UniTask StartProcess(CancellationToken cancellation) {
            for (int i = 0; i < 3; i++) {
                await UniTask.Yield(cancellation);
            }
            
            await _loader.Close(_cancellation.Token);
            _session.state.Value = LevelSessionModel.State.Action;
        }
        
        protected override void Exit() {
            CancellationUtility.Release(ref _cancellation);
        }
        
        protected override bool IsCurrent(LevelSessionModel.State state) => state == LevelSessionModel.State.LevelEnter;
    }
}