using System;
using UnityEngine;

namespace Levels.Parameters {
    [Serializable]
    public class LayerConfig {
        [field: SerializeField] public RowConfig[] rows { get; private set; } = Array.Empty<RowConfig>();
        
        public void Resize(int width, int length) {
            RowConfig[] result = new RowConfig[length];
            
            for (int y = 0; y < length; y++) {
                RowConfig row = y < rows.Length && rows[y] != null ? rows[y] : new RowConfig();
                row.Resize(width);
                result[y] = row;
            }
            
            rows = result;
        }
    }
}