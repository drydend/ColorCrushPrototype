using Bullets.Parameters;
using Cubes.Models;
using UnityEngine;

namespace Bullets {
    public class BulletModel {
        public CubeModel target;
        public Vector3 position;
        public float speed;
        public float size;
        
        public BulletModel(CubeModel target, Vector3 position, BulletsParameters.BulletConfig config) {
            this.target = target;
            this.position = position;
            this.speed = config.speed;
            this.size = config.size;
        }
    }
}