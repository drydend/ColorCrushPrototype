using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Loader;
using Services;
using Services.Audio.Players;
using VContainer.Unity;

namespace Boot.Controllers {
    public class GameEntryPoint : IStartable, IDisposable {
        private readonly LoaderCurtain _curtain;
        private readonly LevelLoader _levelLoader;
        
        private CancellationTokenSource _cancellation;
        
        public GameEntryPoint(LoaderCurtain curtain, LevelLoader levelLoader) {
            _curtain = curtain;
            _levelLoader = levelLoader;
            
        }
        
        public void Start() {
            _cancellation = new CancellationTokenSource();
            LoadProcess(_cancellation.Token).Forget();
        }
        
        private async UniTask LoadProcess(CancellationToken cancellation) {
            _curtain.ForceOpen();
            
            InitializeServices();
            
            _levelLoader.LoadStartLevel();
        }
        
        private void InitializeServices() {
            PauseService.Initialize();
            
            AudioService.instance.Init();
        }
        
        public void Dispose() {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }
    }
}