using System.Collections.Generic;
using Turrels.Parameters;
using UnityEngine;

namespace Turrels.Models {
    public class TurretsGridModel {
        public TurretSpotModel[] spots;
        
        public int width;
        public int height;
        public TurretModel[,] grid;
        
        public TurretModel[] all;
        
        public TurretsGridModel(TurretsGridConfig levelConfig, TurretsParameters parameters) {
            spots = new TurretSpotModel[levelConfig.spotsCount];
            
            for (int i = 0; i < levelConfig.spotsCount; i++) {
                spots[i] = new TurretSpotModel();
            }
            
            List<TurretModel> models = new List<TurretModel>();
            Vector2Int size = levelConfig.CalculateSize();
            width = size.x;
            height = size.y;
            
            grid = new TurretModel[width, height];
            
            for (int rowId = 0; rowId < width; rowId++) {
                TurretsRow row = levelConfig.rows[rowId];
                
                for (int collumId = 0; collumId < row.turrets.Length; collumId++) {
                    TurretModel turretModel = new TurretModel(row.turrets[collumId], parameters.shootPerSec, new Vector2Int(rowId, collumId));
                    grid[rowId, collumId] = turretModel;
                    models.Add(turretModel);
                }
            }
            
            all = models.ToArray();
        }
    }
}