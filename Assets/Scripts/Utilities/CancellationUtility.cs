using System.Threading;
using Unity.VisualScripting;

namespace Utilities {
    public static class CancellationUtility {
        public static void Release(ref CancellationTokenSource cancellation) {
            if (cancellation != null) {
                cancellation.Cancel();
                cancellation.Dispose();
                cancellation = null;
            }
        }
    }
}