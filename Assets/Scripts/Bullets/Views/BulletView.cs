using UnityEngine;

namespace Bullets {
    public class BulletView : MonoBehaviour {
        [field: SerializeField]
        private ParticleSystem _hitParticle;
        
        public void OnHit() {
            ParticleSystem instance = Instantiate(_hitParticle, transform.position, Quaternion.identity);
            instance.Play();
            Destroy(instance.gameObject, 5f);
        }
    }
}