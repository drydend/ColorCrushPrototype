using System.Collections.Generic;
using Turrels.Models;
using Turrels.Parameters;
using UnityEngine;

namespace Turrels.Views {
    public class TurretsGridView : MonoBehaviour {
        [field: SerializeField]
        private Vector2 _spacing;
        
        [field: SerializeField]
        private Transform _origin;
        
        private int _width;
        private int _height;
        private List<TurretView> _turrets;
        
        public List<TurretView> Initialize(TurretsGridModel model, TurretsParameters parameters) {
            _turrets = new List<TurretView>();
            
            _width = model.width;
            _height = model.height;
            
            
            for (int turretId = 0; turretId < model.all.Length; turretId++) {
                TurretModel turretModel = model.all[turretId];
                TurretView prefab = parameters.Get(turretModel.color);
                TurretView instance = Instantiate(prefab, CalculateTurretPosition(turretModel.gridPosition), Quaternion.identity, _origin);
                instance.Initialize(turretModel);
                _turrets.Add(instance);
            }
            
            return _turrets;
        }
        
        public void Tick() {
            for (int turretId = 0; turretId < _turrets.Count; turretId++) {
                _turrets[turretId].Tick();
            }
        }
        
        public Vector3 CalculateTurretPosition(Vector2Int gridPosition) {
            float offsetX = (_width - 1) * 0.5f;
            float offsetZ = _height - 1;
            
            float posX = (gridPosition.x - offsetX) * _spacing.x;
            float posZ = gridPosition.y * _spacing.y;
            
            return _origin.position + Vector3.right * posX + Vector3.forward * posZ * -1;
        }
    }
}