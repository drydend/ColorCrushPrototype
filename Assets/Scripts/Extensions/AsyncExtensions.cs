using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;

namespace Extensions {
    public static class AsyncExtensions {
        public static async UniTask AsyncWaitForCompletion(this Tween t, CancellationToken cancellation) {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }
            
            while (t.active && !t.IsComplete()) {
                await UniTask.Yield(cancellation);
            }
        }
    }
}