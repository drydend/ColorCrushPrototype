using R3;
using UnityEngine;

namespace Cubes.Models {
    public class CubeModel {
        public int configId;
        public float size;
        public bool isDestroyed;
        public EntityColor color;
        
        public ReactiveProperty<int> currentHealth;
        public ReactiveProperty<int> predictedHealth;
        
        public Vector3Int gridPosition;
        public Vector3 worldPosition;
        public float distanceToTargetLayer;
        
        public CubeModel(CubeConfigsBase.CubePreset config, Vector3Int gridPosition) {
            configId = config.id;
            size = 1;
            currentHealth = new ReactiveProperty<int>(config.health);
            predictedHealth = new ReactiveProperty<int>(config.health);
            color = config.color;
            this.gridPosition = gridPosition;
            worldPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            distanceToTargetLayer = float.MaxValue;
        }
    }
}