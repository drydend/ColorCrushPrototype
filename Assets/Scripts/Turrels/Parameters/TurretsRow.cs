using System;
using UnityEngine;

namespace Turrels.Parameters {
    [Serializable]
    public class TurretsRow {
        [field: SerializeField]
        public TurretConfig[] turrets { get; private set; }
        
        public TurretsRow(TurretConfig[] turrets) {
            this.turrets = turrets;
        }
    }
}