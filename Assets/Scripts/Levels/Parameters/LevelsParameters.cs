using System;
using Cubes;
using UnityEngine;

namespace Levels.Parameters {
    [CreateAssetMenu(menuName = "Parameters/" + _PATH, fileName = _PATH)]
    public class LevelsParameters : ScriptableObject {
        [field: SerializeField]
        public LevelConfig[] levels { get; private set; } = Array.Empty<LevelConfig>();
        
        private const string _PATH = nameof(LevelsParameters);
        
        public static LevelsParameters LoadFromResources() => Resources.Load<LevelsParameters>(_PATH);
        
        public void AddLevel() {
            LevelConfig[] result = new LevelConfig[levels.Length + 1];
            Array.Copy(levels, result, levels.Length);
            
            LevelConfig level = new LevelConfig();
            level.AddLayer();
            
            result[^1] = level;
            levels = result;
        }
        
        public void RemoveLevel(int index) {
            if (index < 0 || index >= levels.Length)
                return;
            
            LevelConfig[] result = new LevelConfig[levels.Length - 1];
            
            Array.Copy(levels, 0, result, 0, index);
            Array.Copy(levels, index + 1, result, index, levels.Length - index - 1);
            
            levels = result;
        }
    }
}