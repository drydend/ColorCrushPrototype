using System.Collections.Generic;
using Bullets.Parameters;
using Cubes.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bullets.Controllers {
    public class BulletsController {
        private readonly Dictionary<BulletModel, BulletView> _bulletsMap;
        private readonly BulletsFactoryView _viewFactory;
        private readonly BulletsModel _bullets;
        
        public BulletsController(BulletsModel bullets, BulletsFactoryView view) {
            _bullets = bullets;
            _viewFactory = view;
            _bulletsMap = new Dictionary<BulletModel, BulletView>();
        }
        
        public void CreateBullet(Vector3 position, CubeModel target, BulletsParameters.BulletConfig config) {
            BulletModel model = new BulletModel(target, position, config);
            target.predictedHealth.Value -= 1;
            
            BulletView view = _viewFactory.Create(model);
            _bullets.all.Add(model);
            _bulletsMap.Add(model, view);
        }
        
        public void Update() {
            for (int bulletId = 0; bulletId < _bullets.all.Count;) {
                if (MoveAndCheckCollision(_bullets.all[bulletId])) {
                    ApplyDamage(_bullets.all[bulletId]);
                    DestroyBullet(_bullets.all[bulletId]);
                    _bullets.all.RemoveAt(bulletId);
                    continue;
                }
                
                bulletId++;
            }
        }
        
        private void ApplyDamage(BulletModel model) {
            model.target.currentHealth.Value = Mathf.Max(0, model.target.currentHealth.Value - 1);
        }
        
        private void DestroyBullet(BulletModel model) {
            if (_bulletsMap.Remove(model, out BulletView view)) {
                view.OnHit();
                Object.Destroy(view.gameObject);
            }
        }
        
        private bool MoveAndCheckCollision(BulletModel model) {
            float delta = model.speed * Time.deltaTime;
            Vector3 position = Vector3.MoveTowards(model.position, model.target.worldPosition, delta);
            model.position = position;
            
            UpdateView(model);
            
            if (IsSphereInsideCube(position, model.size, model.target.worldPosition, model.target.size / 2)) {
                return true;
            }
            
            return false;
        }
        
        private void UpdateView(BulletModel model) {
            if (_bulletsMap.TryGetValue(model, out BulletView view)) {
                view.transform.position = model.position;
            }
        }
        
        public static bool IsSphereInsideCube(Vector3 sphereCenter, float sphereRadius, Vector3 cubeCenter, float cubeHalfSize) {
            Vector3 localPos = sphereCenter - cubeCenter;
            
            return Mathf.Abs(localPos.x) + sphereRadius <= cubeHalfSize && Mathf.Abs(localPos.y) + sphereRadius <= cubeHalfSize
                && Mathf.Abs(localPos.z) + sphereRadius <= cubeHalfSize;
        }
    }
}