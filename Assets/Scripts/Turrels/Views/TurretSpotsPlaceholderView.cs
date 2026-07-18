using System.Collections.Generic;
using Progression;
using Turrels.Models;
using Turrels.Parameters;
using UnityEngine;

namespace Turrels.Views {
    public class TurretSpotsPlaceholderView : MonoBehaviour {
        [field: SerializeField]
        public Transform parent { get; private set; }
        
        [field: SerializeField]
        public float verticalSpacing { get; private set; }
        
        [field: SerializeField]
        private float _spacing;
        
        [field: SerializeField]
        private TurretSpotView _prefab;
        
        public List<TurretSpotView> spots { get; private set; }
        
        public List<TurretSpotView> Initialize(TurretsGridModel grid) {
            spots = new List<TurretSpotView>();
            
            int count = grid.spots.Length;
            
            float offset = (count - 1) * 0.5f;
            
            for (int i = 0; i < count; i++) {
                float x = (i - offset) * _spacing;
                
                Vector3 position = transform.position + Vector3.right * x;
                
                TurretSpotView instance = Instantiate(_prefab, position, Quaternion.identity, transform);
                
                spots.Add(instance);
            }
            
            return spots;
        }
    }
}