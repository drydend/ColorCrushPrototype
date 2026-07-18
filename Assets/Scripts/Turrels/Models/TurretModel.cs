using Cubes;
using R3;
using Turrels.Parameters;
using UnityEngine;

namespace Turrels.Models {
    public class TurretModel {
        public EntityColor color;
        public ReactiveProperty<State> state;
        public ReactiveProperty<int> durationLeft;
        public float shotsPerSec;
        public float lastShotTime;
        public Vector2Int gridPosition;
        public int lastTargetedRow;
        
        public TurretModel(TurretConfig config, float shootPerSec, Vector2Int gridPosition) {
            this.gridPosition = gridPosition;
            color = config.color;
            state = new ReactiveProperty<State>(State.Inactive);
            durationLeft = new ReactiveProperty<int>(config.durability);
            shotsPerSec = shootPerSec;
            lastShotTime = float.MinValue;
            lastTargetedRow = 0;
        }
        
        public enum State {
            Inactive,
            Available,
            Moved,
            Active,
            Removed,
        }
    }
}