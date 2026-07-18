using System;
using Cubes;
using Turrels.Views;
using UnityEngine;

namespace Turrels.Parameters {
    [CreateAssetMenu(menuName = "Parameters/" + _PATH, fileName = _PATH)]
    public class TurretsParameters : ScriptableObject {
        [field: SerializeField]
        public float shootPerSec { get; private set; } = 10;
        
        [field: SerializeField]
        public float moveSpeed { get; private set; } = 10;
        
        [field: SerializeField]
        public TurretView defaultView { get; private set; }
        
        [field: SerializeField]
        public Config[] configs { get; private set; }
        
        [Serializable]
        public class Config {
            [field: SerializeField]
            public EntityColor color { get; private set; }
            
            [field: SerializeField]
            public TurretView turret { get; private set; }
        }
        
        public TurretView Get(EntityColor color) {
            for (int i = 0; i < configs.Length; i++) {
                if (configs[i].color == color) {
                    return configs[i].turret;
                }
            }
            
            return null;
        }
        
        private const string _PATH = nameof(TurretsParameters);
        
        public static TurretsParameters LoadFromResources() => Resources.Load<TurretsParameters>(_PATH);
    }
}