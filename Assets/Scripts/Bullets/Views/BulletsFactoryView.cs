using UnityEngine;

namespace Bullets {
    public class BulletsFactoryView : MonoBehaviour {
        [field: SerializeField]
        private BulletView _prefab;
        
        public BulletView Create(BulletModel model) {
            BulletView instance = Instantiate(_prefab, model.position, Quaternion.identity, transform);
            return instance;
        }
    }
}