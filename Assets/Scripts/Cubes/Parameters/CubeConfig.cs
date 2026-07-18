using System;
using UnityEngine;

namespace Cubes {
    [Serializable]
    public class CubeConfig {
        [field: SerializeField]
        public int id { get; private set; }
        
        public CubeConfig(int id = 0) {
            this.id = id;
        }
    }
}