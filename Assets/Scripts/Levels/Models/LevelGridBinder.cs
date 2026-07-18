using System.Collections.Generic;
using Cubes;
using Cubes.Models;
using Levels.Parameters;
using R3;
using UnityEngine;

namespace Levels.Models {
    public class LevelGridBinder {
        public CubeGridModel Bind(CubeConfigsBase configsBase, LevelConfig config) {
            CubeGridModel gridModel = new CubeGridModel();
            
            Vector3Int size = CalculateSize(config);
            CubeModel[,,] models = new CubeModel[size.x, size.y, size.z];
            List<CubeModel> allCubes = new List<CubeModel>();
            
            for (int layerId = 0; layerId < config.layers.Length; layerId++) {
                LayerConfig layer = config.layers[layerId];
                
                for (int rowId = 0; rowId < layer.rows.Length; rowId++) {
                    RowConfig row = layer.rows[rowId];
                    
                    for (int cubeId = 0; cubeId < row.cubes.Length; cubeId++) {
                        CubeConfig cube = row.cubes[cubeId];
                        
                        if (!configsBase.TryGetPreset(cube.id, out CubeConfigsBase.CubePreset preset)) {
                            Debug.LogError($"Missing cube preset: {cube.id}");
                            continue;
                        }
                        
                        Vector3Int position = new(cubeId, layerId, rowId);
                        
                        CubeModel cubeModel = new CubeModel(preset, position);
                        
                        models[cubeId, layerId, rowId] = cubeModel;
                        allCubes.Add(cubeModel);
                    }
                }
            }
            
            gridModel.width = size.x;
            gridModel.height = size.y;
            gridModel.depth = size.z;
            
            gridModel.grid = models;
            gridModel.all = allCubes.ToArray();
            gridModel.aliveCubesLeft = new ReactiveProperty<int>(gridModel.all.Length);
            
            return gridModel;
        }
        
        private Vector3Int CalculateSize(LevelConfig config) {
            Vector3Int size = new Vector3Int(0, 0, 0);
            size.y = Mathf.Max(size.y, config.layers.Length);
            
            for (int layerId = 0; layerId < config.layers.Length; layerId++) {
                size.z = Mathf.Max(size.z, config.layers[layerId].rows.Length);
                
                for (int rowId = 0; rowId < config.layers[layerId].rows.Length; rowId++) {
                    RowConfig row = config.layers[layerId].rows[rowId];
                    size.x = Mathf.Max(row.cubes.Length, size.x);
                }
            }
            
            return size;
        }
    }
}