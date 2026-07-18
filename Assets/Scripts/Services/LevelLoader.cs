using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Loader;
using Progression.Models;
using Utilities;

namespace Services {
    public class LevelLoader : IDisposable {
        private readonly GameModel _game;
        private readonly SceneLoaderService _sceneLoader;
        private readonly LoaderCurtain _curtain;
        
        private CancellationTokenSource _cancellation;
        
        public LevelLoader(LoaderCurtain curtain, SceneLoaderService sceneLoader, GameModel game) {
            _curtain = curtain;
            _sceneLoader = sceneLoader;
            _game = game;
        }
        
        public void Dispose() {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }
        
        public void RestartCurrentLevel() {
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            LoadProcess(_game.currentLevelId, _cancellation.Token).Forget();
        }
        
        public void LoadStartLevel() {
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            LoadProcess(0, _cancellation.Token).Forget();
        }
        
        public void LoadNextLevel() {
            CancellationUtility.Release(ref _cancellation);
            _cancellation = new CancellationTokenSource();
            LoadProcess(_game.currentLevelId + 1, _cancellation.Token).Forget();
        }
        
        private async UniTask LoadProcess(int levelId, CancellationToken cancellation) {
            _game.currentLevelId = levelId;
            await _curtain.Open(cancellation);
            await UniTask.Delay(500, true, cancellationToken: cancellation);
            await _sceneLoader.LoadActionScene(cancellation);
        }
    }
}