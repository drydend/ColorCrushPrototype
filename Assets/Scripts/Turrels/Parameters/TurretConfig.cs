using System;
using Cubes;
using UnityEngine;

namespace Turrels.Parameters {
    [Serializable]
    public class TurretConfig {
        [field: SerializeField]
        public EntityColor color { get; private set; }
        
        [field: SerializeField]
        public int durability { get; private set; }
        
        public TurretConfig(int durability, EntityColor color) {
            this.durability = durability;
            this.color = color;
        }
    }
}