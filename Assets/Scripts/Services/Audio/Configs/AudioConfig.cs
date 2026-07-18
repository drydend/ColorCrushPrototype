using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio.Configs {
    [Serializable]
    public class AudioConfig {
        [field: SerializeField]
        public AudioClip clip { get; private set; }
        
        [field: SerializeField]
        public AudioMixerGroup mixer { get; private set; }
        
        [field: SerializeField, Range(0, 1f)]
        public float volume { get; private set; } = 1f;
    }
}