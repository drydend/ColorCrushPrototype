using System.Collections.Generic;
using Levels.Models;
using Turrels.Views;
using VContainer.Unity;

namespace Turrels.Controllers {
    public class TurretSpotsPlaceholderController : IStartable {
        private readonly TurretSpotsPlaceholderView _placeholderView;
        private readonly LevelSessionModel _level;
        
        public TurretSpotsPlaceholderController(TurretSpotsPlaceholderView placeholderView, LevelSessionModel level) {
            _placeholderView = placeholderView;
            _level = level;
        }
        
        public void Start() {
            List<TurretSpotView> spots = _placeholderView.Initialize(_level.turretsGrid);
            
            for (int spotId = 0; spotId < spots.Count; spotId++) {
                TurretSpotController spotController = new TurretSpotController(_level.turretsGrid.spots[spotId], spots[spotId]);
                spotController.Initialize();
            }
        }
    }
}