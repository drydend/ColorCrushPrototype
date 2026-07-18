using System;
using Turrels.Models;
using Turrels.Views;

namespace Turrels.Controllers {
    public class TurretSpotController {
        private readonly TurretSpotModel _model;
        private readonly TurretSpotView _view;
        
        public TurretSpotController(TurretSpotModel model, TurretSpotView view) {
            _model = model;
            _view = view;
        }
        
        public void Initialize() {
            _view.Initialize(_model);
            
            _model.position = _view.transform.position;
        }
    }
}