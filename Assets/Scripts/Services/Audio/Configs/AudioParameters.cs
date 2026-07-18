using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio.Configs {
    [CreateAssetMenu(menuName = "Parameters/" + _PATH, fileName = _PATH)]
    public class AudioParameters : ScriptableObject {
        [field: SerializeField]
        public AudioConfig music { get; private set; }
        
        [field: SerializeField]
        public AudioConfig shootSound { get; private set; }
        
        [field: SerializeField]
        public AudioConfig buttonClick { get; private set; }
        
        [field: SerializeField]
        public AudioConfig turretSelect { get; private set; }
        
        [field: SerializeField]
        public AudioConfig lose { get; private set; }
        
        [field: SerializeField]
        public AudioConfig win { get; private set; }
        
        [field: SerializeField]
        public Sources sources { get; private set; }
        
        [field: SerializeField]
        public Mixers mixers { get; private set; }
        
        [field: SerializeField]
        public float distanceMin { get; private set; } = 0.05f;
        
        [field: SerializeField]
        public float distanceMax { get; private set; } = 7.5f;
        
        private const string _PATH = nameof(AudioParameters);
        
        public static AudioParameters LoadFromResources() => Resources.Load<AudioParameters>(_PATH);
        
        [Serializable]
        public sealed class Sources {
            [field: SerializeField]
            public AudioSource single { get; private set; }
            
            [field: SerializeField]
            public AudioSource loop { get; private set; }
        }
        
        [Serializable]
        public sealed class Mixers {
            [field: SerializeField]
            public AudioMixer music { get; private set; }
            
            [field: SerializeField]
            public AudioMixer soundScaled { get; private set; }
            
            [field: SerializeField]
            public AudioMixer soundUnscaled { get; private set; }
        }
    }
}