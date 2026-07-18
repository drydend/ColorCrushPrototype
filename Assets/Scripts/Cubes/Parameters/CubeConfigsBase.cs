using System;
using Cubes.Views;
using UnityEngine;

namespace Cubes {
    [CreateAssetMenu(menuName = "Parameters/CubesConfigBase")]
    public class CubeConfigsBase : ScriptableObject {
        [field: SerializeField]
        public CubePreset[] presets { get; private set; }
        
        public bool TryGetPreset(int id, out CubePreset result) {
            if (presets != null) {
                for (int index = 0; index < presets.Length; index++) {
                    CubePreset preset = presets[index];
                    
                    if (preset != null && preset.id == id) {
                        result = preset;
                        return true;
                    }
                }
            }
            
            result = null;
            return false;
        }
        
        public static CubeConfigsBase LoadFromResources() => Resources.Load<CubeConfigsBase>(nameof(CubeConfigsBase));
        
        [Serializable]
        public class CubePreset {
            [field: SerializeField, Min(0)]
            public int id { get; private set; }
            
            [field: SerializeField]
            public EntityColor color { get; private set; }
            
            [field: SerializeField]
            public CubeView cube { get; private set; }
            
            [field: SerializeField, Min(1)]
            public int health { get; private set; } = 1;
            
            [field: SerializeField]
            public Color previewColor { get; private set; } = Color.white;
        }
    }
}