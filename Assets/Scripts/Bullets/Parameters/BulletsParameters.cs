using System;
using UnityEngine;

namespace Bullets.Parameters {
    [CreateAssetMenu(menuName = "Parameters/" + _PATH, fileName = _PATH)]
    public class BulletsParameters : ScriptableObject {
        [field: SerializeField]
        public BulletConfig defaultBullet { get; private set; }
        
        [Serializable]
        public class BulletConfig {
            [field: SerializeField]
            public float speed { get; private set; } = 15f;
            
            [field: SerializeField]
            public float size { get; private set; } = 0.3f;
        }
        
        private const string _PATH = nameof(BulletsParameters);
        
        public static BulletsParameters LoadFromResources() => Resources.Load<BulletsParameters>(_PATH);
    }
}