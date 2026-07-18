using Cubes.Models;
using Levels.Models;
using UnityEngine;

namespace Cubes.Behaviour {
    public class CubesGridMover {
        private readonly CubeGridModel _cubeGrid;
        
        public CubesGridMover(LevelSessionModel session) {
            _cubeGrid = session.cubeGrid;
        }
        
        public CubesGridMover(CubeGridModel cubeGrid) {
            _cubeGrid = cubeGrid;
        }
        
        public void Update() {
            for (int x = 0; x < _cubeGrid.width; x++) {
                UpdateRow(x);
            }
        }
        
        private void UpdateRow(int x) {
            int writeZ = 0;
            
            for (int z = 0; z < _cubeGrid.depth; z++) {
                CubeModel cube = _cubeGrid.grid[x, 0, z];
                
                if (cube == null) {
                    TryMoveDown(x, z);
                    cube = _cubeGrid.grid[x, 0, z];
                }
                
                if (cube != null) {
                    if (z != writeZ) {
                        TryMoveTower(x, z, writeZ);
                        cube = _cubeGrid.grid[x, 0, z];
                    }
                    
                    writeZ++;
                }
            }
        }
        
        private void TryMoveTower(int x, int z, int newZ) {
            for (int y = 0; y < _cubeGrid.height; y++) {
                CubeModel cube = _cubeGrid.grid[x, y, z];
                
                _cubeGrid.grid[x, y, z] = null;
                _cubeGrid.grid[x, y, newZ] = cube;
                
                if (cube != null) {
                    cube.gridPosition = new Vector3Int(x, y, newZ);
                }
            }
        }
        
        private void TryMoveDown(int x, int z) {
            int emptyCellY = 0;
            
            for (int y = 0; y < _cubeGrid.height; y++) {
                CubeModel cube = _cubeGrid.grid[x, y, z];
                
                if (cube == null) {
                    continue;
                }
                
                if (emptyCellY != y) {
                    _cubeGrid.grid[x, emptyCellY, z] = cube;
                    _cubeGrid.grid[x, y, z] = null;
                    
                    cube.gridPosition = new Vector3Int(x, emptyCellY, z);
                }
                
                emptyCellY++;
            }
        }
    }
}