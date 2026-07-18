using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Services {
    public class SceneLoaderService {
        private const int _ACTION_SCENE_ID = 1;
        
        public async UniTask LoadActionScene(CancellationToken cancellation) {
            await SceneManager.LoadSceneAsync(_ACTION_SCENE_ID).ToUniTask(cancellationToken: cancellation);
        }
    }
}