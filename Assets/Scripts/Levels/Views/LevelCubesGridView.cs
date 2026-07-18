using System.Collections.Generic;
using Cubes;
using Cubes.Models;
using Cubes.Views;
using Levels.Models;
using UnityEngine;

namespace Levels.Views {
    public class LevelCubesGridView : MonoBehaviour {
        [field: SerializeField]
        private Transform _cubesParent;
        
        private List<CubeView> _cubes;
        
        public List<CubeView> CreateViews(CubeGridModel grid, CubeConfigsBase cubeConfigs) {
            _cubes = new List<CubeView>();
            
            for (int i = 0; i < grid.all.Length; i++) {
                CubeModel cubeModel = grid.all[i];
                cubeConfigs.TryGetPreset(cubeModel.configId, out CubeConfigsBase.CubePreset config);
                CubeView instance = Instantiate(config.cube, GetTargetPositionForCubeOnGrid(cubeModel.gridPosition), Quaternion.identity, _cubesParent);
                instance.Initialize(cubeModel);
                _cubes.Add(instance);
            }
            
            return _cubes;
        }
        
        public Vector3 GetTargetPositionForCubeOnGrid(Vector3Int cubePosition) {
            Vector3 offset = transform.right * cubePosition.x + transform.forward * cubePosition.z + transform.up * cubePosition.y;
            return _cubesParent.transform.position + offset;
        }
    }
}