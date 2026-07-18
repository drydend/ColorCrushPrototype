using Progression;
using Turrels.Models;
using UnityEngine;

namespace Turrels.Views {
    public class TurretSpotView : MonoBehaviour {
        public TurretSpotModel model { get; private set; }
        
        public void Initialize(TurretSpotModel model) {
            this.model = model;
        }
    }
}