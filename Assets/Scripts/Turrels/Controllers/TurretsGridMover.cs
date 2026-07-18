using Levels.Models;
using Turrels.Models;
using UnityEngine;

namespace Turrels.Controllers {
    public class TurretsGridMover {
        private readonly TurretsGridModel _grid;
        
        public TurretsGridMover(LevelSessionModel session) {
            _grid = session.turretsGrid;
        }
        
        public void Update() {
            for (int x = 0; x < _grid.width; x++) {
                int emptyY = 0;
                
                for (int y = 0; y < _grid.height; y++) {
                    if (_grid.grid[x, y] == null) {
                        emptyY = y;
                        
                        for (int targetY = y + 1; targetY < _grid.height; targetY++) {
                            if (_grid.grid[x, targetY] != null) {
                                _grid.grid[x, emptyY] = _grid.grid[x, targetY];
                                _grid.grid[x, targetY] = null;
                                
                                _grid.grid[x, emptyY].gridPosition = new Vector2Int(x, emptyY);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}