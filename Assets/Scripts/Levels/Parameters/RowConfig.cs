using System;
using Cubes;
using UnityEngine;

namespace Levels.Parameters {
    [Serializable]
    public class RowConfig {
        [field: SerializeField]
        public CubeConfig[] cubes { get; private set; } = Array.Empty<CubeConfig>();
        
        public void Resize(int width) {
            CubeConfig[] result = new CubeConfig[width];
            
            for (var x = 0; x < width; x++) {
                result[x] = x < cubes.Length && cubes[x] != null ? cubes[x] : new CubeConfig();
            }
            
            cubes = result;
        }
    }
}