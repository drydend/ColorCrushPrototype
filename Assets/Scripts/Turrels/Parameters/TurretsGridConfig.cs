using System;
using System.Collections.Generic;
using UnityEngine;

namespace Turrels.Parameters {
    [Serializable]
    public class TurretsGridConfig {
        [field: SerializeField]
        public int spotsCount { get; private set; }
        
        [field: SerializeField]
        public TurretsRow[] rows { get; private set; }
        
        public Vector2Int CalculateSize() {
            int height = 0;
            
            for (int rowId = 0; rowId < rows.Length; rowId++) {
                height = Mathf.Max(height, rows[rowId].turrets.Length);
            }
            
            return new Vector2Int(rows.Length, height);
        }
        
        public void FillRows(TurretsRow[] rows) {
            this.rows = rows;
        }
    }
}